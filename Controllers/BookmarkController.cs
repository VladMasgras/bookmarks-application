using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProiectDaw.Models;
using Microsoft.AspNet.Identity;
using System.Net;

namespace ProiectDaw.Controllers
{
    public class BookmarkController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: Bookmark

        [AllowAnonymous]
        public ActionResult Index()
        {
            List<Bookmark> bookmarks = db.Bookmarks.Include("CommentsList").ToList();
            ViewBag.Bookmarks = bookmarks;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Create(Bookmark bookmark, HttpPostedFileBase file)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

            if (file == null)
            {
                ModelState.AddModelError("ImagePath", "Please upload file");
            }

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/")
                                                  + file.FileName);
                    bookmark.ImagePath = "~/Images/" + file.FileName;
                }
                bookmark.Date = DateTime.Now;
                bookmark.UserId = User.Identity.GetUserId();
                db.Bookmarks.Add(bookmark);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            bookmark.CatergoriesList = GetAllCategories();
            return View(bookmark);
        }

        [AllowAnonymous]
        public ActionResult Show(int id)
        {
            var bookmark = db.Bookmarks.Include("CommentsList").SingleOrDefault(x => x.BookmarkId == id);
            if (bookmark != null)
            {
                ViewBag.IsAdmin = User.IsInRole("Administrator");
                ViewBag.CurrentUser = User.Identity.GetUserId();

                return View(bookmark);
            }
            return HttpNotFound("Couldn't find the bookmark with id " + id.ToString());
        }

        [HttpGet]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Create()
        {
            Bookmark bookmark = new Bookmark();
            bookmark.CatergoriesList = GetAllCategories();
            return View(bookmark);
        }

        [HttpGet]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Edit(int id)
        {
            string userId = User.Identity.GetUserId();
            var bookmark = db.Bookmarks.Where(b => b.BookmarkId == id).FirstOrDefault();
            if (bookmark != null)
            {
                if (bookmark.UserId != userId || !User.IsInRole("Administrator"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                bookmark.CatergoriesList = GetAllCategories();
                return View(bookmark);
            }

            return HttpNotFound("Couldn't find the bookmark with id " + id.ToString());
        }

        [HttpPost]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Edit(Bookmark bookmark)
        {
            if (ModelState.IsValid)
            {
                var bkmrk = db.Bookmarks.Where(b => b.BookmarkId == bookmark.BookmarkId).FirstOrDefault();
                bkmrk.Title = bookmark.Title;
                bkmrk.Description = bookmark.Description;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(bookmark);
        }

        [HttpPost]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Delete(int id)
        {
            string userId = User.Identity.GetUserId();
            Bookmark bookmark = db.Bookmarks.Include("CommentsList").FirstOrDefault(b => b.BookmarkId == id);
            if (bookmark != null)
            {
                if (bookmark.UserId != userId || !User.IsInRole("Administrator"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                foreach (Comment comment in bookmark.CommentsList)
                {
                    db.Comments.Remove(comment);
                }

                db.Bookmarks.Remove(bookmark);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return HttpNotFound("Couldn't find the bookmark with id " + id.ToString());
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            var selectList = new List<SelectListItem>();

            foreach(var category in db.Categories.ToList())
            {
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.Title
                });

            }

            return selectList;
        }
    }
}