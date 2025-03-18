using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Web;
using VocalSpace.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace VocalSpace.Controllers
{
    public class EcpayController : Controller
    {

        private readonly VocalSpaceDbContext _context;
        private readonly IConfiguration? _config;
        public EcpayController(VocalSpaceDbContext context, IConfiguration? config)
        {
            _context = context;
            _config = config;
        }
        //step4 : 新增訂單
        [HttpPost]
        public string AddOrders([FromBody] Dictionary<string, string> json)   // [FromBody] 取得 POST 傳遞的 JSON
        {
            string num = "0";
            try
            {
                Ecpay Orders = new Ecpay();

                Orders.MerchantTradeNo = json["MerchantTradeNo"];
                Orders.SponsorId = 2;
                Orders.ReceiverId = 3;
                Orders.ItemName = json["ItemName"];
                Orders.RtnCode = 0; //未付款
                Orders.RtnMsg = "訂單成功尚未付款";
                //Orders.TradeNo = json.MerchantID.ToString();
                Orders.TradeAmt = int.Parse(json["TotalAmount"]);

                Orders.PaymentType = json["PaymentType"];
                Orders.PaymentTypeChargeFee = "0";
                Orders.MerchantTradeDate = json["MerchantTradeDate"];

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
        // step5 : ReturnURL 接收綠界付款結果
        [HttpPost]
        [AllowAnonymous]
        public IActionResult AddPayInfo(IFormCollection form)
        {
            Console.WriteLine(form.ToString());
            // 將綠界傳來的 Form 資料轉成 Dictionary 
            var collection = form.ToDictionary(x => x.Key, x => x.Value.ToString());
            if (!collection.TryGetValue("CheckMacValue", out string? receivedCheckMac))
            {
                return Content("0|缺少檢查碼");
            }

            // 計算我們這邊的檢查碼（注意需排除綠界傳來的 CheckMacValue 本身）
            var CollectionForMac = new Dictionary<string, string>();
            foreach (var data in collection ) 
            {
                if (data.Key == "CheckMacValue")
                {
                    continue;
                }
                CollectionForMac.Add(data.Key, data.Value);
            }

            
            string computedMac = GetCheckMacValue(CollectionForMac);
            if ( !computedMac.Equals(receivedCheckMac, StringComparison.OrdinalIgnoreCase) )
            {
                return Content("0|CheckMacValueError");
            }
            UpdatePayInfo(collection);
            return Content("1|OK");

            
        }
        //  付款結果更新資料庫
        private void UpdatePayInfo( Dictionary<string, string> collection)
        {
            string temp = collection["MerchantTradeNo"];
            var result = _context.Ecpays.FirstOrDefault(data => data.MerchantTradeNo == temp);
            if (result != null) 
            {
                result.RtnCode = int.Parse(collection["RtnCode"]);
                result.RtnMsg = collection["RtnMsg"];
                result.EcpayTradeNo = collection["TradeNo"];
                //result.TradeAmt = int.Parse(collection["TradeAmt"]);
                result.PaymentDate = Convert.ToDateTime(collection["PaymentDate"]);
                result.PaymentType = collection["PaymentType"];
                result.PaymentTypeChargeFee = collection["PaymentTypeChargeFee"];
                _context.SaveChanges();
            }
           
        }

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
        
    }
}