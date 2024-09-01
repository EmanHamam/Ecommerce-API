using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.DTOs
{
    public class ReviewDto
    {
        public int ReviewID { get; set; }
        public double? Rating {  get; set; }
        public int? numOfReviews { get; set;}
        public int? ProductID { get; set; }
        public string? Comment { get; set; }
        public string? AppUserID { get; set; }
        public DateTime? ReviewDate { get; set; } = DateTime.Now;
        public string? ProductName { get; set; }

    }
    public class CreateReviewDto
    {
        public int Rating { get; set; }
        public int ProductID { get; set; }
        public string? Comment { get; set; }
        public string AppUserID { get; set; }
 

    }
    public class ProductReviewSummaryDto
    {
        public int ProductID { get; set; }
        public double AverageRating { get; set; }
        public int NumOfReviews { get; set; }
     
    }

}
