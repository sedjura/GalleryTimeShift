using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using GalleryProject.Models;
using System.Threading.Tasks;
using GalleryProject.Util;

namespace GalleryProject.Controllers
{
    public class HomeController : Controller
    {

        GalleryContext db = new GalleryContext();

        public async Task<ActionResult> Index()
        {
            IEnumerable<Gallery> galleries = await Task.Run(() => db.Galleries);
            ViewBag.Galleries = galleries;
            return View("Index");
        }

        [HttpGet]
        public ActionResult EditGallery(int id){
            var gallery = db.Galleries.Find(id);
            if (gallery  != null)
            {
                return View(gallery);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult EditGallery(Gallery gallery){
            if (ModelState.IsValid) { 
                db.Entry(gallery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult CreateGallery()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateGallery(Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                db.Galleries.Add(gallery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else {
                 ModelState.AddModelError("Name", "");
                 return View();
            }
        }

        [HttpGet]
        public ActionResult DeleteGallery(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
           Gallery g = db.Galleries.Find(id);
            if (g == null)
            {
                return HttpNotFound();
            }
            return View(g);
        }

        [HttpPost, ActionName("DeleteGallery")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Gallery g = db.Galleries.Find(id);
            if (g == null)
            {
                return HttpNotFound();
            }
            db.Galleries.Remove(g);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public RedirectToRouteResult ViewGallery(int id)
        {
            Gallery g = db.Galleries.Find(id);
            return RedirectToAction("ViewPhotos", "Photo", g);
        }
    }
}
