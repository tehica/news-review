using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProgramiranje.Data;
using WebProgramiranje.Models;
using WebProgramiranje.ViewModels;

namespace WebProgramiranje.Areas.Guest.Controllers
{

    [Area("Guest")]
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NewsController(ApplicationDbContext db,
                              IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<News> news = await _db.News.ToListAsync();
            return View(news);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) {
                return NotFound();
            }

            //var newsFromDb = await _db.News.Include(c => c.Comments)
            //                       .FirstOrDefaultAsync(n => n.Id == id);

            ViewBag.loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            NewsCommentsAndUsersViewModel model = new NewsCommentsAndUsersViewModel
            {
                News = await _db.News.FindAsync(id),
                Comments = await _db.Comment.Include(u => u.ApplicationUser)
                                            .Where(c => c.NewsId == id).ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(NewsCommentsAndUsersViewModel model)
        {
            //if(model.Comment == null)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                _db.Comment.Add(model.Comment);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model.Comment);

            //_db.News.Add(model);
            //await _db.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteComment(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var getCommentFromDb = await _db.Comment.FindAsync(id);
            if (getCommentFromDb == null)
            {
                return NotFound();
            }
            else
            {
                _db.Comment.Remove(getCommentFromDb);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
