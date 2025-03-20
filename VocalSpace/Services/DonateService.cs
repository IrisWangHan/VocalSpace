using System.Text;
using System.Web;
using VocalSpace.Models;
using System.Security.Cryptography;  //  這裡是要使用 SHA256

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

        //step2 : 新增訂單到資料庫
        //[HttpPost]
        public string AddOrderToDb(Dictionary<string, string> order)   
        {
            string num = "0";
            int SponsorId = int.Parse(order["SponsorId"]);
            int SongId = int.Parse(order["SongId"]);
            //  查詢歌手id
            long ReceiverId = _context.Songs.FirstOrDefault(id => id.SongId == SongId).Artist;

            try
            {
                Ecpay Orders = new Ecpay();

                Orders.MerchantTradeNo = order["MerchantTradeNo"];
                Orders.SponsorId = SponsorId;
                Orders.ReceiverId = ReceiverId;
                Orders.ItemName = order["ItemName"];
                Orders.RtnCode = 0;     //未付款
                Orders.RtnMsg = "訂單成功尚未付款";
                //Orders.TradeNo = json.MerchantID.ToString();
                Orders.TradeAmt = int.Parse(order["TotalAmount"]);
                Orders.PaymentType = order["PaymentType"];
                Orders.PaymentTypeChargeFee = "0";
                Orders.MerchantTradeDate = order["MerchantTradeDate"];

                _context.Ecpays.Add(Orders);
                _context.SaveChanges();
                num = "OK";
            }
            catch (Exception ex)
            {
                num = ex.ToString();
            }
            return num;
        }
    }
}
