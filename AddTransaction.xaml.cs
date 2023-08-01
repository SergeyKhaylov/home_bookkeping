using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Homebookkeping
{
    public partial class AddTransaction : Window
    {
        int? _userId;
        public AddTransaction(int? userId)
        {
            InitializeComponent();
            _userId = userId;
        }
        List<string> incomeCategory = new List<string>()
        {
            "Заработная плата",
            "Доход со сдачи в аренду недвижимости",
            "Иные доходы"
        };
        List<string> expenseCategory = new List<string>()
        {
            "Продукты питания",
            "Транспорт",
            "Мобильная связь",
            "Интернет",
            "Развлечения",
            "Другое"
        };

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if(double.TryParse(tbPrice.Text, out double priceValue))
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Transaction transaction = new Transaction
                    {
                        type = cbTypeTransactions.Text,
                        category = cbCategoryTransaction.Text,
                        price = priceValue,
                        comment = tbComment.Text,
                        adding_date = DateOnly.Parse(datePicker.Text)
                    };
                    transaction.price = Math.Abs(transaction.price);
                    if (cbTypeTransactions.SelectedIndex == 1)
                        transaction.price *= -1;

                    // добавляем их в бд
                    db.transactions.Add(transaction);
                    db.SaveChanges();
                }

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
                cbCategoryTransaction.ItemsSource = incomeCategory;
            }
            else if (cbTypeTransactions .SelectedIndex == 1)
            {
                cbCategoryTransaction.ItemsSource = expenseCategory;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
