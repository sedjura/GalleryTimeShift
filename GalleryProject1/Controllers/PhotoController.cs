using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using GalleryProject.Models;
using System.IO;
using GalleryProject.Util;

namespace GalleryProject.Controllers
{
    public class PhotoController : Controller
    {

        GalleryContext db = new GalleryContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewPhotos(Gallery g) {
            IEnumerable<Photo> photos = db.Photos.Where(p => p.GalleryId == g.Id);
            ViewBag.Gallery = g;
            return View(photos);
        }

        [HttpGet]
        public ActionResult AddPhoto(int? id)
        {
            Gallery g = db.Galleries.Find(id);
            ViewBag.GId = g.Id;
            return View();
        }


        [HttpPost]
        public ActionResult AddPhoto(Photo pic, HttpPostedFileBase uploadImage)
        {
            try
            {
                if (ModelState.IsValid && uploadImage != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    if (imageData.Length > 1024 * 1024 * 2)
                    {
                        return new HtmlResult("Слишком большой файл");
                    }

                    pic.Img = imageData;
                    db.Photos.Add(pic);
                    db.SaveChanges();
                    Gallery g = db.Galleries.Find(pic.GalleryId);
                    return RedirectToAction("ViewPhotos", g);
                }
                else {
                    return View();
                }
            }
            catch (HttpException ex) {
                return new HtmlResult(ex.Message);
            }
            return View(pic);
        }

        [HttpGet]
        public ActionResult EditPhoto(int id)
        {   
            
            var photo = db.Photos.Find(id);
            if (photo != null)
            {
                return View(photo);
            }
            Gallery gallery = db.Galleries.Find(photo.GalleryId);
            return RedirectToAction("ViewPhotos",gallery);
        }

        [HttpPost]
        public ActionResult EditPhoto(Photo photo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photo).State = EntityState.Modified;
                db.SaveChanges();
                Gallery gallery = db.Galleries.Find(photo.GalleryId);
                gallery = db.Galleries.Find(photo.GalleryId);
                return RedirectToAction("ViewPhotos", gallery);
            }
            else {
                return View();
            }
        }

        [HttpGet]
        public ActionResult DeletePhoto(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        [HttpPost, ActionName("DeletePhoto")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Photo photo = db.Photos.Find(id);
            Gallery gallery = db.Galleries.Find(photo.GalleryId);
            if (photo == null)
            {
                return HttpNotFound();
            }
            db.Photos.Remove(photo);
            db.SaveChanges();

            return RedirectToAction("ViewPhotos",gallery);
        }

        public ActionResult PhotoView(int? id) {
            Photo photo = db.Photos.Find(id);
            return View(photo);
        }

        

        [HttpGet]
        public ActionResult CommentAdd(int Id) {
            ViewBag.PhotoId = Id;
            return View();
        }
        [HttpPost]
        public ActionResult CommentAdd(Comment comment) {
            ViewBag.PhotoId = comment.PhotoId;
            if (ModelState.IsValid)
            {
                if (comment.Text==null)
                {
                    ModelState.AddModelError("Comment", "Заполните поле комментария");
                    return View();
                }
                db.Comments.Add(comment);
                db.SaveChanges();
                return View("PhotoView", db.Photos.Find(comment.PhotoId));
            }
            else {
                ModelState.AddModelError("Comment", "!!!");
                return View();
            }
        }

        [HttpGet]
        public ActionResult CommentDelete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Comment c = db.Comments.Find(id);
            if (c == null)
            {
                return HttpNotFound();
            }
            return View(c);
        }

        [HttpPost, ActionName("CommentDelete")]
        public ActionResult DeleteConfirmedComment(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Comment c = db.Comments.Find(id);
            if (c == null)
            {
                return HttpNotFound();
            }
            Photo photo = db.Photos.Find(c.PhotoId);
            db.Comments.Remove(c);
            db.SaveChanges();
            return RedirectToAction("PhotoView",photo);
        }
        [HttpPost]
        public ActionResult LikesControl(string button, int objID)
        {
            Photo photo = (from item in db.Photos where item.Id == objID select item).FirstOrDefault();

            HttpCookie voteCookie = Request.Cookies["Votes"];

            if (voteCookie != null)
            {
                if (voteCookie[objID.ToString()] != null)
                {
                    if (voteCookie[objID.ToString()] == "RatingDown" && button == "RatingUp")
                    {
                        photo.Likes++;
                        voteCookie[objID.ToString()] = button;
                        Response.Cookies.Add(voteCookie);
                        db.SaveChanges();
                        return PartialView("PhotoView", photo);
                    }
                    if (voteCookie[objID.ToString()] == "RatingUp" && button == "RatingDown")
                    {
                        photo.Likes--;
                        voteCookie[objID.ToString()] = button;
                        Response.Cookies.Add(voteCookie);
                        db.SaveChanges();
                        return PartialView("PhotoView", photo);
                    }
                    return PartialView("PhotoView", photo);
                }
            }
            voteCookie = new HttpCookie("Votes");

            if (button == "RatingDown")
                photo.Likes--;
            if (button == "RatingUp")
                photo.Likes++;

            voteCookie[objID.ToString()] = button;
            Response.Cookies.Add(voteCookie);

            db.SaveChanges();
            return PartialView("PhotoView", photo);
        }
    }
}
