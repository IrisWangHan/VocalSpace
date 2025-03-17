using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace VocalSpace.Controllers
{
    public class TestSponsorController : Controller
    {
        //   從 User Secrets 讀取綠界的HashKey / HashIV
        private readonly IConfiguration? _config;
        public TestSponsorController(IConfiguration? config)
        {
            _config = config;
        }
        
        public IActionResult Index()
        {
            //  Guid.NewGuid() : 產生全球唯一識別碼 (UUID)
            //  orderId : 產生隨機20碼訂單編號
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            //需填入你的網址
            var website = $"https://qlxw3lk1-7145.asse.devtunnels.ms/";
            var order = new Dictionary<string, string>
            {
                //綠界需要的參數
                { "MerchantTradeNo",  orderId},
                { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
                { "TotalAmount",  "387"},
                { "TradeDesc",  "無"},
                { "ItemName",  "測試商品"},
                { "CustomField1",  ""},
                { "CustomField2",  ""},
                { "CustomField3",  ""},
                { "CustomField4",  ""},
                { "ReturnURL",  $"{website}Ecpay/AddPayInfo"},
                { "MerchantID",  "3002607"},
                { "PaymentType",  "aio"},
                { "ChoosePayment",  "Credit"},
                { "EncryptType",  "1"},
            };
            //檢查碼
            order["CheckMacValue"] = GetCheckMacValue(order);
            return View(order);
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

