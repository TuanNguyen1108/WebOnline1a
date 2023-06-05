using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSite_Online1a.Models;

namespace WebSite_Online1a.Controllers
{
	public class ProductAppleController : Controller
	{
		private readonly WebOnline1Context _context;
		public ProductAppleController(WebOnline1Context context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var listsanpham = _context.Products
			.Include(p => p.Brand)
			.Include(p => p.Category)
			.Include(p => p.Specification)
			.Where(p => p.BrandId == 1)
			.ToList();
			return View(listsanpham);
		}

        public IActionResult LocSanPham()
        {
            var lsCategory = _context.Categories
				 .AsNoTracking()
				 .Where(x => x.BrandId == 1)
				 .Take(10)
				 .ToList();
            ViewBag.Category = lsCategory;
            return View();
        }
    }
}


/*public IActionResult Details(int id)
        {
			var lsImage = _context.Prices
			.AsNoTracking()
			*//*.Where(x => x.ProductId == id)*//*
			.Where(x => x.ProductId == id)
			// dùng để test giỏ hàng
			// khi mà bảng price có 1 productid sản phẩm duy nhất 
			// có 1 productid = 1 
			// có 1 productid = 2 => cartcontroller mới chạy oke => bắt sự kiện cho type radio để chọn 1 dữ liệu duy nhất => lấy ra được priceid
			.Take(10)
			.ToList();
			ViewBag.Image = lsImage;


			var product = _context.Products
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Specification)				
                .FirstOrDefault(x => x.ProductId == id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            return View(product);
        }*/