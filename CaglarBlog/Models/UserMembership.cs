using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaglarBlog.Models
{
    public class UserMembership
    {
        public int UserID { get; set; }
        [Required] public string Username { get; set; }
        [Required,MinLength(6,ErrorMessage ="Can not be less then 6 characters!")] public string Password { get; set; }
    }
}