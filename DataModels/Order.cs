using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLab2.DataModels
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int CustomerID { get; set; }

        public Customer Customer { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
