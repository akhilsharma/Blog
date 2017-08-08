using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogWebsite.Models
{
    public class Author
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Expertise { get; set; }
        public List<int> Followers { get; set; }
        public List<int> Follows { get; set; }
    }
}