using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSite_Online1a.Models;

namespace WebSite_Online1a.Controllers
{
    public class ProductXiaomiController : Controller
    {
        private readonly WebOnline1Context _context;
        public ProductXiaomiController(WebOnline1Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var listsanpham = _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Specification)
            .Where(p => p.BrandId == 5)
            .ToList();
            return View(listsanpham);
        }
    }
}
