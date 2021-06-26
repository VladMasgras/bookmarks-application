using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ProiectDaw.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [MinLength(3, ErrorMessage = "Comment cannot be less than 3")]
        public string CommentBody { get; set; }
        public int BookmarkId { get; set; }
        public virtual Bookmark Bookmark { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}