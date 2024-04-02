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
    public class AdminShopFunc : ControllerBase
    {
  
        [Route("api/order")]
        [HttpGet]
        public ActionResult<List<Order>> GetOrder()
        {
            return GetrContext.Context.Orders.ToList();
        }
        [Route("api/redactstatus")]
        [HttpPost]
        public ActionResult RedactOrder(OrderDTO ord)
        {
            if (ord !=null)
            {
                Order order = GetrContext.Context.Orders.FirstOrDefault(x => ord.OrderId == x.OrderId);
                order.Status = ord.Status;
                GetrContext.Context.SaveChanges();
                return Ok();
            }

            return BadRequest();
        }
    }
}
