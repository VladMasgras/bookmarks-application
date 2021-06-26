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
    public class Link
    {
        [Key]
        public int LinkId { get; set; }
        //[LinkValidator]
        public string UrlBody { get; set; }
        [Required]
        public virtual Bookmark Bookmark { get; set; }
    }
}