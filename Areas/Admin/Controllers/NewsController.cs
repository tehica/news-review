using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProgramiranje.Data;
using WebProgramiranje.Models;

namespace WebProgramiranje.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
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
            if(id == null)
            {
                return NotFound();
            }

            var getNewsFromDb = await _db.News.FindAsync(id);
            if(getNewsFromDb == null)
            {
                return NotFound();
            }

            return View(getNewsFromDb);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(News model)
        {
            if (ModelState.IsValid)
            {
                string imageName = "noimage.png";

                if (model.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    imageName = Guid.NewGuid().ToString() + "_" + model.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await model.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                }

                model.Image = imageName;
                _db.News.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _db.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, News model)
        {
            if (ModelState.IsValid)
            {

                if (model.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    if (!string.Equals(model.Image, "noimage.png"))
                    {
                        // delete old image
                        string oldImagePath = Path.Combine(uploadsDir, model.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string imageName = Guid.NewGuid().ToString() + "_" + model.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await model.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    model.Image = imageName;
                }


                _db.News.Update(model);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var news = await _db.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            else
            {
                // check if the product image has not 'noimage.jpg'
                if (!string.Equals(news.Image, "noimage.png"))
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    string oldImagePath = Path.Combine(uploadsDir, news.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        // removing old image
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _db.News.Remove(news);
                await _db.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index));
        }
    }
}
