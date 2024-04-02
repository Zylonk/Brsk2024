using brskweb.Class;
using brskweb.Model;
using brskweb.ModelDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace brskweb.Controllers
{
    [Authorize]
    
    [ApiController]
    public class UserFunc : ControllerBase
    {
        int order = 0;
        [Route("api/addorder")]
        [HttpPost]
        [Authorize]
        public ActionResult AddOrder(OrderDTO ord)
        {
            if (ord != null)
            {
                ord.OrderId = GetrContext.Context.Orders.ToList().Count() + 1;
                order = ord.OrderId;
                GetrContext.Context.Orders.Add(OrderDTO.ConvertToOrder(ord));
                GetrContext.Context.SaveChanges();
                return Ok();
            }
            return BadRequest("Ошибка");
        }
        [Route("api/addorderitem")]
        [HttpPost]
        [Authorize]
        public ActionResult AddOrderItem(Dictionary<int, int> cartProductsQuantity)
        {
            if (cartProductsQuantity != null)
            {
                foreach (var i in cartProductsQuantity)
                {

                    int id = GetrContext.Context.OrderItems.ToList().Count() + 1;
                    int idord = GetrContext.Context.Orders.ToList().Count();
                    OrderItem ord = new OrderItem
                    {
                        OrderItemId = id,
                        OrderId = order,
                        ProductId = i.Key,
                        Quantity = i.Value,
                    };
                    GetrContext.Context.OrderItems.Add(ord);
                    GetrContext.Context.SaveChanges();
                }
                return Ok();
            }
            return BadRequest("Ошибка");
        }

    }
}
