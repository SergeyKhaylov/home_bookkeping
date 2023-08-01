using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Homebookkeping
{
    public partial class MainWindow : Window
    {
        public int? _userId = 2;
        bool _isLogined = false;
        public MainWindow()
        {
            InitializeComponent();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        private void ShowSum()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (_isLogined)
                {
                    double sum = 0;
                    string tr = "";
                    sum = db.transactions.Where(p => p.user_id == _userId && p.adding_date > DateOnly.FromDateTime(DateTime.Now.AddDays(-DateTime.Now.Day))).Sum(p => p.price);
                    if (sum != 0.0)
                    {
                        if (sum > 0)
                            tr = "Приход";
                        else if (sum < 0)
                            tr = "Расход";
                        tbShowSum.Text = $"{tr} за текущий месяц {sum}";
                    }
                    else
                    {
                        tbShowSum.Text = "Нет расходов";
                    }
                }
            }
        }
        private void ShowTransactions_Click(object sender, RoutedEventArgs e)
        {
            ListOfTrsnsactions listOfTransactions = new ListOfTrsnsactions(_userId);

            listOfTransactions.Owner = this;

            listOfTransactions.Show();
           
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddTransaction addTransaction = new AddTransaction(_userId);

            addTransaction.Owner = this;

            addTransaction.Show();
            addTransaction.datePicker.Text = DateOnly.FromDateTime(DateTime.Now).ToString();
            addTransaction.cbTypeTransactions.SelectedIndex = 0;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            ShowSum();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User user = new User()
                { user_name = tbUserName.Text, password = tbPass.Text };
                List<User> users = db.users.Where(u => u.user_name == user.user_name && u.password == user.password).ToList();

                if (tbUserName.Text.Length == 0)
                {
                    MessageBox.Show("Введите логин");
                }
                else if (tbPass.Text.Length == 0)
                {
                    MessageBox.Show("Введите пароль");
                }
                else if (users.Count == 0)
                {
                    MessageBox.Show("Не верный логин или пароль");
                }
                else
                {
                    user = users[0];
                    MessageBox.Show($"Вы успешно вошли {user.id}");
                    _userId = user.id;
                    _isLogined = true;
                    Login();
                }
            }
        }
        private void Login()
        {
            gMain.Visibility = Visibility.Visible;
            gRegist.Visibility = Visibility.Collapsed;
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User user = new User()
                { user_name = tbUserName.Text, password = tbPass.Text };
                List<User> users = db.users.Where(u => u.user_name == user.user_name).ToList();

                if (tbUserName.Text.Length == 0)
                {
                    MessageBox.Show("Введите логин");
                }
                else if (tbPass.Text.Length == 0)
                {
                    MessageBox.Show("Введите пароль");
                }
                else if (tbPass.Text.Length < 8)
                {
                    MessageBox.Show("Пароль должен быть длинее 8 символов");
                }
                else if (users.Count > 0)
                {
                    MessageBox.Show("Такой пользователь уже существует");
                }
                else
                {
                    db.users.Add(user);
                    db.SaveChanges();
                    users = db.users.Where(u => u.user_name == user.user_name).ToList();
                    user = users[0];
                    MessageBox.Show("Вы успешно зарегистрировались");
                    _userId = user.id;
                    _isLogined = true;
                    Login();
                }
            }

        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            _isLogined = false;
            gMain.Visibility = Visibility.Collapsed;
            gRegist.Visibility = Visibility.Visible;
        }
    }
}
