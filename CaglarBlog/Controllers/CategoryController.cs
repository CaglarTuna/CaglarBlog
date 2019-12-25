using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaglarBlog.Models;

namespace CaglarBlog.Controllers
{
    public class CategoryController : Controller
    {
        CaglarBlogEntities db = new CaglarBlogEntities();

        // GET: Kategori
        public ActionResult Index()
        {
            var model = db.Category.ToList();

            return View(model);
        }

        public ActionResult KategoriGoster(int? id)
        {
            var model = db.Category.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View("KategoriGoster", model);
        }
    }
}