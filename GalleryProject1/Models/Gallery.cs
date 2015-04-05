using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GalleryProject.Models
{
    public class Gallery
    {
        public int Id { get; set; }
        [Required (ErrorMessage = "Поле должно быть установлено")]
        public string Name { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
        
    }
}