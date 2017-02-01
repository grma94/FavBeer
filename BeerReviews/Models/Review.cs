using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BeerReviews.Models
{
    public class Review
    {
        public int ReviewID { get; set; }
        public int Aroma { get; set; }
        public int Taste { get; set; }
        public int Palate { get; set; }
        public int Apperance { get; set; }
        public double Overall { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Date { get; set; }
        public string UserID { get; set; }
        public int BeerID { get; set; }

        public virtual Beer Beer { get; set; }
     //   public virtual ApplicationUser User { get; set; }
    }
}