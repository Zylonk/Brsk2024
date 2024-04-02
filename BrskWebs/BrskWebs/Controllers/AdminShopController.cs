using BrskWebs.Class;
using BrskWebs.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BrskWebs.Controllers
{
    public class AdminShopController : Controller
    {
        string _apiUrl = "https://localhost:7066/api/";
        public async Task<IActionResult> Index()
        {
            List<Order> ord = await GetrListCou();
            ViewBag.Order = ord;
            return View();
        }
        public async Task<IActionResult> PutStatus(int id)
        {
            List<Order> ord = await GetrListCou();
            var info = ord.ToList().FirstOrDefault(x => id == x.OrderId);
            return View(info);
        }
        public async Task<IActionResult> PutsStatus(Order order )
        {
            order.UserId = 1;
            order.OrderDate = DateTime.Now;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Tokens.Token);
                string jsonOrder = Newtonsoft.Json.JsonConvert.SerializeObject(order);
                var content = new StringContent(jsonOrder, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await httpClient.PostAsync(_apiUrl + "redactstatus", content);
                    if (response.IsSuccessStatusCode)
                    {
                       
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        TempData["ErrorMessage"] = $"Ошибка: {response.StatusCode} - {response.ReasonPhrase}. Дополнительная информация: {errorContent}";
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Исключение: {ex.Message}";
                    Console.WriteLine($"Exception: {ex.Message}");
                    return RedirectToAction("Index");
                }


            }
          
        }
            private async Task<List<Order>> GetrListCou()
            {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Tokens.Token);
                HttpResponseMessage response = await httpClient.GetAsync(_apiUrl + "order");
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    List<Order> tasks = JsonConvert.DeserializeObject<List<Order>>(jsonResponse);
                    return tasks;
                }
                else
                {
                    Console.WriteLine($"Ошибка: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
        }
    }
}
