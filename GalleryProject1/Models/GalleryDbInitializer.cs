using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace GalleryProject.Models
{
    public class GalleryDbInitializer : DropCreateDatabaseAlways<GalleryContext>
    {

        protected override void Seed(GalleryContext db)
        {
            base.Seed(db);
        }
    }
}