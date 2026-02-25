
using FinApp.DataBase;
using FinApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinApp.DataBase
{
     class FinanceProvider : IFinanceProvider
    {
        private readonly DbFinance _db;

        public FinanceProvider(DbFinance db)
        {
            _db = db;
        }

        public async Task AddWalletAsync(Wallet wallet)
        {
            _db.Wallets.Add(wallet);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteWalletAsync(Wallet wallet)
        {
            _db.Wallets.Remove(wallet);
            await _db.SaveChangesAsync();
           
        }
        public async Task<IEnumerable<Wallet>> GetAllWalletsAsync()
        {
            return await _db.Wallets
            .Include(w => w.Transactions).ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync(Wallet wallet)
        {
            return await _db.Transactions
                .Include(t => t.Category)
                .Where(c => c.WalletId == wallet.Id).ToListAsync();
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(Wallet wallet)
        {
            return await _db.Categories.Where(c => c.WalletId == wallet.Id).ToListAsync();
        }
        public async Task DeleteTransactionAsync(Transaction transaction)
        {
            _db.Transactions.Remove(transaction);
            await _db.SaveChangesAsync();
        }


        public async Task AddCategoryAsync(Category category)
        {
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
        }


        public async Task DeleteCategoryAsync(Category category)
        {
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
        }
    }
}
