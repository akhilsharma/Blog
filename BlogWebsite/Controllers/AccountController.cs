using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BlogWebsite.Models;

namespace BlogWebsite.Controllers
{
    public class AccountController : Controller
    {
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
           
            return RedirectToAction("CreateAuthor");
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



            return View(r);
        }

        [HttpPost]
        public ActionResult ShowProfile(RegisterModel model)
        {
            var user = Membership.GetUser();
           
             user.Email=model.Email;
           
            Membership.UpdateUser(user);
           return RedirectToAction("Index","Posts");
        }

     
    }
}