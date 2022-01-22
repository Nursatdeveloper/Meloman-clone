using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meloman_clone.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public string ProductType { get; set; }
        public string Username { get; set; }
        public string Advantages { get; set; }
        public string Disadvantages { get; set; }
        public string GeneralReview { get; set; }
        public decimal Rating { get; set; }
        public DateTime Time { get; set; }
    }
}
