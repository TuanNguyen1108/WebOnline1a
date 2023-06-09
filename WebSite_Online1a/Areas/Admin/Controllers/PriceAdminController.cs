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
    public class PriceAdminController : Controller
    {
        private readonly WebOnline1Context _context;

        public PriceAdminController(WebOnline1Context context)
        {
            _context = context;
        }

        // GET: Admin/PriceAdmin
        public async Task<IActionResult> Index(int? page)
        {
            // hiện tên khi đăng nhập
            ViewBag.UserName = HttpContext.Session.GetString("HoTenAdmin");

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            //var pageSize = Utilities.PAGE_SIZE;//20
            var pageSize = 10;
            var lsPrice = _context.Prices.AsNoTracking().OrderByDescending(x => x.PriceId);
            // (x => x.CategoryId): lấy theo ID, id tạo sau sẽ hiện lên đầu tiên, có nghĩa là 1,2,3,4 thì 1-2 sẽ nằm trang 2, 3-4 sẽ nằm trang 1

            PagedList<Price> models = new PagedList<Price>(lsPrice, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);

            /*  var webOnline1Context = _context.Prices.Include(p => p.Product);
              return View(await webOnline1Context.ToListAsync());*/
        }

        // GET: Admin/PriceAdmin/Details/5
        /*public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Prices == null)
            {
                return NotFound();
            }

            var price = await _context.Prices
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.PriceId == id);
            if (price == null)
            {
                return NotFound();
            }

            return View(price);
        }*/

        // GET: Admin/PriceAdmin/Create
        public IActionResult Create()
        {
            ViewData["NameProduct"] = new SelectList(_context.Products, "ProductId", "NameProduct");
            return View();
        }

        // POST: Admin/PriceAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PriceId,NamePrice,ProductId,PriceOld,PriceNew,Gb,Color,ColorImage")] Price price, List<IFormFile> userfiles)
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
                            string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//IMAGE/Product_Details", filename);
                            var stream = new FileStream(uploadpath, FileMode.Create);
                            file.CopyToAsync(stream);
                            price.ColorImage = filename; //gán giá trị cho cột SeoDescription                            

                        }
                        ViewBag.Message = "Total" + userfiles.Count.ToString() + "Files Upoaded Successfully.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error while uploading the file";
                }
                _context.Add(price);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", price.ProductId);
            return View(price);
        }

        // GET: Admin/PriceAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Prices == null)
            {
                return NotFound();
            }

            var price = await _context.Prices.FindAsync(id);
            if (price == null)
            {
                return NotFound();
            }
            ViewData["NameProduct"] = new SelectList(_context.Products, "ProductId", "NameProduct", price.ProductId);
            return View(price);
        }

        // POST: Admin/PriceAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PriceId,NamePrice,ProductId,PriceOld,PriceNew,Gb,Color,ColorImage")] Price price, List<IFormFile> userfiles)
        {
            if (id != price.PriceId)
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
                            string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//IMAGE/Product_Details", filename);
                            FileStream stream = new FileStream(uploadpath, FileMode.Create);
                            file.CopyToAsync(stream);
                            price.ColorImage = filename; //gán giá trị cho cột SeoDescription                            

                        }
                        ViewBag.Message = "Total" + userfiles.Count.ToString() + "Files Upoaded Successfully.";
                    }
                    _context.Update(price);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PriceExists(price.PriceId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", price.ProductId);
            return View(price);
        }

        // GET: Admin/PriceAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Prices == null)
            {
                return NotFound();
            }

            var price = await _context.Prices
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.PriceId == id);
            if (price == null)
            {
                return NotFound();
            }

            return View(price);
        }

        // POST: Admin/PriceAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Prices == null)
            {
                return Problem("Entity set 'WebOnline1Context.Prices'  is null.");
            }
            var price = await _context.Prices.FindAsync(id);
            if (price != null)
            {
                _context.Prices.Remove(price);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PriceExists(int id)
        {
          return (_context.Prices?.Any(e => e.PriceId == id)).GetValueOrDefault();
        }
    }
}
