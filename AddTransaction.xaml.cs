using System;
using System.Windows;
using System.Windows.Controls;

namespace Homebookkeping
{
    public partial class AddTransaction : Window
    {
        public AddTransaction()
        {
            InitializeComponent();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    // создаем два объекта User
                    Transaction transaction = new Transaction
                    {
                        type = cbTypeTransactions.Text,
                        category = cbCategoryTransaction.Text,
                        price = Convert.ToInt32(tbPrice.Text),
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
            catch
            {
                tbPrice.Text = "";
                MessageBox.Show("Данные введены некорректно");
            }
        }

        private void cbTypeTransactions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbTypeTransactions.SelectedIndex == 0) 
            {
                cbCategoryTransaction.ItemsSource = new itemCategory[]
                {
                    new itemCategory{category= "Заработная плата" },
                    new itemCategory{category= "Дохода с сдачи в аренду недвижимости" },
                    new itemCategory{category= "Иные доходы" },

                };
            }
            else if (cbTypeTransactions .SelectedIndex == 1)
            {
                cbCategoryTransaction.ItemsSource = new itemCategory[]
               {
                   new itemCategory { category = "Продукты питания" },
                   new itemCategory { category = "Транспорт" },
                   new itemCategory { category = "Мобильная связь" },
                   new itemCategory { category = "Интернет" },
                   new itemCategory { category = "Развлечения" },
                   new itemCategory { category = "Другое" }
               };
            }
        }
        public class itemCategory
        {
            public string category { get; set; } = "";
            public override string ToString() => $"{category}";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbTypeTransactions.SelectedIndex = 0;
        }
    }
}
