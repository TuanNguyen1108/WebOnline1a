using Microsoft.AspNetCore.Mvc;
using Website_Online.Areas.Admin.Models.Authentication;
using WebSite_Online1a.Models;

namespace WebSite_Online1a.Areas.Admin.Controllers
{
    [Area("Admin")] // định nghĩa khu vực admin
    [Authentication]
    public class HomeController : Controller
    {
        private readonly WebOnline1Context _context;
        public HomeController(WebOnline1Context context)
        {
            _context = context; 
        }
        public IActionResult Index()
        {
            //ViewBag.UserName = HttpContext.Session.GetString("UserName"); 
            // hiện tên khi đăng nhập
            ViewBag.UserName = HttpContext.Session.GetString("HoTen");

            //Tổng Số Lượng Sản Phẩm
            int ProductCount = _context.Products.Count();
            ViewBag.ProductCount = ProductCount;

            int OrderCount = _context.Orders.Count();  
            ViewBag.OrderCount = OrderCount;    

            int PostCount = _context.Posts.Count();
            ViewBag.PostCount = PostCount;  

            int AccountCount = _context.Accounts.Count();  
            ViewBag.AccountCount = AccountCount;

            return View();
        }
    }
}
