using Microsoft.AspNetCore.Mvc;
using WebSite_Online1a.Models;
using WebSite_Online1a.Helpers;

namespace WebSite_Online1a.Controllers
{
	public class CartSoSanhController : Controller
	{
		private readonly WebOnline1Context _context;
		public CartSoSanhController(WebOnline1Context context)
		{
			_context = context;
		}

		public List<CartItem_SoSanh> Carts_SoSanh
        {
			get
			{
				var data2 = HttpContext.Session.Get<List<CartItem_SoSanh>>("GioHang2");
				if (data2 == null)
				{
					data2 = new List<CartItem_SoSanh>();
				}
				return data2;
			}
		}
		public IActionResult Index()
		{
			return View(Carts_SoSanh);
		}

		public IActionResult AddToCart(int id, int id1, int id2, int id3, int SoLuong, string type = "Normal")
		{
			var myCart2 = Carts_SoSanh;       
			var item = myCart2.SingleOrDefault(p => p.MaHh == id);
			if (item == null) //chưa có
			{
				var hanghoa = _context.Products.SingleOrDefault(p => p.ProductId == id);
				var category = hanghoa.CategoryId;
				var brand = hanghoa.BrandId;
				var specification = hanghoa.SpecificationId;
				var hanghoa1 = _context.Categories.SingleOrDefault(p => p.CategoryId == category);
                var hanghoa2 = _context.Brands.SingleOrDefault(p => p.BrandId == brand);
                var hanghoa3 = _context.Specifications.SingleOrDefault(p => p.SpecificationId == specification);
                item = new CartItem_SoSanh
                {
					MaHh = id, 
					TenHH = hanghoa.NameProduct,
					SoLuong = SoLuong,
					Gia = hanghoa.Price.Value,
					HinhAnh = hanghoa.ProductImage,
                    ThuongHieu = hanghoa2.NameBrand,
                    DanhMuc = hanghoa1.NameCategory,
                    Cpu = hanghoa3.Cpu,
                    ManHinh = hanghoa3.SizeCreen,
                    HeDieuHanh = hanghoa3.OperatingSystem,
                    Ram = hanghoa3.Ram,
                    Pin = hanghoa3.Battery,
                    Camera = hanghoa3.Camera,
                    CameraSelfie = hanghoa3.CameraSelfie
                };
                myCart2.Add(item);// thêm vào giỏ hàng
			}
			else
			{
                
            }
			HttpContext.Session.Set("GioHang2", myCart2);
			return RedirectToAction("Index");
		}

        public IActionResult Remove(int id)
        {
            try
            {
                var myCart2 = Carts_SoSanh;
                var item = myCart2.SingleOrDefault(p => p.MaHh == id);
                if (item != null)
                {
                    myCart2.Remove(item);
                }
                HttpContext.Session.Set("GioHang2", myCart2);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

    }
}
