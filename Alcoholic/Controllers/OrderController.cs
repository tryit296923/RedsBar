using Alcoholic.Models;
using Alcoholic.Models.DTO;
using Alcoholic.Models.Entities;
using Alcoholic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace Alcoholic.Controllers
{
    [Authorize(Roles = "member,Guest")]
    public class OrderController : Controller
    {
        private readonly db_a8de26_projectContext _db;
        private readonly IConfiguration config;
        private readonly AesService aesService;
        private readonly HashService hashService;

        public OrderController(db_a8de26_projectContext projectContext, IConfiguration config, AesService aesService, HashService hashService)
        {
            this._db = projectContext;
            this.config = config;
            this.aesService = aesService;
            this.hashService = hashService;
        }        
        [HttpPost]
        public bool AddToCart([FromBody] List<CartItem> cartItem)
        {
            if (cartItem == null)
            {
                return false;
            }
            else
            {
                var sesStr = HttpContext.Session.GetString("CartItem");
                var addItem = new List<CartItem>();

                //判斷是否有session
                if (string.IsNullOrEmpty(sesStr))
                {
                    var cartString = JsonConvert.SerializeObject(cartItem);
                    HttpContext.Session.SetString("CartItem", cartString);
                }
                else
                {
                    var sesItem = JsonConvert.DeserializeObject<List<CartItem>>(sesStr);
                    //判斷商品是否已在session中
                    for (int i = 0; i < cartItem.Count; i++)
                    {
                        for (int S = 0; S < sesItem.Count; S++)
                        {
                            //重複產品
                            if (sesItem[S].Id == cartItem[i].Id)
                            {
                                sesItem[S].Qty += cartItem[i].Qty;
                                addItem.Add(sesItem[S]);
                                break;
                            }
                            //找完集合沒有重復產品
                            else if (sesItem[S].Id != cartItem[i].Id && S == sesItem.Count - 1)
                            {
                                sesItem.Add(cartItem[i]);
                                break;
                            }

                        }
                    }
                    //cartString為購物車字串，additem為物件
                    var cartString = JsonConvert.SerializeObject(sesItem);
                    HttpContext.Session.SetString("CartItem", cartString);
                }
                return true;

            }

        }
        public IActionResult Cart()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Success(string orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }

        public IActionResult Total()
        {
            return View();
        }
        public IActionResult OrderList()
        {
            return View();
        }
        public IActionResult Check()
        {
            return View();
        }
        public IActionResult OnlinePayment(OnlinePaymentReturn onlinePaymentReturn)
        {
            if (onlinePaymentReturn.Status == "SUCCESS")
            {
                string hashKey = config["Payment:HashKey"];
                string hashIV = config["Payment:HashIV"];
                string decryptTradeInfo = aesService.DecryptAESHex(onlinePaymentReturn.TradeInfo, hashKey, hashIV);
                PaymentResult obj_PaymentResult = JsonConvert.DeserializeObject<PaymentResult>(decryptTradeInfo);

                var orderId = obj_PaymentResult.Result.MerchantOrderNo;

                ViewBag.orderId = orderId;
                return View("OnlinePaymentSucceed");
            }
            else
            {
                return View("OnlinePaymentFailed");
            }

        }
        public IActionResult FrontDeskCheckout()
        {
            return View();
        }
        public IActionResult Feedback(string OrderId)
        {
            ViewBag.order = OrderId;
            return View();
        }
    }
}
