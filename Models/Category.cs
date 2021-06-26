using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProiectDaw.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Category title can only contain one word")]
        public string Title { get; set; }
        public virtual ICollection<Bookmark> Bookmarks { get; set; }
    }
}