using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace ProiectDaw.Models.MyValidation
{
    public class LinkValidator:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            BookmarkLinkVM link = (BookmarkLinkVM)validationContext.ObjectInstance;
            string UrlBody = link.UrlBody;

            if (UrlBody == null)
            {
                return new ValidationResult("Url field is required.");
            }

            Regex regex = new Regex(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)");
            
            if (!regex.IsMatch(UrlBody))
            {
                return new ValidationResult("Insert a valid URL");
            }

            return ValidationResult.Success;
        }
    }
}