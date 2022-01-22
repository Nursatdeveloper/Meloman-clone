using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Meloman_clone.Models
{
    public class OrderProduct
    {
        [Key]
        public int OrderProductId { get; set; }
        public int ProductId { get; set; }
        public string ProductType { get; set; }
        public string ProductName { get; set; }
    }
}
