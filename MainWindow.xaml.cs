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
        private void ShowSum()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var transaction = db.transactions.ToList();
                DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now.AddDays(-DateTime.Now.Day));
                double sum = 0;
                string tr = "";
                foreach (Transaction u in transaction)
                {
                    if (u.adding_date > dateOnly)
                        sum += u.price;
                }
                if (sum != 0)
                {
                    if (sum > 0)
                        tr = "Приход";
                    else if (sum < 0)
                        tr = "Расход";
                    lShowSum.Text = $"{tr} за текущий месяц {sum}";
                }
            }
        }

        private void ShowTransactions_Click(object sender, RoutedEventArgs e)
        {
            ListOfTrsnsactions listOfTransactions = new ListOfTrsnsactions();

            listOfTransactions.Owner = this;

            listOfTransactions.Show();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddTransaction addTransaction = new AddTransaction();

            addTransaction.Owner = this;

            addTransaction.Show();
            addTransaction.datePicker.Text = DateOnly.FromDateTime(DateTime.Now).ToString();
            addTransaction.cbTypeTransactions.SelectedIndex = 0;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            ShowSum();
        }
    }
}
