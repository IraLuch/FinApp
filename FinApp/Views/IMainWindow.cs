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
        TypeOfOperation SelectedTypeOfOperation { get; set; }
        string Amount { get; set; }
        string Description { get; set; }
        DateTime Date { get; set; }

        public void ShowMessage(string message, string title = "Внимание!");

 
        void SetCategories(ObservableCollection<Category> cats);
        public void SetOperationTypes(List<EnumHelper.ValueDescription> types);
        void SetTransactions(ObservableCollection<Transaction> transactions);

        void ClearInputs();

    }
}
