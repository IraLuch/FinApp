using FinApp.DataBase;
using FinApp.Models;
using FinApp.Presenter;
using FinApp.Views;
using System.Collections.ObjectModel;
using System.Text;
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

        public async void AddCategoryButton_Click(Object sender, EventArgs e)
        {
            await _presenter.AddCategoryDialog();
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

    
        private void DataGridTrs_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            
        }

    }
}