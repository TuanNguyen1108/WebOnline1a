using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebDemo.Helpers;
using Website_Online.Areas.Admin.Models.Authentication;
using WebSite_Online1a.Models;

namespace WebSite_Online1a.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authentication]
    public class ProductAdminController : Controller
    {
        private readonly WebOnline1Context _context;

        public ProductAdminController(WebOnline1Context context)
        {
            _context = context;
        }

        // GET: Admin/ProductAdmin
        public async Task<IActionResult> Index()
        {
            // hiện tên khi đăng nhập
            ViewBag.UserName = HttpContext.Session.GetString("HoTenAdmin");

            var webOnline1Context = _context.Products.Include(p => p.Brand).Include(p => p.Category).Include(p => p.Specification);
            return View(await webOnline1Context.ToListAsync());
        }

        // GET: Admin/ProductAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Specification)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/ProductAdmin/Create
        public IActionResult Create()
        {
            ViewData["NameBrand"] = new SelectList(_context.Brands, "BrandId", "NameBrand");
            ViewData["NameCategory"] = new SelectList(_context.Categories, "CategoryId", "NameCategory");
            ViewData["NameSpecification"] = new SelectList(_context.Specifications, "SpecificationId", "NameSpecification");
            return View();
        }

        // POST: Admin/ProductAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,BrandId,CategoryId,NameProduct,Gb,SpecificationId,ProductImage,Price,PriceOld,Alias,SpNoiBat,SpGiamGia,PhanTramGiamGia")] Product product , List<IFormFile> userfiles)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (userfiles.Count > 0)
                    {
                        foreach (var file in userfiles)
                        {
                            string filename = file.FileName;
                            filename = Path.GetFileName(filename);
                            //đường dẫn của file
                            string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//IMAGE/Products", filename);
                            var stream = new FileStream(uploadpath, FileMode.Create);
                            file.CopyToAsync(stream);
                            product.ProductImage = filename; //gán giá trị cho cột SeoDescription                            

                        }
                        ViewBag.Message = "Total" + userfiles.Count.ToString() + "Files Upoaded Successfully.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error while uploading the file";
                }

                product.Alias = Utilities.SEOUrl(product.NameProduct);
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandId", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["SpecificationId"] = new SelectList(_context.Specifications, "SpecificationId", "SpecificationId", product.SpecificationId);
            return View(product);
        }

        // GET: Admin/ProductAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            /*ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandId", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["SpecificationId"] = new SelectList(_context.Specifications, "SpecificationId", "SpecificationId", product.SpecificationId);*/

            ViewData["NameBrand"] = new SelectList(_context.Brands, "BrandId", "NameBrand" , product.BrandId);
            ViewData["NameCategory"] = new SelectList(_context.Categories, "CategoryId", "NameCategory", product.CategoryId);
            ViewData["NameSpecification"] = new SelectList(_context.Specifications, "SpecificationId", "NameSpecification", product.SpecificationId);
            
            return View(product);
        }

        // POST: Admin/ProductAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,BrandId,CategoryId,NameProduct,Gb,SpecificationId,ProductImage,Price,PriceOld,Alias,SpNoiBat,SpGiamGia,PhanTramGiamGia")] Product product, List<IFormFile> userfiles)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (userfiles.Count > 0)
                    {
                        foreach (var file in userfiles)
                        {
                            string filename = file.FileName;
                            filename = Path.GetFileName(filename);
                            //đường dẫn của file
                            string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//IMAGE/Products", filename);
                            var stream = new FileStream(uploadpath, FileMode.Create);
                            file.CopyToAsync(stream);
                            product.ProductImage = filename; //gán giá trị cho cột SeoDescription                            

                        }
                        ViewBag.Message = "Total" + userfiles.Count.ToString() + "Files Upoaded Successfully.";
                    }
                    product.Alias = Utilities.SEOUrl(product.NameProduct);
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandId", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["SpecificationId"] = new SelectList(_context.Specifications, "SpecificationId", "SpecificationId", product.SpecificationId);
            return View(product);
        }

        // GET: Admin/ProductAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Specification)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/ProductAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'WebOnline1Context.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
