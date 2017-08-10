using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogWebsite.Models
{
    public class Likes
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public string UserName { get; set; }
    }
}