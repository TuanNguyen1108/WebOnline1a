using Microsoft.AspNetCore.Mvc;
using Website_Online.Models.Authentication_1;
using WebSite_Online1a.Models;
using WebSite_Online1a.Models.MD5;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace WebSite_Online1a.Controllers
{
    public class AccountController : Controller
    {
        private readonly WebOnline1Context _context;
        public INotyfService _notifyService { get; }
        public AccountController(WebOnline1Context context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register(string Email, string Password, string HoTen, string Sdt, string ConfirmPassword)
        {

            if (ModelState.IsValid)
            {
                // Kiểm tra mật khẩu nhập lại có giống mật khẩu không
                if (Password != ConfirmPassword)
                {
                    ViewBag.Message = "Mật Khẩu không khớp!";
                    return View();
                }
                // Kiểm tra xem tài khoản đã tồn tại chưa
                if (_context.Accounts.Any(a => a.Email == Email))
                {
                    ViewBag.Message = ("Tài khoản đã tồn tại");
                    return View();
                }
                // mã hóa mật khẩu
                String passmd5 = MD5.MD5Hash(Password);
                var newAccount = new Account
                {
                    Email = Email,
                    Password = passmd5, // lưu mật khẩu đã mã hóa vào Password trong database
                    //Password = password,
                    ConfirmPassword = ConfirmPassword,
                    RoleId = 2, // khi tạo ở trang người dùng thì sẽ có roleid = 2(user)
                    HoTen = HoTen,
                    Sdt = Sdt,
                };
                // Tạo mới tài khoản
                _context.Accounts.Add(newAccount);
                _context.SaveChanges();
                _notifyService.Success("Tạo Tài Khoảng thành công");
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Account model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Mã hóa mật khẩu người dùng nhập vào bằng MD5
                string hashedInputPassword = MD5.MD5Hash(model.Password);
                // Kiểm tra email và mật khẩu đã mã hóa khớp với dữ liệu trong cơ sở dữ liệu
                var account = _context.Accounts.FirstOrDefault(a => a.Email == model.Email && a.Password == hashedInputPassword);

                // kiểu không mã hóa md5 pass:
                // var account = _context.Accounts.FirstOrDefault(a => a.Email == model.Email && a.Password == model.Password);

                //if (account != null && account.Role == "User")
                if (account != null)
                {
                    if (account.RoleId == 2)
                    {
                        // Lưu thông tin người dùng vào phiên - lấy được AccountId
                        // Lấy được Id của Account Khi Login
                        HttpContext.Session.SetInt32("AccountId", account.AccountId);
                        HttpContext.Session.SetString("HoTen", account.HoTen);
                        _notifyService.Success("Đăng nhập thành công");
                        //Kiểm tra xem có tồn tại Url không
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            // Chuyển hướng người dùng đến URL
                            return LocalRedirect(returnUrl);
                        }
                        // Chuyển hướng người dùng đến trang chủ hoặc URL mặc định                  
                        return RedirectToAction("AccountInfo", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Account này không phải thành viên! vui lòng dùng account khác ");
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
        public IActionResult ChangePassWord(Account model, string Password, string NewPassword, string ConfirmPassword)
        {
            int? accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId.HasValue)
            {
                var account = _context.Accounts.FirstOrDefault(a => a.AccountId == accountId.Value);
                if (ModelState.IsValid)
                {
                    // Mã hóa mật khẩu người dùng nhập vào bằng MD5
                    string hashedInputPassword = MD5.MD5Hash(Password);
                    string hashedInputNewPassword = MD5.MD5Hash(NewPassword);
                    string hashedInputConfirmPassword = MD5.MD5Hash(ConfirmPassword);

                    if (account.Password != hashedInputPassword)
                    {
                        ViewBag.Message = ("Mật khẩu không đúng");
                        return View(account); // return về trang có thông tin account
                    }
                    if (hashedInputNewPassword != hashedInputConfirmPassword)
                    {
                        ViewBag.Message = ("Mật khẩu mới không khớp");
                        return View(account);
                    }

                    account.Password = hashedInputNewPassword;
                    account.ConfirmPassword = NewPassword;
                    account.NewPassword = hashedInputNewPassword;
                    _context.SaveChanges();
                    _notifyService.Success("Mật khẩu đã được thay đổi thành công");
                    HttpContext.Session.Remove("AccountId"); // xóa sesion của AccountId để bắt login lại
                    return RedirectToAction("Login", "Account");

                }
                return View(account);
            }
            return RedirectToAction("Login", "Account");
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
            return RedirectToAction("Login", "Account");
        }
        public IActionResult Orders(int id)
        {
            // Lấy được Id của Account Khi Login
            int? accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId.HasValue)
            {
                // từ accountId => Thông tin của Account đó để hiển thị ra
                var account = _context.Accounts.FirstOrDefault(a => a.AccountId == accountId.Value);
                if (account != null)
                {
                    // từ accountId => đơn hàng có accountId đó => hiển thị ra 
                    var listdanhsachdonhang = _context.Orders
                            /*.Include(x=>x.OrderStatus)*/
                            .AsNoTracking()
                            .Where(p => p.AccountId == accountId.Value)
                            .OrderByDescending(p => p.OderDate) // Sắp xếp theo trường Ngày giảm dần
                            .Take(20)
                            .ToList();
                    ViewBag.DanhSachDonHang = listdanhsachdonhang;

                    return View(account);
                }
            }
            return RedirectToAction("Login", "Account");
        }
        public IActionResult OrderDetails(int id)
        {
            // Lấy được Id của Account Khi Login
            int? accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId.HasValue)
            {
                // từ accountId => Thông tin của Account đó để hiển thị ra
                var account = _context.Accounts.FirstOrDefault(a => a.AccountId == accountId.Value);
                if (account != null)
                {
                    var listchitietdonhang = _context.OderDetails
                    .Include(x => x.Product)
                    .Include(x => x.PriceNavigation)
                    .Include(x => x.Order)
                    .Where(x => x.OrderId == id)
                    .Take(10)
                    .ToList();
                    ViewBag.ChiTietDonHang = listchitietdonhang;
                    return View(account);
                }
            }
            return RedirectToAction("Login", "Account");
        }
        public IActionResult Logout()
        {
            /*HttpContext.Session.Clear();*/
            HttpContext.Session.Remove("AccountId"); // xóa sesion của AccountId
            return RedirectToAction("Login", "Account");
        }
    }
}
