using FinApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinApp.DataBase
{
    public interface IFinanceProvider
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync(Wallet wallet);
        Task<IEnumerable<Category>> GetAllCategoriesAsync(Wallet wallet);
        Task<IEnumerable<Wallet>> GetAllWalletsAsync();
        Task AddTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(Transaction transaction);

        Task AddCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);

        Task AddWalletAsync(Wallet wallet);
        Task DeleteWalletAsync(Wallet wallet);  


    }
}
