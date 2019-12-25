using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CaglarBlog.Models;

namespace CaglarBlog.Controllers
{
    public class AccountController : Controller
        
    {

        CaglarBlogEntities db = new CaglarBlogEntities();

        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserMembership us)
        {
            bool isValid = db.Users.Any(x => x.Username == us.Username && x.Password == us.Password);

            if (isValid)
            {
                FormsAuthentication.SetAuthCookie(us.Username, false);
                return RedirectToAction("Index", "Posts");
            }

            ModelState.AddModelError("", "Invalid Username and Password");
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(Users us)
        {
            if (db.Users.Any(x => x.Username == us.Username))
            {
                ModelState.AddModelError("",  "Username exists");
                return View();
            }

            db.Users.Add(us);
            db.SaveChanges();

            ModelState.Clear();
            ViewBag.Success = "Registration Successful";

            return RedirectToAction("Login", new Users());
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult UserList()
        {
            var a = db.Users.ToList();
            db.SaveChanges();

            return View(a);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Users us = db.Users.Find(id);

            if (us == null)
            {
                return HttpNotFound();
            }

            ViewBag.Category =db.Category.Select(m=> new SelectListItem{ Value = m.CategoryID.ToString(),Text=m.CategoryName}).ToList();
            ViewBag.Rol = db.Roles.Select(m => new SelectListItem { Value = m.RoleID.ToString(), Text = m.Rolename }).ToList();

            return View(us);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users us)
        {
            if (ModelState.IsValid)
            {
                db.Entry(us).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Posts");
            }
            else
            {
                return View(us);
            }

        }

        public ActionResult DeleteAccount(int id)
        {
           Users us = db.Users.Find(id);
            db.Users.Remove(us);
            db.SaveChanges();
            return View("UserList","Account");
        }
    }
}