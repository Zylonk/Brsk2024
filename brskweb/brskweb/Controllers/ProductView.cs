using brskweb.Class;
using brskweb.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace brskweb.Controllers
{
   
    [ApiController]
    public class ProductView : ControllerBase
    {
        [Route("api/product")]
        [HttpGet]
        public ActionResult<List<Product>> GetProduct()
        {
            return GetrContext.Context.Products.ToList();
        }
        [Route("api/product/{id}")]
        [HttpGet]
        public ActionResult<Product> GetProductById(int id)
        {
            var product = GetrContext.Context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound(); // Возвращаем 404 Not Found, если товар с указанным идентификатором не найден
            }
            return product;
        }
    }
}
