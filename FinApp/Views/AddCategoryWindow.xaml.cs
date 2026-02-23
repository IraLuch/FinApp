using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FinApp.Views
{
    /// <summary>
    /// Логика взаимодействия для AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window, IAddCategoryWindow
    {
        public AddCategoryWindow()
        {
            InitializeComponent();
        }

        public string Input {
            get => InputTextBox.Text;
            set => InputTextBox.Text = value;
        }


        public void CanselButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        public void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

        }
    }
}
