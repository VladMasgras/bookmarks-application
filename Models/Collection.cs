using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace ProiectDaw.Models
{
    public class Collection
    {
        [Key]
        public int CollectionId { get; set; }
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Collection title can't contain numbers.")]
        [MinLength(3, ErrorMessage = "Collection title cannot be less than 3")]
        public string Title { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Bookmark> Bookmarks { get; set; }

    }
}