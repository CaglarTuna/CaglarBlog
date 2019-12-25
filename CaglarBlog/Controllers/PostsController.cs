using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CaglarBlog.Models;

namespace CaglarBlog.Controllers
{
    public class PostsController : Controller
    {
        CaglarBlogEntities db = new CaglarBlogEntities();

        public ActionResult Index()
        {

            var model = db.Post.ToList();

            return View(model);
        }
        [Authorize(Roles ="Admin")]
        public ActionResult DeletePost(int id)
        {
            var idd = db.Comment.Where(x => x.Postid == id).FirstOrDefault().Postid;
            bool check = true;

            while (id==idd&&check==true)
            {
                var iddd = db.Comment.Where(x => x.Postid == id).FirstOrDefault().CommentID;
                Comment ct = db.Comment.Find(iddd);
                db.Comment.Remove(ct);
                db.SaveChanges();
                check = db.Comment.Where(x => x.Postid == id).Any(x=>x.CommentIcerik.Length>0);
            }
            
            Post post = db.Post.Find(id);
            db.Post.Remove(post);
            db.SaveChanges();
            return View("Goster");
        }
        public ActionResult Goster(int? id)
        {
            var model = db.Post.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View("Goster", model);
        }


        public ActionResult YorumList()
        {
            var model = db.Comment.ToList();
            return View(model);
        }
        public ActionResult kayitDuzenle(int? id)
        {
            Post post = db.Post.Where(k => k.PostID == id).SingleOrDefault();
            Makale model = new Makale()
            {
                PostID = post.PostID,
                PostBaslik = post.PostBaslik,
                PostIcerik = post.PostIcerik,
                Like = post.Begeni,
                TimeAdded =post.TimeAdded,
                Category=post.CatID
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult kayitDuzenle(Makale m)
        {
            Post kayit = db.Post.Where(k => k.PostID == m.PostID).SingleOrDefault();
            kayit.PostBaslik = m.PostBaslik;
            kayit.PostIcerik = m.PostIcerik;
            kayit.Begeni = m.Like;
            kayit.TimeAdded = m.TimeAdded;
            kayit.CatID = m.Category;
            db.SaveChanges();
            ViewBag.sonuc = "Kayıt Güncellendi";
            ViewBag.Category = db.Category.Select(a => new SelectListItem { Value = a.CategoryID.ToString(), Text = a.CategoryName }).ToList();
            return View();
        }

        public ActionResult YorumKaydet(Comment ct)
        {
            //var username = User.Identity.Name;
            //var id = db.Users.Where(x => x.Username == username).FirstOrDefault().UserID;
            //ct = db.Comment.FirstOrDefault(a => a.Userid == id);
            db.Comment.Add(ct);
            db.SaveChanges();


            return Redirect(Request.UrlReferrer.PathAndQuery);

        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment ct = db.Comment.Find(id);
            if (ct == null)
            {
                return HttpNotFound();
            }
            return View(ct);
        }

        [HttpPost,ActionName("Delete")]
        public ActionResult YorumSil(int id)
        {
           // var com = db.Comment.Where(x => x.Postid == id).Select(x => x.CommentID);
            Comment ct = db.Comment.Find(id);
            db.Comment.Remove(ct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult LikeKaydet(int? id)
        {

            var guncelrating = db.Post.Find(id);

            if (guncelrating.Begeni == null)
            {
                guncelrating.Begeni = 1;
            }
            else
            {
                guncelrating.Begeni += 1;
            }


            db.SaveChanges();

            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        public ActionResult PostKaydet(Post post)
        {
            db.Post.Add(post);
            db.SaveChanges();
            return View();
        }
    }
}
