using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace ProiectDaw.Models.MyValidation
{
    public class ImageValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Bookmark bookmark = (Bookmark)validationContext.ObjectInstance;

            
            return base.IsValid(value, validationContext);
        }
    }
}