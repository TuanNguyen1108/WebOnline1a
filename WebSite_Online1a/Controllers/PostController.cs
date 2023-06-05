using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSite_Online1a.Models;
/*using PagedList.Core;*/
using X.PagedList;

namespace WebSite_Online1a.Controllers
{
    public class PostController : Controller
    {
        private readonly WebOnline1Context _context;
        public PostController(WebOnline1Context context)
        {
            _context = context;
        }

        [Route("Tin-tuc.html", Name = "Post")]
        public IActionResult Index(int page = 1)
        {
            page = page < 1?1 : page; // biểu thức 3 ngôi
            int pageSize = 6;
            /*var post = _context.Posts.ToPagedList(page, pageSize);*/ 
            var post = _context.Posts.AsNoTracking().OrderByDescending(x => x.PostId).Where(x=>x.Published == true).ToPagedList(page, pageSize);
            /*lấy theo ID, id tạo sau sẽ hiện lên đầu tiên*/
            
            var lsTinNoiBat = _context.Posts
                .AsNoTracking()
                .Where(x => x.IsHot == true && x.Published == true)
                .Take(5)
                .ToList();
            ViewBag.TinNoiBat = lsTinNoiBat;

            return View(post);
        }
        [Route("/tin-tuc/{Alias}-{id}.html", Name = "TinDetails")]
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

            var lsBaiVietLienQuan = _context.Posts
                .AsNoTracking()
                .Where(x => x.Published == true && x.PostId != id)
                .Take(3)
                /*.OrderByDescending(x => x.CreateDate)*/
                .ToList();
            ViewBag.BaiVietLienQuan = lsBaiVietLienQuan;

            var lsTinNoiBat = _context.Posts
                .AsNoTracking()
                .Where(x => x.IsHot == true)
                .Take(5)
                .ToList();
            ViewBag.TinNoiBat = lsTinNoiBat;

            return View(post);
        }
    }
}
