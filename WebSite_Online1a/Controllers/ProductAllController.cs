using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSite_Online1a.Models;

namespace WebSite_Online1a.Controllers
{
	public class ProductAllController : Controller
	{
		private readonly WebOnline1Context _context;
		public ProductAllController(WebOnline1Context context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var listsanpham = _context.Products
			.Include(p => p.Brand)
			.Include(p => p.Category)
			.Include(p => p.Specification)	
			/*.OrderByDescending(x=>x.NameProduct)*/
			.ToList();
			return View(listsanpham);
		}
		public IActionResult Details(int id)
		{
			var lsImage = _context.Prices
			.AsNoTracking()
			.Where(x => x.ProductId == id)
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
		}
	}
}
