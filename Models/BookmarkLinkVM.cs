using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using ProiectDaw.Models.MyValidation;

namespace ProiectDaw.Models
{
    public class BookmarkLinkVM
    {
        /*
        [Required(ErrorMessage = "Title is mandatory")]
        [StringLength(20, ErrorMessage = "Title can't have more than 20 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is mandatory")]
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string ImagePath { get; set; }
        */
        public int BookmarkId { get; set; }
        [LinkValidator]
        public string UrlBody { get; set; }
        public IEnumerable<SelectListItem> BookmarkList { get; set; }
    }
}