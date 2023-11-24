using DbLab2.DataModels;
using DbLab2.DbBookContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DbLab2.Services
{
    public class StoreService
    {
        private readonly BookstoreContext _context;

        public StoreService(BookstoreContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<StockBalance> GetStockBalances()
        {
            return _context.StockBalance
                .Include(sb => sb.Store)
                .Include(sb => sb.Book)
                .ToList();
        }
     
        public List<StockBalance> GetStockBalanceByStore(int storeId)
        {
            return _context.StockBalance
                .Where(sb => sb.StoreID == storeId)
                .Include(sb => sb.Book)
                .ToList();
        }

        public List<Store> GetStores()
        {
            return _context.Stores.ToList();
        }

        public void AddStockBalanceToStore(StockBalance stockBalance)
        {
            try
            {
                var existingStockBalance = _context.StockBalance
               .FirstOrDefault(sb => sb.StoreID == stockBalance.StoreID && sb.ISBN == stockBalance.ISBN);

                if (existingStockBalance != null)
                {
                    existingStockBalance.Quantity = stockBalance.Quantity;
                }
                else
                {
                    var store = _context.Stores.Find(stockBalance.StoreID);
                    if (store == null)
                    {
                        throw new InvalidOperationException("Store does not exist.");
                    }

                    var book = _context.Books.Find(stockBalance.ISBN);
                    if (book == null)
                    {
                        throw new InvalidOperationException("Book does not exist.");
                    }

                    stockBalance.Book = book;
                    stockBalance.Store = store;
                    _context.StockBalance.Add(stockBalance);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while adding stock balance to store.", ex);
            }
        }

        public void RemoveStockBalanceFromStore(StockBalance stockBalance, int quantityToRemove)
        {
            var existingStockBalance = _context.StockBalance
                .FirstOrDefault(sb => sb.StoreID == stockBalance.StoreID && sb.ISBN == stockBalance.ISBN);

            if (existingStockBalance != null)
            {                
                existingStockBalance.Quantity -= quantityToRemove;
                if (existingStockBalance.Quantity <= 0)
                {
                    _context.StockBalance.Remove(existingStockBalance);
                }

                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Stock balance entry does not exist for the provided ISBN in the specified store.");
            }
        }

    }

}



