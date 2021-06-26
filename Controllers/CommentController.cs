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
    public class CommentController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: Comment
        public ActionResult Index()
        {
            List<Comment> comments = db.Comments.ToList();
            ViewBag.Links = comments;
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Create(int Id)
        {
            Comment comment = new Comment()
            {
                BookmarkId = Id
            };
            
            return View(comment);
        }

        [HttpPost]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Date = DateTime.Now;
                comment.UserId = User.Identity.GetUserId();
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Show", "Bookmark", new { id = comment.BookmarkId });
            }

            return View(comment);
        }

        [HttpPost]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Delete(int id)
        {
            string userId = User.Identity.GetUserId();
            Comment comment = db.Comments.Find(id);
            if (comment != null)
            {
                if (comment.UserId != userId || !User.IsInRole("Administrator"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                int bookmarkID = comment.BookmarkId;

                db.Comments.Remove(comment);
                db.SaveChanges();
                return RedirectToAction("Show", "Bookmark", new { id = bookmarkID });
            }

            return HttpNotFound("Couldn't find the comment with id "+ id.ToString());
        }

        [HttpPost]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                var cmnt = db.Comments.Where(c => c.CommentId == comment.CommentId).FirstOrDefault();
                int Id = comment.BookmarkId;

                cmnt.CommentBody = comment.CommentBody;
                db.SaveChanges();

                return RedirectToAction("Show", "Bookmark", new { id = Id });
            }

            return View(comment);
        }

        [HttpGet]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Edit(int id)
        {
            string userId = User.Identity.GetUserId();
            var comment = db.Comments.Find(id);
            if (comment != null)
            {
                if (comment.UserId != userId || !User.IsInRole("Administrator"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                return View(comment);
            }

            return HttpNotFound("Couldn't find the comment with id " + id.ToString());
        }
    }
}