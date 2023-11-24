using DbLab2.DataModels;
using DbLab2.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DbLab2.ViewModels.BookModels
{
    public class BookManagementViewModel : INotifyPropertyChanged
    {
        private readonly BookService _bookService;
        public event Action? RequestAddBookView;
        public event Action<Book>? RequestEditBookView;
        public ObservableCollection<Book> Books { get; private set; }

        public ICommand EditBookCommand { get; private set; }
        public ICommand DeleteBookCommand { get; private set; }
        public ICommand AddBookCommand { get; private set; }
        public ICommand RefreshBooksCommand { get; private set; }

        public Book _selectedBook { get; set; }
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public BookManagementViewModel()
        {
            _bookService = new BookService(); 
            Books = new ObservableCollection<Book>(_bookService.GetAllBooks());
         
            EditBookCommand = new RelayCommand<Book>(EditBook);
            DeleteBookCommand = new RelayCommand<Book>(DeleteBook );
            AddBookCommand = new RelayCommand(AddBook);
            RefreshBooksCommand = new RelayCommand(RefreshBooks);
        }

        private void EditBook(Book book)
        {
            RequestEditBookView?.Invoke(book);
        }

        private void DeleteBook(Book book)
        {
            if (book != null)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the book: {book.Title} by {book.Author.FullName}?", "Delete Book", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _bookService.DeleteBook(book);
                    Books.Remove(book);
                    OnPropertyChanged(nameof(Books));
                }
            }
        }
        private void AddBook()
        {
            RequestAddBookView?.Invoke(); 
        }

        private void RefreshBooks()
        {
            Books.Clear();
            var books = _bookService.GetAllBooks();
            foreach (var book in books)
            {
                Books.Add(book);
            }
        }       

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
