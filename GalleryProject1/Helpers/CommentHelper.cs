using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GalleryProject.Models;
using System.Data.Entity;
using System.Web.Mvc;
using System.Text;

namespace GalleryProject.Helpers
{
    public static class CommentHelper
    {
        static GalleryContext db = new GalleryContext();

        public static IEnumerable<Comment> Comment(int id)
        {
            return  db.Comments.Where(c => c.PhotoId == id).OrderBy(c => c.Id);
            
        }
    }
}