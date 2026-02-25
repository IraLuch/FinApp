
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
        private ObservableCollection<Wallet> wallets = new();
        private readonly IFinanceProvider _provider;
        private readonly IMainWindow _view;

        public string TotalBalance => wallets.Sum(w => w.Transactions.Sum(
    t => t.TypeOfOperation == TypeOfOperation.Income ? t.Amount : -t.Amount)).ToString();

        public string SelectedWalletBalance =>
            _view.SelectedWallet == null ? "0" :
            _view.SelectedWallet.Transactions.Sum(
                t => t.TypeOfOperation == TypeOfOperation.Income ? t.Amount : -t.Amount).ToString();

        public MainPresenter(IMainWindow view, IFinanceProvider provider)
        {
            _provider = provider;
            _view = view;
            _= LoadData();
        }


  

        public async Task LoadData()
        {
            _view.SetOperationTypes( EnumHelper.GetAllValuesAndDescriptions(typeof(TypeOfOperation)));
            var wls = await _provider.GetAllWalletsAsync();
            foreach (var w in wls)
                wallets.Add(w);
            _view.SetWallets(wallets);
            if (wallets.Count > 0)
            {
                _view.SelectedWallet = wallets.First();  // обязательно установить SelectedWallet
                await LoadDataWithWallet(_view.SelectedWallet);
            }

        }

        public async Task LoadDataWithWallet(Wallet wallet)
        {
            categories.Clear();
            transactions.Clear();
            if (wallet == null)
                return;
            var cats = await _provider.GetAllCategoriesAsync(wallet);
            foreach (var c in cats)
                categories.Add(c);

            var trs = await _provider.GetAllTransactionsAsync(wallet);
            foreach (var t in trs)
                transactions.Add(t);

            _view.SetCategories(categories);
            _view.SetTransactions(transactions);
            _view.TotalBalance = TotalBalance;
            _view.SelectedWalletBalance = SelectedWalletBalance;
        }

        public async Task AddTransaction()
        {
           
            if (_view.SelectedWallet == null)
            {
                _view.ShowMessage("Выбирите или создайте кошелек!");
                return;
            }

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
                Description = _view.Description,
                WalletId = _view.SelectedWallet.Id
            };

            await _provider.AddTransactionAsync(tr);
            transactions.Add(tr);
            _view.ClearInputs();
            _view.TotalBalance = TotalBalance;
            _view.SelectedWalletBalance = SelectedWalletBalance;
        }

        public async Task DeleteTransaction(Transaction transaction)
        {
            if (transaction == null) return;

            await _provider.DeleteTransactionAsync(transaction);
            transactions.Remove(transaction);
            _view.TotalBalance = TotalBalance;
            _view.SelectedWalletBalance = SelectedWalletBalance;
        }

        public async Task DeleteWallet(Wallet wallet)
        {
            if (wallets.Any(w => w.Transactions.Any(t => t.WalletId == wallet.Id)))
            {
                _view.ShowMessage("В данном кошельке находятся записи! \n Для удаления удалите все записи");
                return ;
            }
            await _provider.DeleteWalletAsync(wallet);
            wallets.Remove(wallet);
            if (wallets.Count > 0)
            {
                _view.SelectedWallet = wallets.First();
            }
            _view.TotalBalance = TotalBalance;
            _view.SelectedWalletBalance = SelectedWalletBalance;

        }



        public async Task AddCategory(Category category)
        {
            await _provider.AddCategoryAsync(category);
            categories.Add(category);
        }

        public async Task DeleteCategory(Category category)
        {

            if (transactions.Any(t => t.CategoryId == category.Id))
            {
                _view.ShowMessage("Категория используется в записях! \n Для удаления удалите связанные записи.");
                return;
            }
            await _provider.DeleteCategoryAsync(category);
            categories.Remove(category);

            if (categories.Count > 0)
            {
                _view.SelectedCategory = categories.First();
            }
        }

        public async Task AddCategoryDialog()
        {
            if (_view.SelectedWallet == null)
            {
                _view.ShowMessage("Выбирите или создайте кошелек!");
                return;
            }
            string? categoryName = _view.OpenCategoryDialog();
            if (categoryName == null)
                return;

            if (string.IsNullOrWhiteSpace(categoryName))
                {
                   _view.ShowMessage("Название категории не может быть пустым!");
                    return;
                }
                var cats = await _provider.GetAllCategoriesAsync(_view.SelectedWallet);
                if (cats.Any(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase)))
                {
                    _view.ShowMessage("Такая категория уже существует!");
                    return;
                }

                var category = new Category { Name = categoryName, WalletId = _view.SelectedWallet.Id };
                await _provider.AddCategoryAsync(category);
                categories.Add(category);
                _view.SelectedCategory = category;
             
            
        }
        public async Task AddWalletDialog()
        {
           
            string? walletName = _view.OpenWalletDialog();
            if (walletName == null)
                return;

            if (string.IsNullOrWhiteSpace(walletName))
            {
                _view.ShowMessage("Название кошелька не может быть пустым!");
                return;
            }
            var wls = await _provider.GetAllWalletsAsync();
            if (wls.Any(c => c.Name.Equals(walletName, StringComparison.OrdinalIgnoreCase)))
            {
                _view.ShowMessage("Такой кошелек уже существует!");
                return;
            }

            var wallet = new Wallet { Name = walletName };
            await _provider.AddWalletAsync(wallet);
            wallets.Add(wallet);
            _view.SelectedWallet = wallet;


        }

   
    }
}
