using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BlogWebsite.Models;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace BlogWebsite.Controllers
{
    public class AccountController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        const string PostAddModelKey = "_post_model";
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (Membership.ValidateUser(model.Username, model.password))
            {
                FormsAuthentication.SetAuthCookie(model.Username,true);
                Session["Details"] = model.Username;
              
                return RedirectToAction("Index","Posts");
            }
            return RedirectToAction("Login");
        }


        // GET: Account/Register
        [HttpGet]
        public ActionResult Register()
        {
           
        
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel user)
        {
            Membership.CreateUser(user.Username,user.Password,user.Email);
           
            return RedirectToAction("");
        }
        public ActionResult CreateAuthor()
        {
            return View();
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Posts");
        }

        public ActionResult ShowProfile()
        {
           
               var user = Membership.GetUser();
            RegisterModel r = new RegisterModel();
            r.Username = user.UserName;
            r.Email = user.Email;
            ProfileView p = new ProfileView();
           
            
            p.Register.Email = r.Email;
            p.Register.Username = r.Username;
            try
            {
               Author a = db.Authors.Where(X =>X.UserName.Equals (r.Username)).FirstOrDefault();
                p.ath.FullName = a.FullName;
                p.ath.Expertise = a.Expertise;
                p.ath.About = a.About;
            }
            catch
            {
                
            }
            return View(p);
        }

        [HttpPost]
        public ActionResult ShowProfile(ProfileView model, HttpPostedFileBase file)
        {
            Author a = new Author();          
            var user = Membership.GetUser(); 
             user.Email=model.Register.Email;
            a.UserName = model.Register.Username;          
            a.FullName = model.ath.FullName;
           a.Expertise=model.ath.Expertise;
            a.About = model.ath.About;
            db.Authors.Add(a);
            db.SaveChanges();
            Membership.UpdateUser(user);


            Image myImg = Image.FromStream(file.InputStream, true, true);
                ImageModel img = new ImageModel();
                using (var d = new ApplicationDbContext())
                {
                    img.Data = ConvertToBytes(myImg);
                    img.Username = User.Identity.Name;
                    img.MimeType = myImg.GetType().ToString();
                    d.Images.Add(img);
                    d.SaveChanges();
                }
         
                return RedirectToAction("Index", "Posts");          
        }
        public ActionResult GetImage(string Username)
        {
            using (var d = new ApplicationDbContext())
            {

                var image = d.Images.FirstOrDefault(x => x.Username == Username);
                if (image != null)
                {
                    byte[] arr = image.Data;

                    MemoryStream ms = new MemoryStream(arr);
                    //Image returnImage = Image.FromStream(ms); 
                    return new FileContentResult(ms.ToArray(), "image/*");
                }

                else
                    return new EmptyResult();

            }

        }
        private static byte[] ConvertToBytes(Image myImg)
        {
            MemoryStream ms = new MemoryStream();
            myImg.Save(ms, ImageFormat.Jpeg);
            return ms.ToArray();
        }

    }
}