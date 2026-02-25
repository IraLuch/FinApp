using FinApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static FinApp.EnumHelper;

namespace FinApp.Views
{
    interface IMainWindow
    {
        Category SelectedCategory { get; set; }
        Wallet SelectedWallet { get; set; }
        TypeOfOperation SelectedTypeOfOperation { get; set; }
        string Amount { get; set; }
        string Description { get; set; }
        DateTime Date { get; set; }

        string SelectedWalletBalance {  get; set; }

        string TotalBalance { get; set; }
        //void SetTotalBalence();

        //void SetSelectedWalletBalance();

        public void ShowMessage(string message, string title = "Внимание!");

 
        void SetCategories(ObservableCollection<Category> cats);
        void SetOperationTypes(List<EnumHelper.ValueDescription> types);
        void SetTransactions(ObservableCollection<Transaction> transactions);

        void SetWallets(ObservableCollection<Wallet> wallets);
        void ClearInputs();
        string? OpenCategoryDialog();
        string? OpenWalletDialog();

    }
}
