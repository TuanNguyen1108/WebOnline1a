using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSite_Online1a.Helpers;
using WebSite_Online1a.Models;
using WebSite_Online1a.Models.MD5;
using WebSite_Online1a.Models.MuaHangVM;

namespace WebSite_Online1a.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly WebOnline1Context _context;
        public INotyfService _notifyService { get; }
        public CheckOutController(WebOnline1Context context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }
        public List<CartItem> Carts
        {
            get
            {
                var data = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (data == null)
                {
                    data = new List<CartItem>();
                }
                return data;
            }
        }
        public IActionResult Index()
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            if (HttpContext.Session.GetInt32("AccountId") != null)
            {
                int? accountId = HttpContext.Session.GetInt32("AccountId");
                MuaHangVM model = new MuaHangVM();  
                if(accountId != null) 
                {
                    var khachhang = _context.Accounts.AsNoTracking().SingleOrDefault(x=>x.AccountId == accountId);
                    model.CustomerId = khachhang.AccountId;
                    model.Email = khachhang.Email;
                    model.FullName = khachhang.HoTen;
                    model.Phone = khachhang.Sdt;
                    model.Address = khachhang.Address;
                }
                ViewBag.GioHang = Carts;
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account", new {returnUrl = "/CheckOut/index"});
            }
        }

        public async Task<IActionResult> CheckOut(MuaHangVM muahang)
        { 
            // lấy giỏ hàng để xử lý
            var myCart = Carts;
            // lấy AccountId khi login
            int? accountId = HttpContext.Session.GetInt32("AccountId");
            // lấy model muahangvm đã tạo để lưu thông tin, từ đó lưu xuống table Order
            MuaHangVM model = new MuaHangVM();
            if(accountId != null)
            {
                var khachhang = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.AccountId == accountId);
                model.CustomerId = khachhang.AccountId;
                // model.Email = khachhang.Email; 
                model.FullName = khachhang.HoTen;
                model.Phone = khachhang.Sdt;
                model.Address = khachhang.Address;

                //Nếu khách hàng có thay đổi thông tin trong form, thì cập nhật lại thông tin cho account luôn dựa vào MuaHangVM
                khachhang.HoTen = muahang.FullName;
                khachhang.Sdt = muahang.Phone;
                khachhang.Address = muahang.Address;

                _context.Update(khachhang);
                _context.SaveChanges();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    // Khởi tạo đơn hàng
                    Order donhang = new Order();
                    donhang.AccountId = model.CustomerId;
                    donhang.FullName = model.FullName;
                    donhang.Phone = model.Phone;
                    donhang.Address = muahang.Address;
                    donhang.OrderStatusId = 1; // đơn hàng mới nên sẽ chờ duyệt
                    donhang.Payments = 1; // đơn hàng mới nên chưa thanh toán
                    donhang.OderDate = DateTime.Now;
                    donhang.TotalMoney = Convert.ToInt32(myCart.Sum(p => p.ThanhTien));
                    donhang.Note = muahang.Note;
                    _context.Add(donhang);
                    _context.SaveChanges();

                    // Tạo danh sách đơn hàng
                    foreach (var item in myCart)
                    {
                        OderDetail oderDetail = new OderDetail();
                        oderDetail.OrderDetailId = null; // vì trong database để tự tăng nên không cần insert cột này vô
                        oderDetail.OrderId = donhang.OrderId;
                        oderDetail.ProductId = item.MaSP;
                        oderDetail.PriceId = item.MaHh;
                        oderDetail.Num = item.SoLuong;
                        oderDetail.Price = item.Gia;
                        _context.Add(oderDetail);
                    }
                        _context.SaveChanges();
                    // clear giỏ hàng
                    HttpContext.Session.Remove("GioHang");
                    // Thông Báo
                    _notifyService.Success("Đặt Hàng Thành Công");
                    return RedirectToAction("Orders","Account");
                }    
            }
            catch
            {
                return Content("lỗi");
            }
            return View();
        }

        /*public async Task<IActionResult> CheckOut([Bind("ProductId,PriceId,Num,CustomerId")] OderDetail Order, Account model)
        {

            try
            {
                // lấy giỏ hàng
                var myCart = Carts;
                // lấy id của account đã login
                int? accountId = HttpContext.Session.GetInt32("AccountId");
                foreach (var item in myCart)
                {
                    if (ModelState.IsValid)
                    {
                        Order.OrderDetailId = null; // vì trong database để tự tăng nên không cần insert cột này vô
                        Order.ProductId = item.MaSP;
                        Order.PriceId = item.MaHh;
                        Order.Num = item.SoLuong;
                    }                 
                }
                //Order.CustomerId = accountId;
                _context.Add(Order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("lỗi");
            }
        }*/
    }
}
