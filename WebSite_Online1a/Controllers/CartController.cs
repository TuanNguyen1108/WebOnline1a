using Microsoft.AspNetCore.Mvc;
using WebSite_Online1a.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using WebSite_Online1a.Helpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Session;

namespace WebSite_Online1a.Controllers
{
    public class CartController : Controller
    {
        private readonly WebOnline1Context _context;
        public CartController(WebOnline1Context context)
        {
            _context = context;
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
            return View(Carts);
        }

        public IActionResult AddToCart(int id, int idprice, int SoLuong, string type = "Normal")
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(p => p.MaHh == idprice);
            if (item == null) //chưa có
            {
                var hanghoa = _context.Products.SingleOrDefault(p => p.ProductId == id);
                var hanghoa1 = _context.Prices.SingleOrDefault(p => p.PriceId == idprice);

                item = new CartItem
                {
                    MaHh = idprice,
                    MaSP = id,
                    // lấy được id của bảng product
                    SoLuong = SoLuong,
                    TenHH = hanghoa.NameProduct,
                    Color = hanghoa1.Color,
                    Gia = hanghoa1.PriceNew.Value,
                    HinhAnh = hanghoa1.ColorImage
                };
                myCart.Add(item);// thêm vào giỏ hàng
            }
            else
            {
                /*item.SoLuong++;*/
                item.SoLuong = item.SoLuong + SoLuong;
            }
            HttpContext.Session.Set("GioHang", myCart);
            if (type == "ajax")
            {
                return Json(new
                {
                    SoLuong = Carts.Sum(c => c.SoLuong),
                    /*SoLuong = Carts.Count,*/
                });
            }
            return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng." });
        }

        /*public IActionResult AddToCart(int id, int idprice, int SoLuong, string type = "Normal")
        {
            var myCart = Carts; // lấy thông tin của giỏ hàng Carts khai báo bên trên           
            *//*var selectedColor = Request.Form["color"];*/
        /*int idprice = int.Parse(selectedColor);*//*
        // lấy được id của Table Price vì khai báo bên View Index giỏ hàng (type= radio, value="@item.PriceId")
        //Phương thức int.Parse() sẽ chuyển đổi chuỗi đầu vào sang kiểu int và trả về giá trị tương ứng
        //nếu chuỗi đó là một số nguyên hợp lệ.
        //Nếu chuỗi không thể chuyển đổi thành kiểu int, phương thức này sẽ sinh ra một ngoại lệ (exception).
        try
        {
            var item = myCart.SingleOrDefault(p => p.MaHh == idprice);
            if (item == null) //chưa có
            {
                var hanghoa = _context.Products.SingleOrDefault(p => p.ProductId == id);
                var hanghoa1 = _context.Prices.SingleOrDefault(p => p.PriceId == idprice);

                item = new CartItem
                {
                    *//*MaHh = id,*//*
                    MaHh = idprice, // phân biệt màu dựa vào idprice chọn bên giao diện "radio"
                                    // lấy được id của bảng price
                    MaSP = id,
                    // lấy được id của bảng product
                    SoLuong = SoLuong,
                    TenHH = hanghoa.NameProduct,
                    Color = hanghoa1.Color,
                    Gia = hanghoa1.PriceNew.Value,
                    HinhAnh = hanghoa1.ColorImage
                };
                myCart.Add(item);// thêm vào giỏ hàng
            }
            else
            {
                *//*item.SoLuong++;*//*
                item.SoLuong = item.SoLuong + SoLuong;
            }
            HttpContext.Session.Set("GioHang", myCart);
            return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng." });
        }
        catch(Exception ex)
        {
            return Json(new { success = false, message = "Thêm Sản phẩm thất bại." });
        }       
        if(type == "ajax")
        {
            *//* return Json(new
             {
                 SoLuong =  Carts.Sum(c=>c.SoLuong),
                 *//*SoLuong = Carts.Count,*//*
                 success = true,
                 message = "Sản phẩm đã được thêm vào giỏ hàng."
             });*/
        /*return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng." });*//*

    }
    return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng." });
    *//*return RedirectToAction("Index");*//*
    }*/

        public IActionResult Remove(int id)
        {
            try
            {
                var myCart = Carts;
                var item = myCart.SingleOrDefault(p => p.MaHh == id);
                if (item != null)
                {
                    myCart.Remove(item);
                }
                HttpContext.Session.Set("GioHang", myCart);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public IActionResult ThemQuantity(int id)
        {
            try
            {
                var myCart = Carts;
                var item = myCart.SingleOrDefault(p => p.MaHh == id);
                if (item != null)
                {
                    item.SoLuong++;
                }
                HttpContext.Session.Set("GioHang", myCart);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }
        public IActionResult GiamQuantity(int id)
        {
            try
            {
                var myCart = Carts;
                var item = myCart.SingleOrDefault(p => p.MaHh == id);
                if (item != null)
                {
                    item.SoLuong = item.SoLuong - 1;
                    if (item.SoLuong == 0)
                    {
                        myCart.Remove(item);
                    }
                }
                HttpContext.Session.Set("GioHang", myCart);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }
        /* public async Task<IActionResult> CheckOut(int id)
         {
             try
             {
                 var myCart = Carts;
                 *//* var item = myCart.SingleOrDefault(p => p.MaHh == id);*/

        /*OderDetail _oder = new OderDetail();
        _oder.ProductId = item.MaHh;
        _oder.Num = item.SoLuong;
        _context.OderDetails.Add(_oder);*//*

        // đây là bảng customer của mình , để khách hàng nhập thông tin

        foreach (var item in myCart )
        {
            OderDetail _oderdetail = new OderDetail();
            {
                // ví dụ đây là (Đơn hàng = 1 => chứa 4 sản phẩm iphone của khách hàng A), (Đơn hàng = 2 => chứa 4 sản phẩm iphone của khách hàng B)
                _oderdetail.ProductId = item.MaSP;
                _oderdetail.PriceId = item.MaHh; // làm cách nào để lấy được tất cả mã hh của tất cả sp => lưu vào 1 oderid
                _oderdetail.Num = item.SoLuong;
            }

            _context.Add(_oderdetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }                               
        return RedirectToAction("Index");
    }
    catch
    {
        return Content("lỗi");
    }
    }*/
        public async Task<IActionResult> CheckOut(int id, [Bind("ProductId,PriceId,Num")] OderDetail Order)
        {
            try
            {
                var myCart = Carts;
                foreach (var item in myCart)
                {
                    if (ModelState.IsValid)
                    {
                        Order.OrderDetailId = null; // vì trong database để tự tăng nên không cần insert cột này vô
                        Order.ProductId = item.MaSP;
                        Order.PriceId = item.MaHh;
                        Order.Num = item.SoLuong;
                        _context.Add(Order);
                        await _context.SaveChangesAsync();
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return Content("lỗi");
            }
        }



        /*if (ModelState.IsValid)
        {
            Oder.ProductId = item.MaSP;
            Oder.PriceId = item.MaHh;

            _context.Add(Oder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
    }

        return RedirectToAction(nameof(Index));*/



        /*public PartialViewResult BagCart(int id)
        {
            int soluong = 0;
            var myCart = Carts;
            var item = myCart.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                soluong = item.SoLuong;
            }
            ViewBag.infoCart = soluong;
            return PartialView("BagCart");
        }*/
    }
}
