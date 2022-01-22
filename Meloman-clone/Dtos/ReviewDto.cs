using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meloman_clone.Dtos
{
    public class ReviewDto
    {
        public int ProductId { get; set; }
        public string Advantages { get; set; }
        public string Disadvantages { get; set; }
        public string GeneralReview { get; set; }
        public decimal Rating { get; set; }
    }
}
