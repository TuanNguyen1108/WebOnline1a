using Microsoft.AspNetCore.Mvc;
using Website_Online.Models.Authentication_1;
using WebSite_Online1a.Models;

namespace WebSite_Online1a.Controllers
{
    
    public class LoginController : Controller
    {
        private readonly WebOnline1Context _context;
        public LoginController(WebOnline1Context context)
        {
            _context = context;
        }
         //// phần login/login đã được chuyển sang account/login ( phần login/login là phần làm nháp)
         //// phần login/login đã được chuyển sang account/login ( phần login/login là phần làm nháp)
         //// phần login/login đã được chuyển sang account/login ( phần login/login là phần làm nháp)
         //// phần login/login đã được chuyển sang account/login ( phần login/login là phần làm nháp)
         //// phần login/login đã được chuyển sang account/login ( phần login/login là phần làm nháp)
         //// phần login/login đã được chuyển sang account/login ( phần login/login là phần làm nháp)
         //// phần login/login đã được chuyển sang account/login ( phần login/login là phần làm nháp)
         //// phần login/login đã được chuyển sang account/login ( phần login/login là phần làm nháp)
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Account model)
        {
            if (ModelState.IsValid)
            {
                var account = _context.Accounts.FirstOrDefault(a => a.Email == model.Email && a.Password == model.Password);
                if (account != null)
                {
                    HttpContext.Session.SetInt32("AccountId", account.AccountId);
                    return RedirectToAction("AccountInfo");
                }
                else
                {
                    ViewBag.Message = "Sai tài khoảng hoặc mật khẩu";
                    ModelState.AddModelError("", "Đăng nhập không thành công, Sai tài khoảng hoặc mật khẩu ");
                }
            }
            return View(model);
        }
        public IActionResult AccountInfo()
        {
            int? accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId.HasValue)
            {
                var account = _context.Accounts.FirstOrDefault(a => a.AccountId == accountId.Value);
                if (account != null)
                {
                    return View(account);
                }
            }
            return RedirectToAction("Login");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("AccountId");
            return RedirectToAction("Login", "Login");
        }

        public IActionResult Register(string email, string password, string hoten, string sdt)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tài khoản đã tồn tại chưa
                if (_context.Accounts.Any(a => a.Email == email))
                {
                    ViewBag.Message = ("Tài khoản đã tồn tại");
                    return View();
                }
                var newAccount = new Account
                {
                    Email = email,
                    Password = password,
                    HoTen = hoten,
                    Sdt = sdt,
                };
                // Tạo mới tài khoản
                _context.Accounts.Add(newAccount);
                _context.SaveChanges();

                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        /*public IActionResult Register(Account model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tài khoản đã tồn tại chưa
                if (_context.Accounts.Any(a => a.Email == model.Email))
                {
                    ModelState.AddModelError("", "Tài khoản đã tồn tại");
                    return View(model);
                }
                // Tạo mới tài khoản
                _context.Accounts.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Login", "Login");
            }
            return View(model);
        }*/
        /*public IActionResult Register(Account account)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tài khoản đã tồn tại chưa
                var existingAccount = _context.Accounts.FirstOrDefault(a => a.Email == account.Email);
                if (existingAccount == null)
                {
                    return Json(new { success = false, message = "Tài khoản đã tồn tại" });
                }

                // Tạo mới tài khoản
                _context.Accounts.Add(account);
                _context.SaveChanges();

                return Json(new { success = true, message = "Tạo mới tài khoản thành công" });
            }

            return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
        }*/
    }
}


