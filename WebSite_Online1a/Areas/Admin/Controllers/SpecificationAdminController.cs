using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Website_Online.Areas.Admin.Models.Authentication;
using WebSite_Online1a.Models;

namespace WebSite_Online1a.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authentication]
    public class SpecificationAdminController : Controller
    {
        private readonly WebOnline1Context _context;

        public SpecificationAdminController(WebOnline1Context context)
        {
            _context = context;
        }

        // GET: Admin/SpecificationAdmin
        public async Task<IActionResult> Index(int? page)
        {
            // hiện tên khi đăng nhập
            ViewBag.UserName = HttpContext.Session.GetString("HoTenAdmin");

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            //var pageSize = Utilities.PAGE_SIZE;//20
            var pageSize = 5;
            var lsSpecification = _context.Specifications.AsNoTracking().OrderByDescending(x => x.SpecificationId);
            // (x => x.CategoryId): lấy theo ID, id tạo sau sẽ hiện lên đầu tiên, có nghĩa là 1,2,3,4 thì 1-2 sẽ nằm trang 2, 3-4 sẽ nằm trang 1

            PagedList<Specification> models = new PagedList<Specification>(lsSpecification, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/SpecificationAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Specifications == null)
            {
                return NotFound();
            }

            var specification = await _context.Specifications
                .FirstOrDefaultAsync(m => m.SpecificationId == id);
            if (specification == null)
            {
                return NotFound();
            }

            return View(specification);
        }

        // GET: Admin/SpecificationAdmin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/SpecificationAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpecificationId,NameSpecification,Description,Cpu,SizeCreen,OperatingSystem,Ram,Camera,CameraSelfie,Battery")] Specification specification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(specification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specification);
        }

        // GET: Admin/SpecificationAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Specifications == null)
            {
                return NotFound();
            }

            var specification = await _context.Specifications.FindAsync(id);
            if (specification == null)
            {
                return NotFound();
            }
            return View(specification);
        }

        // POST: Admin/SpecificationAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpecificationId,NameSpecification,Description,Cpu,SizeCreen,OperatingSystem,Ram,Camera,CameraSelfie,Battery")] Specification specification)
        {
            if (id != specification.SpecificationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecificationExists(specification.SpecificationId))
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
            return View(specification);
        }

        // GET: Admin/SpecificationAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Specifications == null)
            {
                return NotFound();
            }

            var specification = await _context.Specifications
                .FirstOrDefaultAsync(m => m.SpecificationId == id);
            if (specification == null)
            {
                return NotFound();
            }

            return View(specification);
        }

        // POST: Admin/SpecificationAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Specifications == null)
            {
                return Problem("Entity set 'WebOnline1Context.Specifications'  is null.");
            }
            var specification = await _context.Specifications.FindAsync(id);
            if (specification != null)
            {
                _context.Specifications.Remove(specification);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecificationExists(int id)
        {
          return (_context.Specifications?.Any(e => e.SpecificationId == id)).GetValueOrDefault();
        }
    }
}
