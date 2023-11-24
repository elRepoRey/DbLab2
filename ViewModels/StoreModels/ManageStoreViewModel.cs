using DbLab2.DataModels;
using DbLab2.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System;
using GalaSoft.MvvmLight.Command;
using System.Linq;
using System.Windows;
using System.Globalization;
using System.Windows.Controls;

namespace DbLab2.ViewModels.StoreModels
{

    public class ManageStoreViewModel : INotifyPropertyChanged
    {
        private readonly StoreService _storeService;
        private readonly BookService _bookService;
        private Book _selectedBookToAdd;
        private StockBalance _selectedStockBalanceToRemove;
        private int _quantityToAdd;
        private int _quantityToRemove;

        public ObservableCollection<StockBalance> StoreStockBalances { get; private set; }
        public ObservableCollection<Book> AvailableBooks { get; private set; }

        public Store SelectedStore { get; set; }

        public Book SelectedBookToAdd
        {
            get => _selectedBookToAdd;
            set
            {
                if (_selectedBookToAdd != value)
                {
                    _selectedBookToAdd = value;
                    OnPropertyChanged(nameof(SelectedBookToAdd));
                }
            }
        }

        public StockBalance SelectedStockBalanceToRemove
        {
            get => _selectedStockBalanceToRemove;
            set
            {
                if (_selectedStockBalanceToRemove != value)
                {
                    _selectedStockBalanceToRemove = value;
                    OnPropertyChanged(nameof(SelectedStockBalanceToRemove));

                }
            }
        }

        public int QuantityToAdd
        {
            get => _quantityToAdd;
            set
            {
                if (_quantityToAdd != value)
                {
                    _quantityToAdd = value;
                    OnPropertyChanged(nameof(QuantityToAdd));
                }
            }
        }

        public int QuantityToRemove
        {
            get => _quantityToRemove;
            set
            {
                if (_quantityToRemove != value)
                {
                    _quantityToRemove = value;
                    OnPropertyChanged(nameof(QuantityToRemove));
                }
            }
        }

        public ICommand AddBookToStoreCommand { get; private set; }
        public ICommand RemoveBookFromStoreCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ManageStoreViewModel(StoreService storeService, BookService bookService, int storeId)
        {
            _storeService = storeService;
            _bookService = bookService;

            StoreStockBalances = new ObservableCollection<StockBalance>(_storeService.GetStockBalanceByStore(storeId));
            AvailableBooks = new ObservableCollection<Book>(_bookService.GetAllBooks());

            AddBookToStoreCommand = new RelayCommand(AddBookToStore);
            RemoveBookFromStoreCommand = new RelayCommand(RemoveBookFromStore);

            SelectedStore = _storeService.GetStores().FirstOrDefault(s => s.ID == storeId);
        }

        private void AddBookToStore()
        {
            if (SelectedBookToAdd == null)
            {
                MessageBox.Show("No book is selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (QuantityToAdd <= 0)
            {
                MessageBox.Show("Quantity to add must be greater than zero.", "Invalid Quantity", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var existingStockBalance = StoreStockBalances
                    .FirstOrDefault(sb => sb.StoreID == SelectedStore.ID && sb.ISBN == SelectedBookToAdd.ISBN13);

                if (existingStockBalance != null)
                {

                    existingStockBalance.Quantity += QuantityToAdd;
                    _storeService.AddStockBalanceToStore(existingStockBalance);
                }
                else
                {
                    var newStockBalance = new StockBalance
                    {
                        StoreID = SelectedStore.ID,
                        ISBN = SelectedBookToAdd.ISBN13,
                        Quantity = QuantityToAdd
                    };

                    _storeService.AddStockBalanceToStore(newStockBalance);

                    StoreStockBalances.Add(newStockBalance);
                }

                QuantityToAdd = 0;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveBookFromStore()
        {
            if (SelectedStockBalanceToRemove == null)
            {
                MessageBox.Show("No book is selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (QuantityToRemove <= 0)
            {
                MessageBox.Show("Quantity to remove must be greater than zero.", "Invalid Quantity", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (QuantityToRemove > SelectedStockBalanceToRemove.Quantity)
            {
                MessageBox.Show("Cannot remove more than the current stock quantity.", "Invalid Quantity", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (QuantityToRemove == SelectedStockBalanceToRemove.Quantity)
                {
                    _storeService.RemoveStockBalanceFromStore(SelectedStockBalanceToRemove, QuantityToRemove);
                    StoreStockBalances.Remove(SelectedStockBalanceToRemove);
                }
                else
                {
                    _storeService.RemoveStockBalanceFromStore(SelectedStockBalanceToRemove, QuantityToRemove);
                }
                QuantityToRemove = 0;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    public class PositiveIntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse(value.ToString(), out int result))
            {
                if (result > 0)
                    return ValidationResult.ValidResult;
                else
                    return new ValidationResult(false, "Quantity must be greater than zero.");
            }
            return new ValidationResult(false, "Not a valid integer.");
        }
    }
}