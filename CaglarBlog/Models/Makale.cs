using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaglarBlog.Models
{
    public class Makale
    {
        public int PostID { get; set; }
        public string PostIcerik { get; set; }
        public string PostBaslik { get; set; }
        public int? Category { get; set; }
        public DateTime? TimeAdded { get; set; }
        public int? Like { get; set; }
    }
}