using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebSite_Online1a.Models;

namespace WebSite_Online1a.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchAdminController : Controller

    {
        private readonly WebOnline1Context _context;

        public SearchAdminController(WebOnline1Context context)
        {
            _context = context;
        }

        // GET: Search/FindSpecification
        [HttpPost]
        public IActionResult FindSpecification(string keyword)
        {
            List<Specification> ls = new List<Specification>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListSpecificationSearchPartial", null);
            }

            ls = _context.Specifications
                .AsNoTracking()
                .Where(x => x.NameSpecification.Contains(keyword))               
                .OrderByDescending(x => x.NameSpecification)
                .Take(10)
                .ToList();            

            if (ls == null)
            {
                return PartialView("ListSpecificationSearchPartial", null);
            }
            else
            {
                return PartialView("ListSpecificationSearchPartial", ls);
            }

        }

        // GET: Search/FindCategory
        public IActionResult FindCategory(string keyword)
        {
            List<Category> ls = new List<Category>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListCategorySearchPartial", null);
            }

            ls = _context.Categories
                .AsNoTracking()
                .Include(x=>x.Brand)
                .Where(x => x.NameCategory.Contains(keyword))
                .OrderByDescending(x => x.NameCategory)
                .Take(10)
                .ToList();

            if (ls == null)
            {
                return PartialView("ListCategorySearchPartial", null);
            }
            else
            {
                return PartialView("ListCategorySearchPartial", ls);
            }

        }

        // GET: Search/FindProduct
        public IActionResult FindProduct(string keyword)
        {
            List<Product> ls = new List<Product>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListProductSearchPartial", null);
            }

            ls = _context.Products
                .AsNoTracking()
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Specification)
                .Where(x => x.NameProduct.Contains(keyword))
                .OrderByDescending(x => x.NameProduct)
                .Take(10)
                .ToList();

            if (ls == null)
            {
                return PartialView("ListProductSearchPartial", null);
            }
            else
            {
                return PartialView("ListProductSearchPartial", ls);
            }

        }

        public IActionResult Find_ProductDetail(string keyword)
        {
            List<Price> ls = new List<Price>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListPriceSearchPartial", null);
            }

            ls = _context.Prices
                .AsNoTracking()
                .Include(x => x.Product)                               
                .Where(x => x.NamePrice.Contains(keyword))
                .OrderByDescending(x => x.NamePrice)
                .Take(10)
                .ToList();

            if (ls == null)
            {
                return PartialView("ListPriceSearchPartial", null);
            }
            else
            {
                return PartialView("ListPriceSearchPartial", ls);
            }

        }
    }
}