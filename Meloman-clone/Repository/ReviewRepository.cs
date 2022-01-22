using Meloman_clone.Data;
using Meloman_clone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meloman_clone.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationContext _context;
        public ReviewRepository(ApplicationContext context)
        {
            _context = context;
        }

        public List<Review> GetReviews(int productId, string productType)
        {
            if(productType == "All" && productId == 0)
            {
                return _context.Reviews.ToList();
            }
            var reviews = from review in _context.Reviews.ToList()
                          where review.ProductType == productType && review.ProductId == productId
                          select review;
            
            return reviews.ToList();

        }

        public bool SaveReview(Review review)
        {
            _context.Reviews.Add(review);
            int obj = _context.SaveChanges();
            if(obj != 0) { return true;  }
            return false;
        }
    }
}
