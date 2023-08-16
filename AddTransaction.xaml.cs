using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Homebookkeping
{
    public partial class AddTransaction : Window
    {
        int _userId;
        List<string> _incomeCategory = new List<string>();
        List<string> _expenseCategory = new List<string>();

        public AddTransaction(int userId)
        {
            InitializeComponent();
            _userId = userId;

        }
        private void Window_Activated(object sender, EventArgs e)
        {
            updateCategory();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbTypeTransactions.SelectedIndex = 0;
            datePicker.Text = DateOnly.FromDateTime(DateTime.Now).ToString();
        }

        void updateCategory()
        {
            using ApplicationContext db = new();
            _incomeCategory.Clear();
            _expenseCategory.Clear();
            foreach (var c in db.categories.Where(c => c.user_id == _userId).ToList())
                if (c.type == "Приход")
                {
                    _incomeCategory.Add(c.category_name);
                }
                else
                {
                    _expenseCategory.Add(c.category_name);
                }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if(double.TryParse(tbPrice.Text, out double priceValue))
            {
                using ApplicationContext db = new();
                Transaction transaction = new()
                {
                    category_id = db.categories.Where(c => c.user_id == _userId && c.category_name == cbCategoryTransaction.Text).ToList()[0].id,
                    price = priceValue,
                    comment = tbComment.Text,
                    adding_date = DateOnly.Parse(datePicker.Text),
                    user_id = _userId
                };
                transaction.price = Math.Abs(transaction.price);
                if (cbTypeTransactions.SelectedIndex == 1)
                    transaction.price *= -1;

                db.transactions.Add(transaction);
                db.SaveChanges();
                

                MessageBox.Show("Данные успешно введены");
                Close();
            }
            else
            {
                tbPrice.Text = "";
                MessageBox.Show("Данные введены некорректно");
            }
        }

        private void cbTypeTransactions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbTypeTransactions.SelectedIndex == 0) 
            {
                cbCategoryTransaction.ItemsSource = _incomeCategory;
            }
            else if (cbTypeTransactions .SelectedIndex == 1)
            {
                cbCategoryTransaction.ItemsSource = _expenseCategory;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void bEditCategory_Click(object sender, RoutedEventArgs e)
        {
            EditCategories editCategories = new EditCategories(_userId);
            editCategories.Owner = this;
            editCategories.Show();
        }
    }
}
