using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLab2.DataModels
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }
        [Phone]
        [StringLength(12)]
        public string PhoneNumber { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
