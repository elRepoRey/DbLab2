using DbLab2.DataModels;
using DbLab2.DbBookContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DbLab2.Services
{
    public class BookService
    {
        private readonly BookstoreContext _context;

        public BookService()
        {
            _context = new BookstoreContext();
        }
        public List<Publisher> GetAllPublishers()
        {
            return _context.Publishers.ToList();
        }

        public List<Book> GetAllBooks()
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .ToList();
        }

        public List<Book> GetAllBooksWithAuthors()
        {
            return _context.Books.Include(b => b.Author).ToList();
        }

        public Book AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return book;
        }

        public void DeleteBook(Book book)
        {
            if (book != null)
            {
                var stockBalance = _context.StockBalance.Where(sb => sb.ISBN == book.ISBN13);
                _context.StockBalance.RemoveRange(stockBalance);

                var orderDetails = _context.OrderDetails.Where(od => od.ISBN == book.ISBN13);
                _context.OrderDetails.RemoveRange(orderDetails);

                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }

        public void UpdateBook(Book book)
        {
            var existingBook = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .SingleOrDefault(b => b.ISBN13 == book.ISBN13);

            if (existingBook != null)
            {
                _context.Entry(existingBook).CurrentValues.SetValues(book);

                if (book.PublisherID != existingBook.PublisherID)
                {
                    var newPublisher = _context.Publishers.Find(book.PublisherID);
                    _context.Entry(existingBook.Publisher).State = EntityState.Detached;
                    existingBook.Publisher = newPublisher;
                }

                if (book.AuthorID != existingBook.AuthorID)
                {
                    var newAuthor = _context.Authors.Find(book.AuthorID);
                    _context.Entry(existingBook.Author).State = EntityState.Detached;
                    existingBook.Author = newAuthor;
                }

                _context.SaveChanges();
            }
            else
            {
                _context.Books.Add(book);
                _context.SaveChanges();
            }
        }


    }
}
