using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogWebsite.Models;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace BlogWebsite.Controllers
{
    public class AuthorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Authors
        public ActionResult Index()
        {
            return View(db.Authors.ToList());
        }

        // GET: Authors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // GET: Authors/Create
        public ActionResult Create(string name)
        {if (name == User.Identity.Name)
            {
                return View();
            }
            else
            {
                Author a = new Author();
                var ath  = db.Authors.Where(x => x.UserName == name).SingleOrDefault();
                a.ID = ath.ID;
                a.FullName = ath.FullName;
                a.Expertise = ath.Expertise;
                a.UserName = ath.UserName;
                a.About = ath.About;

                return View("ShowProfile",a);
            }
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FullName,UserName,Expertise,About")] Author author, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                db.Authors.Add(author);
                db.SaveChanges();
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
                return RedirectToAction("Index");
            }

            return View(author);
        }

        // GET: Authors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FullName,UserName,Expertise")] Author author)
        {
            if (ModelState.IsValid)
            {
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
               
                return RedirectToAction("Index");
            }
            return View(author);
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

            } }

        private static byte[] ConvertToBytes(Image myImg)
        {
            MemoryStream ms = new MemoryStream();
            myImg.Save(ms, ImageFormat.Jpeg);
            return ms.ToArray();
        }


        // GET: Authors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Author author = db.Authors.Find(id);
            db.Authors.Remove(author);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Follow(string Follower, int Leader)
        {
            Follower fl = new Follower();
            var follower = db.Authors.Where(x => x.UserName == Follower).FirstOrDefault();
            fl.Follower_ID = follower.ID;
            fl.Leader_ID = Leader;
            db.Followers.Add(fl);
            db.SaveChanges();
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
