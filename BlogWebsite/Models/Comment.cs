using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogWebsite.Models
{
    public class Comment
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Comments { get; set; }
        public int ParentID { get; set; }

        public virtual Post Post{ get; set; }
    }
}