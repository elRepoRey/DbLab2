using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLab2.DataModels
{
    public class StockBalance: INotifyPropertyChanged
    {
        [Required]
        public int StoreID { get; set; }
        [Required]
        [StringLength(13)]
        public string? ISBN { get; set; }

        [Required]
        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public Store Store { get; set; }
        private Book _book;
        public Book Book
        {
            get => _book;
            set
            {
                if (_book != value)
                {
                    _book = value;
                    OnPropertyChanged(nameof(Book));
                    OnPropertyChanged(nameof(Book.Title)); 
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
