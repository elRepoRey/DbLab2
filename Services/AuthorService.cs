using DbLab2.DataModels;
using DbLab2.DbBookContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DbLab2.Services
{
    public class AuthorService
    {
        private readonly BookstoreContext _context;
        private readonly BookService _bookService;

        public AuthorService()
        {
            _bookService = new BookService();
            _context = new BookstoreContext();
        }

        public List<Author> GetAllAuthors()
        {
            return _context.Authors.ToList();
        }

        public Author AddAuthor(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();
            return author;
        }

        public bool DeleteAuthor(Author author)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this author?", "Delete Author", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                if (author != null && !_bookService.GetAllBooksWithAuthors().Where(b => b.AuthorID == author.ID).Any())
                {
                    _context.Authors.Remove(author);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Author has associated books. Delete all books associated with this author?", "Delete Author", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        if (author != null)
                        {
                            var books = _bookService.GetAllBooksWithAuthors().Where(b => b.AuthorID == author.ID);
                            foreach (var book in books)
                            {
                                _bookService.DeleteBook(book);
                            }
                            _context.Authors.Remove(author);
                            _context.SaveChanges();
                            return true;
                        }
                    }

                }
                return false;
            }
            return false;
          
        }

        public void UpdateAuthor(Author author)
        {
            _context.Authors.Update(author);
            _context.SaveChanges();
        }
    }
}
