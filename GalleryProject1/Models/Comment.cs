using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GalleryProject.Models
{
    public class Comment
    {
        public int Id { get;set; }

        public string Text {get;set;}
        public int PhotoId { get; set; }
        [Required(ErrorMessage = "Поле должно быть установлено")]
        public string Author { get; set; }
    }
}