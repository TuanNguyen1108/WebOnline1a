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
        public async Task<IActionResult> Index(int page = 1, int pageSize = 8)
        {
            // hiện tên khi đăng nhập
            ViewBag.UserName = HttpContext.Session.GetString("HoTenAdmin");

            // Lấy tổng số sản phẩm từ cơ sở dữ liệu
            int totalSpecifications = await _context.Specifications.CountAsync();
            // Tính toán tổng số trang dựa trên tổng số sản phẩm và kích thước trang
            int totalPages = (int)Math.Ceiling((double)totalSpecifications / pageSize);

            var specifications = _context.Specifications
                .AsNoTracking()
                .OrderByDescending(s=>s.SpecificationId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Truyền dữ liệu phân trang vào view
            ViewBag.TotalPages = totalPages;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;

            return View(specifications);
        }

        // GET: Admin/SpecificationAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Specifications == null)
            {
                return NotFound();
            }

            var specification = _context.Specifications
                .Where(s => s.SpecificationId == id)
                .AsNoTracking()
                .Select(s => new Specification
                {
                    NameSpecification = s.NameSpecification,
                    SizeCreen = s.SizeCreen,
                    Cpu = s.Cpu,
                    OperatingSystem = s.OperatingSystem,
                    Ram = s.Ram,
                    Camera = s.Camera,
                    CameraSelfie = s.CameraSelfie,
                    Battery = s.Battery,
                    Description = s.Description,
                })
                .FirstOrDefault();

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
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            return View(specification);
        }

        public IActionResult Delete(int id)
        {
            var specification = _context.Specifications.SingleOrDefault(x => x.SpecificationId == id);
            if (specification != null)
            {
                _context.Specifications.Remove(specification);
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        /*// GET: Admin/SpecificationAdmin/Delete/5
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
        }*/
    }
}
