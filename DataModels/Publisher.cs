using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLab2.DataModels
{
    public class Publisher
    {
        public int PublisherID { get; set; }
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
