using Microsoft.AspNetCore.Mvc;
using WebSite_Online1a.Models;
using WebSite_Online1a.Helpers;
using Microsoft.CodeAnalysis;

namespace WebSite_Online1a.Controllers
{
	public class CartYeuThichController : Controller
	{
		private readonly WebOnline1Context _context;
		public CartYeuThichController(WebOnline1Context context)
		{
			_context = context;
		}

		public List<CartItem_YeuThich> Carts_YeuThich
		{
			get
			{
				var data1 = HttpContext.Session.Get<List<CartItem_YeuThich>>("GioHang1");
				if (data1 == null)
				{
					data1 = new List<CartItem_YeuThich>();
				}
				return data1;
			}
		}
		public IActionResult Index()
		{
			return View(Carts_YeuThich);
		}

		public IActionResult AddToCart(int id, int SoLuong, string type = "Normal")
		{
            //bool isAdded = false;

            var myCart1 = Carts_YeuThich;       
			var item = myCart1.SingleOrDefault(p => p.MaHh == id);
			if (item == null) //chưa có
			{
				var hanghoa = _context.Products.SingleOrDefault(p => p.ProductId == id);
				item = new CartItem_YeuThich
				{
					MaHh = id, 
					TenHH = hanghoa.NameProduct,
					SoLuong = SoLuong,
					Gia = hanghoa.Price.Value,
					HinhAnh = hanghoa.ProductImage
				};
				myCart1.Add(item);// thêm vào giỏ hàng
			}
			else
			{
                //myCart1.Remove(item);
            }
			HttpContext.Session.Set("GioHang1", myCart1);

			if (type == "ajax")
			{
				return Json(new
				{
					SoLuong = Carts_YeuThich.Sum(c => c.SoLuong),
					/*SoLuong = Carts.Count,*/
				});
			}
			return RedirectToAction("Index");
		}

        public IActionResult Remove(int id)
        {
            try
            {
                var myCart1 = Carts_YeuThich;
                var item = myCart1.SingleOrDefault(p => p.MaHh == id);
                if (item != null)
                {
                    myCart1.Remove(item);
                }
                HttpContext.Session.Set("GioHang1", myCart1);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }


        public IActionResult Update(int id, int SoLuong, string type = "Normal")
        {
            bool isAdded = false;
            var myCart1 = Carts_YeuThich;
            var item = myCart1.SingleOrDefault(p => p.MaHh == id);
            if (item == null)
            {
                var hanghoa = _context.Products.SingleOrDefault(p => p.ProductId == id);
                item = new CartItem_YeuThich
                {
                    MaHh = id,
                    TenHH = hanghoa.NameProduct,
                    SoLuong = SoLuong,
                    Gia = hanghoa.Price.Value,
                    HinhAnh = hanghoa.ProductImage
                };
                myCart1.Add(item);// thêm vào giỏ hàng
            }
            else
            {
                myCart1.Remove(item);
                isAdded = true;
            }
            return RedirectToAction("Index");
        }
    }
}
