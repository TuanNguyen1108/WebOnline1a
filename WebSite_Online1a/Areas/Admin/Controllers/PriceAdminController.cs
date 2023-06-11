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
        public async Task<IActionResult> Index(int page = 1, int pageSize = 8)
        {
            // hiện tên khi đăng nhập
            ViewBag.UserName = HttpContext.Session.GetString("HoTenAdmin");

            // Lấy tổng số sản phẩm từ cơ sở dữ liệu
            int totalPrice = await _context.Prices.CountAsync();
            // Tính toán tổng số trang dựa trên tổng số sản phẩm và kích thước trang
            int totalPages = (int)Math.Ceiling((double)totalPrice / pageSize);

            var specifications = _context.Prices
               .OrderByDescending(p => p.PriceId)
              .AsNoTracking()
              .Skip((page - 1) * pageSize)
              .Take(pageSize)
              .ToList();

            // Truyền dữ liệu phân trang vào view
            ViewBag.TotalPages = totalPages;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;

            return View(specifications);
        }


        // GET: Admin/PriceAdmin/Create
        public IActionResult Create()
        {
            ViewData["NameProduct"] = new SelectList(_context.Products, "ProductId", "NameProduct");
            return View();
        }

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
                _context.Update(price);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", price.ProductId);
            return View(price);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Prices == null)
            {
                return NotFound();
            }

            var price = await _context.Prices
                /*.Include(p => p.Product)*/
                .FirstOrDefaultAsync(m => m.PriceId == id);
            if (price != null)
            {
                _context.Prices.Remove(price);
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }




        /*// GET: Admin/PriceAdmin/Delete/5
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
        }*/
    }
}
