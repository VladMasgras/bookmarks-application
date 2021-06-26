using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace ProiectDaw.Models
{
    public class Bookmark
    {
        [Key]
        public int BookmarkId { get; set; }
        [Required(ErrorMessage = "Title is mandatory")]
        [MaxLength(20, ErrorMessage = "Title can't have more than 20 characters")]
        [RegularExpression(@"^[A-Z].+", ErrorMessage ="The title must start with a capital letter.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is mandatory")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage ="The description can contain only alphanumeric characters and spaces")]
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string ImagePath { get; set; }
        public virtual ICollection<Comment> CommentsList { get; set; }
        [Required(ErrorMessage = "Select a category")]
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> CatergoriesList { get; set; }
        public virtual ICollection<Collection> Collections { get; set; }
        public virtual Link Link { get; set; }



    }
}