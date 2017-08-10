using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogWebsite.Models
{
    public class ProfileView
    {

        public ProfileView()
        {
            this.Register = new RegisterModel();
            this.ath = new Author();
                  }
        public RegisterModel Register { get; set; }
        
        public Author ath { get; set; }
    }
}