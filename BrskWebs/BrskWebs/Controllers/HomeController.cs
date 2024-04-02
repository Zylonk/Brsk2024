using BrskWebs.Class;
using BrskWebs.Model;
using BrskWebs.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace BrskWebs.Controllers
{
    public class HomeController : Controller
    {
        string _apiUrl = "https://localhost:7066/api/";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RegistrationPage()
        {
            return View();
        }
        public IActionResult AuthPage()
        {
            return View();
        }
        public async Task<IActionResult> ShopPage()
        {
            List<Product> product = await GetrListCou();


            ViewBag.Product = product;
            return View();
        }
        private async Task<List<Product>> GetrListCou()
        {
            using (HttpClient httpClient = new HttpClient())
            {

                HttpResponseMessage response = await httpClient.GetAsync(_apiUrl + "product");
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    List<Product> tasks = JsonConvert.DeserializeObject<List<Product>>(jsonResponse);
                    return tasks;
                }
                else
                {
                    Console.WriteLine($"Ошибка: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> ShopPage(string SerchInput, string filter, string sort)
        {
            List<Product> select = await GetrListCou();

            if (filter != null)
            {
                switch (filter)
                {
                    case "До 0 - 500":
                        select = select.Where(x => x.Price <= 500).ToList();
                        break;
                    case "От 500 - 5000":
                        select = select.Where(x => x.Price >= 500 && x.Price <= 5000).ToList();
                        break;
                    case "Более 5000":
                        select = select.Where(x => x.Price >= 5000).ToList();
                        break;
                    case "Все":
                        break;

                }
            }
            if (sort != null)
            {
                if (sort == "Убывание")
                {
                    select = select.OrderByDescending(x => x.Price).ToList();
                }
                else
                {
                    select = select.OrderBy(x => x.Price).ToList();
                }

            }

            if (!string.IsNullOrEmpty(SerchInput))
            {
                select = select.Where(x => x.ProductName.Contains(SerchInput)).ToList();

            }
            ViewBag.Product = select;
            return View(select);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrationPage(User user)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                
                string jsonUser = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await httpClient.PostAsync(_apiUrl + "registr", content);
                    if (response.IsSuccessStatusCode)
                    {
                        string token = await response.Content.ReadAsStringAsync();
                        return RedirectToAction("AuthPage", "Home");
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        
                        var errors = ($"Error: {response.StatusCode} - {response.ReasonPhrase}. Additional information: {errorContent}");
                        return View();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AuthPage(AuthUser auth)
        {
            
            using (HttpClient httpClient = new HttpClient())
            {
                ViewBag.ErrorMessage = null;
                string jsonUser = Newtonsoft.Json.JsonConvert.SerializeObject(auth);
                var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await httpClient.PostAsync(_apiUrl + "auth", content);
                    if (response.IsSuccessStatusCode)
                    {
                        string token = await response.Content.ReadAsStringAsync();
                        Tokens.Token = token;
                        var (userId, _, _) = TokenService.GetTokenClaims();
                        switch (userId)
                        {
                            case "3":
                                return RedirectToAction("Index", "AdminShop");
                                break;
                            case "2":
                                return RedirectToAction("ShopPage", "User");
                                break;
                            case "1":
                                return RedirectToAction("ShopPage", "User");
                                break;
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        ViewBag.ErrorMessage = $"Ошибка: Неверный пароль или логин";
                        var errors = ($"Error: {response.StatusCode} - {response.ReasonPhrase}. Additional information: {errorContent}");
                        return View();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}