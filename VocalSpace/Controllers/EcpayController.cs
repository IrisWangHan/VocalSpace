using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;
using VocalSpace.Models;

namespace VocalSpace.Controllers
{
    public class EcpayController : Controller
    {

        private readonly VocalSpaceDbContext _context;
        public EcpayController(VocalSpaceDbContext context)
        {
            _context = context;
        }
    //step4 : 新增訂單
    [HttpPost]
    public string AddOrders([FromBody] Dictionary<string, string> json )   // [FromBody] 取得 POST 傳遞的 JSON
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
    }
}
