using FinApp.DataBase;
using FinApp.Models;
using FinApp.Presenter;
using FinApp.Views;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static FinApp.EnumHelper;

namespace FinApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {  
        public Category SelectedCategory
        {
            get => (Category)CategoryComboBox.SelectedItem;
            set => CategoryComboBox.SelectedItem = value;
        }


        public Wallet SelectedWallet
        {
            get => (Wallet)ComboBoxWallet.SelectedItem; 
            set => ComboBoxWallet.SelectedItem = value;

        }

        public TypeOfOperation SelectedTypeOfOperation
        {
            get => (TypeOfOperation)TypeComboBox.SelectedValue;
            set => TypeComboBox.SelectedItem = value;
        }

        public string Amount
        {
            get => AmountTextBox.Text;
            set => AmountTextBox.Text = value;
        }

        public string Description
        {
            get => DescriptionTextBox.Text;
            set => DescriptionTextBox.Text = value;
        }

        public DateTime Date
        {
            get => DatePicker.SelectedDate.Value;
            set => DatePicker.SelectedDate = value;
        }


        public string TotalBalance
        {
            get => TotalBalanceTextBlock.Text;
            set => TotalBalanceTextBlock.Text = value;
        }

        public string SelectedWalletBalance
        {
            get => SelectedWalletBalanceTexrBlock.Text;
            set => SelectedWalletBalanceTexrBlock.Text = value;
        }


        private readonly MainPresenter _presenter;

        public MainWindow()
        {
            InitializeComponent();
            _presenter = new MainPresenter(this, new FinanceProvider(new DbFinance()));
     
            Date = DateTime.Now;

        }

        public void ShowMessage(string message, string title = "Внимание!") => MessageBox.Show(message, title);


        public void ClearInputs()
        {
            AmountTextBox.Clear();
            DescriptionTextBox.Clear();
        }

        public async void DeleteCategoryButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            var cat = button?.Tag as Category;
            await _presenter.DeleteCategory(cat);

        }

        private async void AddCategoryButton_Click(Object sender, EventArgs e)
        {
            await _presenter.AddCategoryDialog();
        }

        public string? OpenCategoryDialog()
        {
            var dialog = new AddCategoryWindow
            {
                Owner = Application.Current.MainWindow //дочернее окно
            };
            return dialog.ShowDialog() == true
        ? dialog.Input
        : null;

        }

        public string? OpenWalletDialog()
        {
            var dialog = new AddWalletWindow
            {
                Owner = Application.Current.MainWindow //дочернее окно
            };
            return dialog.ShowDialog() == true
        ? dialog.Input
        : null;

        }
        private async void AddWallet_Click(object obj, EventArgs e)
        {
            await _presenter.AddWalletDialog();  
        }

        private async void DeleteWalletButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            var wl = button?.Tag as Wallet;
            await _presenter.DeleteWallet(wl);
        }

        public async void AddTransactionButton_Click(object sender, EventArgs e)
        {
            await _presenter.AddTransaction();
        }

        public async void DeleteTransaction_Click(object sender, EventArgs e)
        {
            var tr = (Transaction)DataGridTrs.SelectedItem;
            await _presenter.DeleteTransaction(tr);
        }

        public void SetOperationTypes(List<EnumHelper.ValueDescription> types)
        {
            TypeComboBox.ItemsSource = types;
            TypeComboBox.DisplayMemberPath = "Description";
            TypeComboBox.SelectedValuePath = "Value";

            if (TypeComboBox.HasItems)
                TypeComboBox.SelectedIndex = 0;
        }

        public void SetCategories(ObservableCollection<Category> cats)
        {
            CategoryComboBox.ItemsSource= cats;
            if (cats.Count > 0)
                CategoryComboBox.SelectedIndex = 0;
        }

         public void SetTransactions(ObservableCollection<Transaction> transactions)
        {
            DataGridTrs.ItemsSource = transactions;
        }

       public void SetWallets(ObservableCollection<Wallet> wallets)
        {
            ComboBoxWallet.ItemsSource = wallets;
            
        }

        private async void WalletSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (_presenter == null)
                return;
            await _presenter.LoadDataWithWallet(SelectedWallet);
        }

    

        private void DataGridTrs_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            
        }

    }
}