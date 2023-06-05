using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebSite_Online1a.Models;

namespace WebSite_Online1a.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginAdminController : Controller
    {
        private readonly WebOnline1Context _context;
        public INotyfService _notifyService_admin { get; }
        public LoginAdminController(WebOnline1Context context, INotyfService notifyService)
        {
            _context = context;
            _notifyService_admin = notifyService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Account model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra email và mật khẩu đã mã hóa khớp với dữ liệu trong cơ sở dữ liệu
                // kiểu không mã hóa md5 pass:
                var account = _context.Accounts.FirstOrDefault(a => a.Email == model.Email && a.Password == model.Password);               
                if (account != null)
                {
                    if(account.RoleId == 1)
                    {
                        // Lưu thông tin người dùng vào phiên - lấy được AccountId
                        // Lấy được Id của Account Khi Login
                        HttpContext.Session.SetInt32("AccountId", account.AccountId);
                        HttpContext.Session.SetString("Email", account.Email);
                        _notifyService_admin.Success("Đăng nhập Admin thành công");
                        // Chuyển hướng người dùng đến trang chủ hoặc URL mặc định                  
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Message = "Account này không có quyền truy cập Admin";
                    }                  
                }
                else
                {
                    ViewBag.Message = "Sai tài khoảng hoặc mật khẩu";
                    ModelState.AddModelError("", "Đăng nhập không thành công, Sai tài khoảng hoặc mật khẩu ");
                }
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            /*HttpContext.Session.Clear();*/
            HttpContext.Session.Remove("AccountId"); // xóa sesion của AccountId
            return RedirectToAction("Login", "LoginAdmin");
        }
        /*WebOnline1Context db = new WebOnline1Context();

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Login(Account user)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var u = db.Accounts.Where(x => x.Email.Equals(user.Email) &&
                x.Password.Equals(user.Password)).FirstOrDefault();

                //if (u != null && u.Role == "Admin")
                if (u != null)
                {
                    HttpContext.Session.SetString("UserName", u.Email.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Sai tài khoảng hoặc mật khẩu";
                    return View();
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login", "LoginAdmin");
        }*/
    }
}
