using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebDemo.Helpers;
using Website_Online.Areas.Admin.Models.Authentication;
using WebSite_Online1a.Models;

namespace WebSite_Online1a.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authentication]
    public class CategoryAdminController : Controller
    {
        private readonly WebOnline1Context _context;

        public CategoryAdminController(WebOnline1Context context)
        {
            _context = context;
        }

        // GET: Admin/CategoryAdmin
        public async Task<IActionResult> Index()
        {
            // hiện tên khi đăng nhập
            ViewBag.UserName = HttpContext.Session.GetString("HoTenAdmin");

            var webOnline1Context = _context.Categories.Include(c => c.Brand);
            return View(await webOnline1Context.ToListAsync());
        }

        /*// GET: Admin/CategoryAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Brand)
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }*/

        // GET: Admin/CategoryAdmin/Create
        public IActionResult Create()
        {
            ViewData["NameBrand"] = new SelectList(_context.Brands, "BrandId", "NameBrand");
            return View();
        }

        // POST: Admin/CategoryAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,BrandId,NameCategory,CategoryImage,Alias")] Category category, List<IFormFile> userfiles)
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
                            string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//IMAGE/Categorys", filename);
                            var stream = new FileStream(uploadpath, FileMode.Create);
                            file.CopyToAsync(stream);
                            category.CategoryImage = filename; //gán giá trị cho cột SeoDescription                            

                        }
                        ViewBag.Message = "Total" + userfiles.Count.ToString() + "Files Upoaded Successfully.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error while uploading the file";
                }

                category.Alias = Utilities.SEOUrl(category.NameCategory);
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Brand"] = new SelectList(_context.Brands, "BrandId", "NameBrand", category.BrandId);
            return View(category);
        }

        // GET: Admin/CategoryAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["NameBrand"] = new SelectList(_context.Brands, "BrandId", "NameBrand");
            return View(category);
        }

        // POST: Admin/CategoryAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,BrandId,NameCategory,CategoryImage,Alias")] Category category, List<IFormFile> userfiles)
        {
            if (id != category.CategoryId)
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
                            string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//IMAGE/Categorys", filename);
                            /*var stream = new FileStream(uploadpath, FileMode.Create);*/
                            FileStream stream = new FileStream(uploadpath, FileMode.Create);
                            file.CopyToAsync(stream);

                            /*bị trùng file nó không chịu*/

                            category.CategoryImage = filename; //gán giá trị cho cột SeoDescription                            

                        }
                        ViewBag.Message = "Total" + userfiles.Count.ToString() + "Files Upoaded Successfully.";
                    }

                    category.Alias = Utilities.SEOUrl(category.NameCategory);
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }


            ViewData["Brand"] = new SelectList(_context.Brands, "BrandId", "NameBrand", category.BrandId);
            return View(category);
        }

        public IActionResult Delete(int id)
        {
            var category = _context.Categories.SingleOrDefault(x => x.CategoryId == id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }    
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/CategoryAdmin/Delete/5
        /*public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Brand)
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/CategoryAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'WebOnline1Context.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }*/
    }
}
