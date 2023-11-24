using DbLab2.DataModels;
using DbLab2.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace DbLab2.ViewModels.BookModels
{
    public class AddBookViewModel
    {
        private readonly BookService _bookService;
        private readonly AuthorService _authorService;
        public event Action? BookAdded;

        private string _isbn13;
        public string ISBN13
        {
            get { return _isbn13; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _isbn13 = value;
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _title = value;
            }
        }

        private string _language;
        public string Language
        {
            get { return _language; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _language = value;
            }
        }

        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private DateTime? _publicationDate;
        public DateTime? PublicationDate
        {
            get { return _publicationDate; }
            set { _publicationDate = value; }
        }

        public ObservableCollection<Author> Authors { get; private set; }
        public ObservableCollection<Publisher> Publishers { get; private set; }

        public Author SelectedAuthor { get; set; }
        public Publisher SelectedPublisher { get; set; }

        public ICommand AddBookCommand { get; private set; }

        public AddBookViewModel(BookService bookService, AuthorService authorService)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));

            AddBookCommand = new RelayCommand(AddBook);

            Authors = new ObservableCollection<Author>(_authorService.GetAllAuthors());
            Publishers = new ObservableCollection<Publisher>(_bookService.GetAllPublishers());
        }

        private void AddBook()
        {   
           try
            {
                if (!string.IsNullOrEmpty(ISBN13) &&
               !string.IsNullOrEmpty(Title) &&
               !string.IsNullOrEmpty(Language) && Price > 0)
                {
                    if (ISBN13.Length != 13)
                    {
                        MessageBox.Show("ISBN13 must be 13 characters long");
                        return;
                    }

                    var newBook = new Book
                    {
                        ISBN13 = this.ISBN13,
                        Title = this.Title,
                        Language = this.Language,
                        Price = this.Price,
                        PublicationDate = this.PublicationDate,
                        AuthorID = SelectedAuthor?.ID ?? 0,
                        PublisherID = SelectedPublisher?.PublisherID ?? 0
                    };

                    _bookService.AddBook(newBook);
                    BookAdded?.Invoke();
                }

                else
                {
                    MessageBox.Show("Please fill all fields");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
