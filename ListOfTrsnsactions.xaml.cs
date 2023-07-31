using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
        private void updateDG()
        {
            if (comboBox.SelectedIndex == 0)
            {
                dgOfTransactins.ItemsSource = GetTransactions();
            }
            else
            {
                dgOfTransactins.ItemsSource = GetTransactions().Where(p => p.adding_date > DateOnly.FromDateTime(DateTime.Now.AddDays(-DateTime.Now.Day)));
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateDG();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            comboBox.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Transaction? transaction = dgOfTransactins.SelectedItem as Transaction;
            if (transaction != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    db.Attach(transaction);
                    db.Remove(transaction);
                    db.SaveChanges();
                }
                updateDG();
            }
        }
    }
}
