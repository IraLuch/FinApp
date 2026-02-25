using FinApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fin.DataBase
{
    public interface IFinanceProvider
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task AddTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(Transaction transaction);

        Task AddCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);
    }
}
