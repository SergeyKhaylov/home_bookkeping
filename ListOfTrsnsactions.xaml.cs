using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Homebookkeping.Entities;

namespace Homebookkeping
{
    public partial class ListOfTrsnsactions : Window 
    {
        private readonly int? _userId;
        public ListOfTrsnsactions(int? userId)
        {
            InitializeComponent();
            _userId = userId;

        }
        private readonly List<DGTransaction> dgTransactions = new();
        private readonly ObservableCollection<CategoryData> categoryDataList = new();
        private List<Category> categories = new();
      
        private void TransactionRange()
        {
            dpEnd.DisplayDateStart = dpStart.SelectedDate;
            dpStart.DisplayDateEnd = dpEnd.SelectedDate;
            
            DateOnly dStart = new();
            DateOnly dEnd = new();
            dStart = DateOnly.FromDateTime(Convert.ToDateTime(dpStart.SelectedDate));
            dEnd = DateOnly.FromDateTime(Convert.ToDateTime(dpEnd.SelectedDate));
            DgOfTransactins.ItemsSource = dgTransactions.Where(t => t.adding_date >= dStart && t.adding_date <= dEnd).ToList();
        }
        private void TransactionYear(int year)
        {
            categoryDataList.Clear();
            using ApplicationContext db = new();
            for (int i = 0; i < 12; i++)
            {
                categoryDataList.Add(new CategoryData
                {
                    Month = new DateTime(2022, i+1, 1).ToString("MMMM"),
                    CategoryAmounts = new Dictionary<string, string?>()
                });
                foreach (var c in categories)
                {
                    if (c.category_name != null)
                    {
                        string? s = dgTransactions.Where(d => d.adding_date.Year == year && d.adding_date.Month == i+1 && d.category_id == c.id).Sum(d => d.price).ToString();
                        s = s == "0" ? null : s;
                        categoryDataList[i].CategoryAmounts.Add(c.category_name, s);
                    }
                }
            }
            dgOfMonth.ItemsSource = categoryDataList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpEnd.SelectedDate = DateTime.Now;
            dpStart.SelectedDate = DateTime.Now.AddMonths(-1);
            dpEnd.DisplayDateEnd = DateTime.Now;
            using ApplicationContext db = new();
            categories = db.categories.Where(c => c.user_id == _userId).ToList();
            foreach (var t in db.transactions.Where(p => p.user_id == _userId).ToList())
            {
                dgTransactions.Add(new DGTransaction
                {
                    id = t.id,
                    adding_date = t.adding_date,
                    type = db.categories.Where(c => c.id == t.category_id).ToList()[0].type,
                    category = db.categories.Where(c => c.id == t.category_id).ToList()[0].category_name,
                    category_id = t.category_id,
                    price = t.price,
                    comment = t.comment
                });
            }
            TransactionYear(DateTime.Now.Year);
            foreach (var category in categoryDataList.First().CategoryAmounts.Keys)
            {
                var binding = new Binding($"CategoryAmounts[{category}]");
                var column = new DataGridTextColumn
                {
                    Header = category,
                    Binding = binding
                };
                dgOfMonth.Columns.Add(column);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DgOfTransactins.SelectedItem is DGTransaction dgTransaction)
            {
                using ApplicationContext db = new();
                db.Remove(db.transactions.Where(t => t.id == dgTransaction.id).ToList()[0]);
                db.SaveChanges();
                TransactionRange();
            }
        }

        private void Dp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TransactionRange();
        }

        private void MSortYear_Click(object sender, RoutedEventArgs e)
        {
            GRange.Visibility = Visibility.Collapsed;
            GYear.Visibility = Visibility.Visible;
            if (int.TryParse(TbYear.Text, out int year) && year > 0)
            {
                TransactionYear(year);
            }
            
        }

        private void MSortRange_Click(object sender, RoutedEventArgs e)
        {
            GRange.Visibility = Visibility.Visible;
            GYear.Visibility = Visibility.Collapsed;
            TransactionRange();
        }

        private void TbYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(TbYear.Text, out int year) && year > 0)
            {
                TransactionYear(year);
            }
        }
    }
}
