using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLab2.DataModels
{
    public class Store: INotifyPropertyChanged
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MaxLength(255)]
        public string StoreName { get; set; }
        [Required]
        [MaxLength(255)]
        public string Address { get; set; }
        [MaxLength(100)]
        public string City { get; set; }

        private ICollection<StockBalance> _stockBalance { get; set; }
        public ICollection<StockBalance> StockBalance
        {
            get => _stockBalance ?? (_stockBalance = new List<StockBalance>());
            set => _stockBalance = value;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
