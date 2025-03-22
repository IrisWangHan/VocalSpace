using System.Text;
using System.Web;
using VocalSpace.Models;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;  //  這裡是要使用 SHA256

namespace VocalSpace.Services
{
    public class DonateService 
    {
       private readonly VocalSpaceDbContext _context;
        //   從 User Secrets 讀取綠界的HashKey / HashIV
        private readonly IConfiguration? _config;
        public DonateService (VocalSpaceDbContext context, IConfiguration? config)
        {
            _context = context;
            _config = config;          
        }
        //  檢查是否贊助給自己，alert('不能贊助給自己')
        public async Task<string> CheckUser(long? SponsorId, string ArtistName, string SongId)
        {
            string script = string.Empty;
            var Sponsor = await _context.Users.FirstOrDefaultAsync(user => user.UserId == SponsorId);
            string SponsorName = Sponsor?.UserName!; 
            if (SponsorName.Equals(ArtistName, StringComparison.OrdinalIgnoreCase))
            {
                script = $@"
                            <script>                               
                                window.location.href='{_config?["ECPay:website"]}Song/{SongId}';
                                alert('不能贊助給自己');
                            </script>";            
            }
            return script;
        }

        //  新增訂單到資料庫
        public async Task AddOrderToDbAsync(Dictionary<string, string> order)
        {
            
            int SponsorId = int.Parse(order["CustomField1"]);
            int SongId = int.Parse(order["CustomField2"]);
            //  查詢歌手id
            var Song = await _context.Songs.FirstOrDefaultAsync(id => id.SongId == SongId);
            long ReceiverId = Song!.Artist;
            try
            {
                Ecpay Orders = new Ecpay();

                Orders.MerchantTradeNo = order["MerchantTradeNo"];
                Orders.SponsorId = SponsorId;
                Orders.ReceiverId = ReceiverId;
                Orders.ItemName = order["ItemName"];
                Orders.RtnCode = 0;     //未付款
                Orders.RtnMsg = "訂單成功尚未付款";
                Orders.TradeAmt = int.Parse(order["TotalAmount"]);
                Orders.PaymentType = order["PaymentType"];
                Orders.PaymentTypeChargeFee = "0";
                Orders.MerchantTradeDate = order["MerchantTradeDate"];

                await _context.Ecpays.AddAsync(Orders);
                await _context.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddOrderToDbAsync錯誤:{ex}");
            }
            
        }
        // 產生檢查碼
        public string GetCheckMacValue(Dictionary<string, string> order)
        {
            var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();
            var checkValue = string.Join("&", param);
            //測試用的 HashKey
            var hashKey = _config?["ECPay:HashKey"];
            //測試用的 HashIV
            var HashIV = _config?["ECPay:HashIV"];
            checkValue = $"HashKey={hashKey}&" + checkValue + $"&HashIV={HashIV}";
            checkValue = HttpUtility.UrlEncode(checkValue).ToLower();
            checkValue = GetSHA256(checkValue);
            return checkValue.ToUpper();
        }
        private string GetSHA256(string value)
        {
            var result = new StringBuilder();
            var sha256 = SHA256.Create();
            //  把字串轉成byte陣列
            byte[] bts = Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bts);
            //  ToString("X2") : 每個byte轉成兩位大寫的十六進位符號。
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        
        //  產生綠界表單(自動送出)
        public string PrepareECPayForm(Dictionary<string, string> order)
        {
            var form = new StringBuilder();
            form.AppendLine("<form id='form' name='form' method='POST' action='https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5'>");
            foreach (var item in order)
            {
                form.AppendLine($"<input type='hidden' name='{item.Key}' value='{item.Value}' />");
            }
            form.AppendLine("</form>");
            form.AppendLine("<script>document.getElementById('form').submit();</script>");
            return form.ToString();
        }
        //  付款結果更新資料庫
        public async Task UpdatePayInfoAsync(Dictionary<string, string> collection)
        {
            string temp = collection["MerchantTradeNo"];
            var result = await _context.Ecpays.FirstOrDefaultAsync(data => data.MerchantTradeNo == temp);
            if (result != null)
            {
                result.RtnCode = int.Parse(collection["RtnCode"]);
                result.RtnMsg = collection["RtnMsg"];
                result.EcpayTradeNo = collection["TradeNo"];
                result.TradeAmt = int.Parse(collection["TradeAmt"]);
                result.PaymentDate = Convert.ToDateTime(collection["PaymentDate"]);
                result.PaymentType = collection["PaymentType"];
                result.PaymentTypeChargeFee = collection["PaymentTypeChargeFee"];
                await _context.SaveChangesAsync();
            }

        }
    }
}
