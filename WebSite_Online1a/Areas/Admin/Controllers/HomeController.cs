using Microsoft.AspNetCore.Mvc;
using Website_Online.Areas.Admin.Models.Authentication;

namespace WebSite_Online1a.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")] // định nghĩa khu vực admin
        [Authentication]
        public IActionResult Index()
        {
            //ViewBag.UserName = HttpContext.Session.GetString("UserName"); 
            // hiện tên khi đăng nhập
            ViewBag.UserName = HttpContext.Session.GetString("Email");
            return View();
        }
    }
}
