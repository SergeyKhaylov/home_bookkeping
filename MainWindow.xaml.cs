using System;
using System.Linq;
using System.Windows;

namespace Homebookkeping
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        private void ShowTransactionsMonth_Click(object sender, RoutedEventArgs e)
        {
            ShowTransactionMonth(false);
        }

        private void ShowTransactions_Click(object sender, RoutedEventArgs e)
        {
            ShowTransactionMonth(true);
        }
        void ShowTransactionMonth (bool  showMonth)
        {
            ListOfTrsnsactions listOfTransactions = new ListOfTrsnsactions();

            listOfTransactions.Owner = this;

            listOfTransactions.Show();
            using (ApplicationContext db = new ApplicationContext())
            {
                var transaction = db.transactions.OrderBy(p => p.adding_date).ToList();
                int d = DateTime.Now.Day;
                DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
                dateOnly = dateOnly.AddDays(-d);
                foreach (Transaction u in transaction)
                {
                    if (u.adding_date > dateOnly || showMonth)
                        listOfTransactions.TextBoxOfTransactins.Text += ($"{u.adding_date} {u.type} {u.category} {u.price} {u.comment} \n");
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var transaction = db.transactions.OrderBy(p => p.adding_date).ToList();
                int d = DateTime.Now.Day;
                DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
                dateOnly = dateOnly.AddDays(-d);
                int sum = 0;
                string tr = "";
                foreach (Transaction u in transaction)
                {
                    if (u.adding_date > dateOnly)
                        sum += u.price;
                }
                if (sum != 0)
                    if(sum > 0)
                        tr = "Приход";
                    else if (sum < 0)
                        tr = "Расход";
                lShowSum.Text = $"{tr} за текущий месяц {sum}";
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddTransaction addTransaction = new AddTransaction();

            addTransaction.Owner = this;

            addTransaction.Show();
            addTransaction.datePicker.Text = DateOnly.FromDateTime(DateTime.Now).ToString();
        }
    }
}
