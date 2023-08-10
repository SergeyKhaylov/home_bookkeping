using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Homebookkeping.Entities;

namespace Homebookkeping
{
    public partial class ListOfTrsnsactions : Window 
    {
        private int? _userId;
        public ListOfTrsnsactions(int? userId)
        {
            InitializeComponent();
            _userId = userId;

        }

        private void updateDG()
        {
            dpEnd.DisplayDateStart = dpStart.SelectedDate;
            dpStart.DisplayDateEnd = dpEnd.SelectedDate;
            
            DateOnly dStart = new DateOnly();
            DateOnly dEnd = new DateOnly();
            dStart = DateOnly.FromDateTime(Convert.ToDateTime(dpStart.SelectedDate));
            dEnd = DateOnly.FromDateTime(Convert.ToDateTime(dpEnd.SelectedDate));
            List<DGTransaction> dgTransactions = new List<DGTransaction>();
            using (ApplicationContext db = new ApplicationContext())
            {

                foreach (var t in db.transactions.Where(p => p.user_id == _userId && p.adding_date >= dStart && p.adding_date <= dEnd).ToList())
                {
                    dgTransactions.Add(new DGTransaction
                    {
                        id = t.id,
                        adding_date = t.adding_date.ToString("dd/MM/yyyy"),
                        type = t.type,
                        category = db.categories.Where(c => c.id == t.category_id).ToList()[0].category_name,
                        price = t.price,
                        comment = t.comment
                    });
                }
            }

            dgOfTransactins.ItemsSource = dgTransactions;

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpEnd.SelectedDate = DateTime.Now;
            dpStart.SelectedDate = DateTime.Now.AddMonths(-1);
            dpEnd.DisplayDateEnd = DateTime.Now;
            updateDG();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DGTransaction? dgTransaction = dgOfTransactins.SelectedItem as DGTransaction;
            if (dgTransaction != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {

                    db.Remove(db.transactions.Where(t => t.id == dgTransaction.id).ToList()[0]);
                    db.SaveChanges();
                }
                updateDG();
            }
        }

        private void dpStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            updateDG();
        }
    }
}
