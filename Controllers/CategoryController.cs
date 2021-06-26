using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProiectDaw.Models;
using Microsoft.AspNet.Identity;

namespace ProiectDaw.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: Category
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Include("Bookmarks").SingleOrDefault(c => c.CategoryId == id);
            if (category != null)
            {
                db.Categories.Remove(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return HttpNotFound("Couldn't find the category with id " + id.ToString());
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            var category = db.Categories.Find(id);
            if (category != null)
            {
                return View(category);
            }

            return HttpNotFound("Couldn't find the category with id " + id.ToString());
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var cat = db.Categories.Where(c => c.CategoryId == category.CategoryId).FirstOrDefault();
                cat.Title = category.Title;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(category);
        }
    }
}