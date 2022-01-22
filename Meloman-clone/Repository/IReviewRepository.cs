using Meloman_clone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meloman_clone.Repository
{
    public interface IReviewRepository
    {
        bool SaveReview(Review review);
        List<Review> GetReviews(int productId, string productType);
    }
}
