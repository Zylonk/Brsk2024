using BrskWebs.Class;
using BrskWebs.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace BrskWebs.Controllers
{
    public class UserController : Controller
    {
        private readonly string _apiUrl = "https://localhost:7066/api/";


        // GET: UserController/CartPage
        public IActionResult CartPage()
        {
            // Получаем список товаров из сессии
            List<Product> cartProducts = GetCartProductsFromSession();
            return View(cartProducts);
        }
        [HttpPost]
        public IActionResult DecreaseQuantity(int productId)
        {
            Dictionary<int, int> cartProductsQuantity = GetCartProductsQuantityFromSession();
            if (cartProductsQuantity.ContainsKey(productId))
            {
                if (cartProductsQuantity[productId] > 1)
                {
                    cartProductsQuantity[productId]--; // уменьшаем количество товара
                    SaveCartProductsQuantityToSession(cartProductsQuantity);
                    return Json(cartProductsQuantity[productId]);
                }
                else
                {
                    cartProductsQuantity.Remove(productId); // удаляем товар из корзины, если количество стало меньше или равно 0
                    SaveCartProductsQuantityToSession(cartProductsQuantity);
                    return Json(0);
                }
            }
            return Json(0);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var (userId, _, _) = TokenService.GetTokenClaims();
            Order order = new Order
            {
                OrderId = 1,
                UserId = Convert.ToInt32(userId),
                OrderDate = DateTime.Now,
                Status = "В процессе"
            };
            
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Tokens.Token);
                string jsonOrder = Newtonsoft.Json.JsonConvert.SerializeObject(order);
                var content = new StringContent(jsonOrder, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await httpClient.PostAsync(_apiUrl + "addorder", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Заказ успешно оформлен!";
                        HttpContext.Session.Remove("Cart");
                        HttpContext.Session.Remove("CartQuantity");
                        return RedirectToAction("ShopPage");
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        TempData["ErrorMessage"] = $"Ошибка: {response.StatusCode} - {response.ReasonPhrase}. Дополнительная информация: {errorContent}";
                        return RedirectToAction("ShopPage");
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Исключение: {ex.Message}";
                    Console.WriteLine($"Exception: {ex.Message}");
                    return RedirectToAction("ShopPage");
                }

                
            }
           
        }
        //public async void AddOrderItem()
        //{
        //    Dictionary<int, int> cartProductsQuantity = GetCartProductsQuantityFromSession();
        //    using (HttpClient httpClient = new HttpClient())
        //    {
        //        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Tokens.Token);
        //        string jsonOrder = Newtonsoft.Json.JsonConvert.SerializeObject(cartProductsQuantity);
        //        var content = new StringContent(jsonOrder, Encoding.UTF8, "application/json");

        //        try
        //        {
        //            HttpResponseMessage response = await httpClient.PostAsync(_apiUrl + "addorderitem", content);
        //            if (response.IsSuccessStatusCode)
        //            {
                        
        //            }
        //            else
        //            {
        //                string errorContent = await response.Content.ReadAsStringAsync();
        //                    errorContent = $"Ошибка: {response.StatusCode} - {response.ReasonPhrase}. Дополнительная информация: {errorContent}";
                        
        //            }
        //        }
        //        catch (Exception ex)
        //        {
                    
        //            Console.WriteLine($"Exception: {ex.Message}");
                     
        //        }
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> IncreaseQuantity(int productId)
        {
            Product productToAdd = await GetProductById(productId);
            Dictionary<int, int> cartProductsQuantity = GetCartProductsQuantityFromSession();
            if (cartProductsQuantity.ContainsKey(productId))
            {
                if (cartProductsQuantity[productId] >= productToAdd.StockQuantity)
                {
                    
                }
                else
                {
                    cartProductsQuantity[productId]++; // увел количество товара 
                }
            
            }
            else
            {
                // если товар еще не был добавлен в корзину, добавляем его с количеством 1
                cartProductsQuantity.Add(productId, 1);
            }
            SaveCartProductsQuantityToSession(cartProductsQuantity);
            return Json(cartProductsQuantity[productId]);
        }
        private Dictionary<int, int> GetCartProductsQuantityFromSession()
        {
            string cartProductsQuantityJson = HttpContext.Session.GetString("CartQuantity");
            if (string.IsNullOrEmpty(cartProductsQuantityJson))
                return new Dictionary<int, int>();
            else
                return JsonConvert.DeserializeObject<Dictionary<int, int>>(cartProductsQuantityJson);
        }

        private void SaveCartProductsQuantityToSession(Dictionary<int, int> cartProductsQuantity)
        {
            string cartProductsQuantityJson = JsonConvert.SerializeObject(cartProductsQuantity);
            HttpContext.Session.SetString("CartQuantity", cartProductsQuantityJson);
        }

        public async Task<ActionResult> ShopPage()
        {
            List<Product> products = await GetProductList();
         
            ViewBag.Products = products;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart(int productId)
        {
            Dictionary<int, int> cartProductsQuantity = GetCartProductsQuantityFromSession();
            cartProductsQuantity.Add(productId, 1);
            List<Product> cartProducts = GetCartProductsFromSession();
            Product productToAdd = await GetProductById(productId);
            cartProducts.Add(productToAdd);
            SaveCartProductsToSession(cartProducts);
            SaveCartProductsQuantityToSession(cartProductsQuantity);

            return RedirectToAction("ShopPage");
        }

        // Метод для получения информации о товаре по его id из API
        private async Task<Product> GetProductById(int id)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(_apiUrl + $"product/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Product product = JsonConvert.DeserializeObject<Product>(jsonResponse);
                    return product;
                }
                else
                {
                    Console.WriteLine($"Ошибка: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
        }

        // Метод для получения списка товаров из API
        private async Task<List<Product>> GetProductList()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(_apiUrl + "product");
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    List<Product> products = JsonConvert.DeserializeObject<List<Product>>(jsonResponse);
                    return products;
                }
                else
                {
                    Console.WriteLine($"Ошибка: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
        }

        // Метод для получения списка товаров из сессии корзины
        private List<Product> GetCartProductsFromSession()
        {
            string cartProductsJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartProductsJson))
                return new List<Product>();
            else
                return JsonConvert.DeserializeObject<List<Product>>(cartProductsJson);
        }

        // Метод для сохранения списка товаров в сессию корзины
        private void SaveCartProductsToSession(List<Product> cartProducts)
        {
            string cartProductsJson = JsonConvert.SerializeObject(cartProducts);
            HttpContext.Session.SetString("Cart", cartProductsJson);
        }
    }
}
