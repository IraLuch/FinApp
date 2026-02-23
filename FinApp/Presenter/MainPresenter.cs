
using Fin.DataBase;
using FinApp.DataBase;
using FinApp.Models;
using FinApp.Views;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FinApp.Presenter
{

    class MainPresenter
    {
        private ObservableCollection<Category> categories = new();
        private ObservableCollection<Transaction> transactions = new();
        private readonly IFinanceProvider _provider;
        private readonly IMainWindow _view;

        public MainPresenter(IMainWindow view, IFinanceProvider provider)
        {
            _provider = provider;
            _view = view;
            LoadData();
        }


  

        private async Task LoadData()
        {

            var cats = await _provider.GetAllCategoriesAsync();
            foreach (var c in cats)
                categories.Add(c);

            var trs = await _provider.GetAllTransactionsAsync();
            foreach (var t in trs)
                transactions.Add(t);

            _view.SetCategories(categories);
            _view.SetTransactions(transactions);
            _view.SetOperationTypes( EnumHelper.GetAllValuesAndDescriptions(typeof(TypeOfOperation)));

        }

        public async Task AddTransaction()
        {
           
            if (!decimal.TryParse(_view.Amount, out decimal amount) || amount <= 0)
            {
                _view.ShowMessage("Некорректная сумма!");
                return;
            }

            if (_view.SelectedCategory == null)
            {
                _view.ShowMessage("Категория не выбрана!");
                return;
            }

            var tr = new Transaction
            {
                CategoryId = _view.SelectedCategory.Id,
                Amount = amount,
                TypeOfOperation = _view.SelectedTypeOfOperation,
                Date = _view.Date,
                Description = _view.Description
            };

            await _provider.AddTransactionAsync(tr);
            transactions.Add(tr);
            _view.ClearInputs();
            
        }

        public async Task DeleteTransaction(Transaction transaction)
        {
            if (transaction == null) return;

            await _provider.DeleteTransactionAsync(transaction);
            transactions.Remove(transaction);
        }

        public async Task AddCategory(Category category)
        {
            await _provider.AddCategoryAsync(category);
            categories.Add(category);
        }

        public async Task DeleteCategory(Category category)
        {

            if (transactions.Any(t => t.Category.Id == category.Id))
            {
                _view.ShowMessage("Категория используется в записях! \n Для удаления удалите связанные записи.");
                return;
            }

            if (categories.Count > 0)
            {
                _view.SelectedCategory = categories.First();
            }
            await _provider.DeleteCategoryAsync(category);
            categories.Remove(category);
        }

        public async Task AddCategoryDialog()
        {
            var dialog = new AddCategoryWindow
            {
                Owner = Application.Current.MainWindow //дочернее окно
            };

            if (dialog.ShowDialog() == true)
            {
                string categoryName = dialog.Input;

                if (string.IsNullOrWhiteSpace(categoryName))
                {
                   _view.ShowMessage("Название категории не может быть пустым!");
                    return;
                }
                var cats = await _provider.GetAllCategoriesAsync();
                if (cats.Any(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase)))
                {
                    _view.ShowMessage("Такая категория уже существует!");
                    return;
                }

                var category = new Category { Name = categoryName };
                await _provider.AddCategoryAsync(category);
                categories.Add(category);
                _view.SelectedCategory = category;
                dialog.Close();
            }
        }
    }
}
