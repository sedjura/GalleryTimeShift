using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace GalleryProject.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public byte[] Img { get; set; }
        public int Likes { get; set; }
        [StringLength(150)]
        public string Tags { get; set; }
        public int GalleryId { get; set; }
        [Required(ErrorMessage = "Поле должно быть установлено")]
        public string Description { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}