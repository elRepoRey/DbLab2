using DbLab2.DataModels;
using DbLab2.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DbLab2.ViewModels.BookModels
{
    public class EditBookViewModel
    {
        public event Action BookUpdated;
        private readonly BookService _bookService;
        private readonly AuthorService _authorService;
        private Book _book;

     
        public string Title { get; set; }
        public string Language { get; set; }
        public decimal Price { get; set; }
        public DateTime? PublicationDate { get; set; }
        public int AuthorID { get; set; }
        public int PublisherID { get; set; }
       
        public ObservableCollection<Author> Authors { get; private set; }
        public ObservableCollection<Publisher> Publishers { get; private set; }

        public Author SelectedAuthor { get; set; }
        public Publisher SelectedPublisher { get; set; }

        public ICommand SaveCommand { get; private set; }

        public EditBookViewModel(BookService bookService, AuthorService authorService, Book book)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
            _book = book ?? throw new ArgumentNullException(nameof(book));                    
            Title = _book.Title;
            Language = _book.Language;
            Price = _book.Price;
            PublicationDate = _book.PublicationDate;
            AuthorID = _book.AuthorID;
            PublisherID = _book.PublisherID;
            SaveCommand = new RelayCommand(SaveBookChanges);
            LoadAuthorsAndPublishers();
        }

        private void LoadAuthorsAndPublishers()
        {
            Authors = new ObservableCollection<Author>(_authorService.GetAllAuthors());
            Publishers = new ObservableCollection<Publisher>(_bookService.GetAllPublishers());
            
            SelectedAuthor = Authors.FirstOrDefault(a => a.ID == AuthorID);
            SelectedPublisher = Publishers.FirstOrDefault(p => p.PublisherID == PublisherID);
        }

        private void SaveBookChanges()
        {
           try
            {             

                if ( !string.IsNullOrEmpty(Title) &&
                    !string.IsNullOrEmpty(Language) && Price > 0)
                {
                    
                    _book.Title = Title;
                    _book.Language = Language;
                    _book.Price = Price;
                    _book.PublicationDate = PublicationDate;
                    _book.AuthorID = SelectedAuthor?.ID ?? 0;
                    _book.PublisherID = SelectedPublisher?.PublisherID ?? 0;

                    _bookService.UpdateBook(_book);
                    BookUpdated?.Invoke();
                }
                else
                {
                    MessageBox.Show("Please fill all fields correctly");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);         
                return;
            }
        }

    }
}
