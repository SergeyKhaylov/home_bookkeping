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

namespace Homebookkeping
{
    /// <summary>
    /// Логика взаимодействия для ListOfTrsnsactions.xaml
    /// </summary>
    public partial class ListOfTrsnsactions : Window
    {
        
        public ListOfTrsnsactions()
        {
            InitializeComponent();
        }
        private List<Transaction> GetTransactions()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.transactions.ToList();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedIndex == 0)
            {
                    dgOfTransactins.ItemsSource = GetTransactions();
            }
            else
            {
                dgOfTransactins.ItemsSource = GetTransactions().Where(p => p.adding_date > DateOnly.FromDateTime(DateTime.Now.AddDays(- DateTime.Now.Day)));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            comboBox.SelectedIndex = 0;
        }
    }
}
