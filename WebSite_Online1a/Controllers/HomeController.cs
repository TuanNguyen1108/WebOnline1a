using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebSite_Online1a.Models;

namespace WebSite_Online1a.Controllers
{
    public class HomeController : Controller
    {
		/*private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/
		private readonly WebOnline1Context _context;
		public HomeController(WebOnline1Context context)
		{
			_context = context;
		}

		public IActionResult Index()
        {
            var lsSpNoiBat = _context.Products
                 .AsNoTracking()
                 .Where(x => x.SpNoiBat == true)
                 .Take(10)
                 .ToList();
            ViewBag.SpNoiBat = lsSpNoiBat;

            var lsSpSale = _context.Products
                 .AsNoTracking()
                 .Where(x => x.SpGiamGia == true)
                 .Take(10)
                 .ToList();
            ViewBag.SpSale = lsSpSale;

            var lsTinNoiBat1 = _context.Posts
               .AsNoTracking()
               .Where(x => x.IsHot == true && x.Published == true)
               .Take(5)
               .ToList();
            ViewBag.TinNoiBat1 = lsTinNoiBat1;

            return View();
        }
		/*public async Task<IActionResult> Search(string name)
		{
			var phones = from b in _context.Products select b;
			if (!string.IsNullOrEmpty(name))
			{
				phones = phones.Where(x => x.NameProduct.Contains(name));
			}
			return View(phones);
		}*/
		public async Task<IActionResult> Search(string name)
		{
			var phones = from b in _context.Products select b;
			if (!string.IsNullOrEmpty(name))
			{				
               phones = phones.Where(x => x.NameProduct.Contains(name));        
            }
            return View(phones);
        }
        public IActionResult GioiThieu()
        {
            return View();
        }
        public IActionResult LienHe()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}