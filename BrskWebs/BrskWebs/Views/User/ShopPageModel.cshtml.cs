using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrskWebs.Views.User.ShopPageModel
{
    public class ShopPageModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Title"] = "Shop Page";
        }
    }
}