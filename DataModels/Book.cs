using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLab2.DataModels
{
    public class Book
    {
        [Key]
        [StringLength(13)]
        public string ISBN13 { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
        [Required]
        [MaxLength(50)]
        public string Language { get; set; }
        [Required]
        public decimal Price { get; set; }
        public DateTime? PublicationDate { get; set; }

        [Required]
        public int AuthorID { get; set; }
        [Required]
        public int PublisherID { get; set; }

        public Author Author { get; set; }
        public Publisher Publisher { get; set; }
        public ICollection<StockBalance> StockBalance { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
