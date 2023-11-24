using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLab2.DataModels
{
    public class OrderDetail
    {
        [Required]
        public int OrderID { get; set; }
        [Required]
        [StringLength(13)]
        public string ISBN { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal PricePerUnit { get; set; }

        public Order Order { get; set; }
        public Book Book { get; set; }
    }
}
