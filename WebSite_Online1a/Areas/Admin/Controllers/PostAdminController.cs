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
    public class PostAdminController : Controller
    {
        private readonly WebOnline1Context _context;

        public PostAdminController(WebOnline1Context context)
        {
            _context = context;
        }

        // GET: Admin/PostAdmin
        public async Task<IActionResult> Index()
        {
            // hiện tên khi đăng nhập
            ViewBag.UserName = HttpContext.Session.GetString("HoTenAdmin");

            return _context.Posts != null ? 
                          View(await _context.Posts.ToListAsync()) :
                          Problem("Entity set 'WebOnline1Context.Posts'  is null.");
        }

        // GET: Admin/PostAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Admin/PostAdmin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/PostAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,Contents,Image,Published,IsHot,IsNewfeed,CreateDate,Author,MetaKey,MetaDesc,Views,Alias")] Post post, List<IFormFile> userfiles)
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
                            string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//IMAGE/News", filename);
                            var stream = new FileStream(uploadpath, FileMode.Create);
                            file.CopyToAsync(stream);
                            post.Image = filename; //gán giá trị cho cột Image                           

                        }
                        ViewBag.Message = "Total" + userfiles.Count.ToString() + "Files Upoaded Successfully.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error while uploading the file";
                }
                if (string.IsNullOrEmpty(post.Image)) post.Image = "default.jpg";

                post.Alias = Utilities.SEOUrl(post.Title);
                post.CreateDate = DateTime.Now;
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Admin/PostAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Admin/PostAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Contents,Image,Published,IsHot,IsNewfeed,CreateDate,Author,MetaKey,MetaDesc,Views,Alias")] Post post, List<IFormFile> userfiles)
        {
            if (id != post.PostId)
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
                            string filename1 = file.FileName;
                            filename1 = Path.GetFileName(filename1);
                            //đường dẫn của file
                            string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//IMAGE/News", filename1);
                            var stream = new FileStream(uploadpath, FileMode.Create);
                            file.CopyToAsync(stream);
                            post.Image = filename1; //gán giá trị cho cột Image                           
                        }
                        ViewBag.Message = "Total" + userfiles.Count.ToString() + "Files Upoaded Successfully.";
                    }
                    post.Alias = Utilities.SEOUrl(post.Title);
                    post.CreateDate = DateTime.Now;
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
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
            return View(post);
        }

        // GET: Admin/PostAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Admin/PostAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'WebOnline1Context.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
          return (_context.Posts?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
