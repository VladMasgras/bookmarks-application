using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Globalization;
using System;

namespace ProiectDaw.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new Initp());
        }
        public DbSet<Bookmark> Bookmarks { get; set; } 
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Link> Links { get; set; }

        public class Initp : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
        {
            protected override void Seed(ApplicationDbContext context)
            {
                /*
                Bookmark bookmark = new Bookmark { Title = "asd", Description = "asd", Date = DateTime.Now};
                context.Bookmarks.Add(bookmark);
                Comment comment = new Comment { CommentBody = "asdsadsad", BookmarkId = bookmark.BookmarkId, Date = DateTime.Now };
                context.Comments.Add(comment);
                context.SaveChanges();
                */
                Category category = new Category { Title = "categoria1" };
                Category category1 = new Category { Title = "categoria2" };
                context.Categories.Add(category);
                context.Categories.Add(category1);
                base.Seed(context);
            }
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}