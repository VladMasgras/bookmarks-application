using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ProiectDaw.Models;
using System.Net.Http;
using System.Net;

namespace ProiectDaw.Controllers
{
    public class CollectionController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: Collection
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Index()
        {
            string Id = User.Identity.GetUserId();
            List<Collection> collections = db.Collections.Where(c => c.UserId == Id).ToList();
            ViewBag.collections = collections;
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult AddBookmark(int id)
        {
            var bookmark = db.Bookmarks.Find(id);
            if (bookmark != null)
            {
                string UserId = User.Identity.GetUserId();
                List<Collection> collections = db.Collections.Where(c => c.UserId == UserId).ToList();
                ViewBag.collections = collections;
                ViewBag.bookmarkId = id;
                return View();
            }
            
            return HttpNotFound("Couldn't find the bookmark with id "+ id.ToString());
        }

        [HttpPost]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult AddBookmark(int CollectionId, int BookmarkId)
        {
            string userId = User.Identity.GetUserId();
            Collection collection = db.Collections.Include("Bookmarks").SingleOrDefault(c => c.CollectionId == CollectionId);
            Bookmark bookmark = db.Bookmarks.SingleOrDefault(b => b.BookmarkId == BookmarkId);
            if (collection != null && bookmark != null)
            {
                if (collection.UserId != userId || !User.IsInRole("Administrator"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                collection.Bookmarks.Add(bookmark);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (collection == null)
            {
                return HttpNotFound("Couldn't find the collection with id "+ CollectionId.ToString());
            }

            return HttpNotFound("Couldn't find the bookmark with id " + BookmarkId.ToString());
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Show(int id)
        {
            string userId = User.Identity.GetUserId();
            Collection collection = db.Collections.Include("Bookmarks").SingleOrDefault(c => c.CollectionId == id);
            if (collection != null)
            {
                if (collection.UserId != userId || !User.IsInRole("Administrator"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                return View(collection);
            }

            return HttpNotFound("Couldn't find the collection with id " + id.ToString());
        }

        [HttpPost]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Delete(int id)
        {
            string userId = User.Identity.GetUserId();
            Collection collection = db.Collections.Include("Bookmarks").SingleOrDefault(c => c.CollectionId == id);
            if (collection != null)
            {
                if (collection.UserId != userId || !User.IsInRole("Administrator"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                db.Collections.Remove(collection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return HttpNotFound("Couldn't find the collection with id " + id.ToString());
        }

        [HttpGet]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Create(Collection collection)
        {
            if (ModelState.IsValid)
            {
                collection.UserId = User.Identity.GetUserId();
                db.Collections.Add(collection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(collection);
        }
    }
}