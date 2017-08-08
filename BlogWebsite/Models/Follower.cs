using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogWebsite.Models
{
    public class Follower
    {
        public int ID { get; set; }
        public int Follower_ID { get; set; }
        public int Leader_ID { get; set; }
    }
}