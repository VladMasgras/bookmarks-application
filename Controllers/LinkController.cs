using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProiectDaw.Models;
using Microsoft.AspNet.Identity;

namespace ProiectDaw.Controllers
{
    public class LinkController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: Link
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<Link> links = db.Links.ToList();
            ViewBag.Links = links;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Create(BookmarkLinkVM linkVM)
        {
            var bookmark = db.Bookmarks.Find(linkVM.BookmarkId);
            linkVM.BookmarkList = GetAllBookmarks(User.Identity.GetUserId());
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            try
            {
                if (ModelState.IsValid)
                {
                    Link link = new Link
                    {
                        UrlBody = linkVM.UrlBody,
                        Bookmark = bookmark
                    };
                    db.Links.Add(link);
                    db.SaveChanges();
                    return RedirectToAction("Show", "Bookmark", new { id = linkVM.BookmarkId });
                }
                return View(linkVM);
            }
            catch(Exception e)
            {
                var msg = e.Message;
                return View(linkVM);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Create()
        {
            BookmarkLinkVM link = new BookmarkLinkVM();
            link.BookmarkList = GetAllBookmarks(User.Identity.GetUserId());

            return View(link);
        }

        [HttpPost]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Delete(int id)
        {
            Link link = db.Links.Find(id);
            if (link != null)
            {
                int linkId = link.LinkId;

                db.Links.Remove(link);
                db.SaveChanges();
                return RedirectToAction("Show", "Bookmark", new { id = linkId });
            }

            return HttpNotFound("Couldn't find the link with id " + id.ToString());
        }

        [HttpPost]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Edit(Link link)
        {
            if (ModelState.IsValid)
            {
                var lnk = db.Links.Find(link.LinkId);
                int Id = lnk.LinkId;

                lnk.UrlBody = link.UrlBody;
                db.SaveChanges();

                return RedirectToAction("Show", "Bookmark", new { id = Id });
            }

            return View(link);
        }

        [HttpGet]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Edit(int id)
        {
            var link = db.Links.Find(id);
            if (link != null)
            {
                return View(link);
            }

            return HttpNotFound("Couldn't find the link with id " + id.ToString());
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllBookmarks(string UserId)
        {
            var selectList = new List<SelectListItem>();

            foreach (var bookmark in db.Bookmarks.ToList())
            {
                if (bookmark.UserId == UserId)
                {
                    selectList.Add(new SelectListItem
                    {
                        Value = bookmark.BookmarkId.ToString(),
                        Text = bookmark.Title
                    });
                }
            }

            return selectList;
        }
    }
}