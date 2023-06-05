using Microsoft.AspNetCore.Mvc;
using WebSite_Online1a.Models;

namespace WebSite_Online1a.Controllers
{
    public class SearchController : Controller
    {
		private readonly WebOnline1Context _context;

		public SearchController(WebOnline1Context context)
		{
			_context = context;
		}
		public IActionResult Index()
        {
            return View();
        }
    }
}
