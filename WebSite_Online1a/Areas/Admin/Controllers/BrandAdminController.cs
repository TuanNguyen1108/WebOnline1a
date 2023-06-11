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
    public class BrandAdminController : Controller
    {
        private readonly WebOnline1Context _context;

        public BrandAdminController(WebOnline1Context context)
        {
            _context = context;
        }

        // GET: Admin/BrandAdmin
        public async Task<IActionResult> Index()
        {
            // hiện tên khi đăng nhập
            ViewBag.UserName = HttpContext.Session.GetString("HoTenAdmin");

            /*var webOnline1Context = _context.Brands.Include(c => c.Categories);
            return View(await webOnline1Context.ToListAsync());*/
            return _context.Brands != null ?
                        View(await _context.Brands.ToListAsync()) :
                        Problem("Entity set 'WebOnline1Context.Brands'  is null.");
        }

        // GET: Admin/BrandAdmin/Details/5
        /*public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }*/

        // GET: Admin/BrandAdmin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/BrandAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrandId,NameBrand,Alias")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                brand.Alias = Utilities.SEOUrl(brand.NameBrand);
                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(brand);
        }

        // GET: Admin/BrandAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Admin/BrandAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BrandId,NameBrand,Alias")] Brand brand)
        {
            if (id != brand.BrandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    brand.Alias = Utilities.SEOUrl(brand.NameBrand);
                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        public IActionResult Delete(int id)
        {
            var brand = _context.Brands.SingleOrDefault(x => x.BrandId == id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: Admin/BrandAdmin/Delete/5
        /*public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Admin/BrandAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Brands == null)
            {
                return Problem("Entity set 'WebOnline1Context.Brands'  is null.");
            }
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(int id)
        {
          return (_context.Brands?.Any(e => e.BrandId == id)).GetValueOrDefault();
        }*/
    }
}
