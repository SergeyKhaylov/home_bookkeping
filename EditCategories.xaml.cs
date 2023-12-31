﻿using System;
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
using Homebookkeping.Entities;

namespace Homebookkeping
{
    public partial class EditCategories : Window
    {
        int _userId;
        Category? _category;
        List<Category> _categories = new List<Category>();
        public EditCategories(int userID)
        {
            InitializeComponent();
            _userId = userID;


        }

        private void bApply_Click(object sender, RoutedEventArgs e)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Category category = new()
                {
                    type = cbType.Text,
                    category_name = tbCategory.Text,
                    user_id = _userId
                };
                if (category.category_name == "")
                {
                    MessageBox.Show("Введите название категории");
                }
                else if (db.categories.Where(c => c.user_id == _userId && c.category_name == category.category_name && c.type == category.type).ToList().Count > 0)
                {
                    MessageBox.Show("Такая категория уже существует");
                }
                else
                { 
                    db.categories.Add(category);
                    db.SaveChanges();
                    _categories = db.categories.Where(c => c.user_id == _userId).OrderBy(c => c.type).ToList();
                    dgCategories.ItemsSource = _categories;
                    tbCategory.Text = "";
                    MessageBox.Show("Категория добавлена");
                }
  
            }
        }

        private void BCancel_Click(object sender, RoutedEventArgs e)
        {
            gAddCategory.Visibility = Visibility.Collapsed;
            gShowCategory.Visibility = Visibility.Visible;
            gChangeCategory.Visibility = Visibility.Collapsed;
        }

        private void MiDelete_Click(object sender, RoutedEventArgs e)
        {
            Category? category = dgCategories.SelectedItem as Category;
            if (category != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    db.Attach(category);
                    db.Remove(category);
                    db.SaveChanges();
                    _categories = db.categories.Where(c => c.user_id == _userId).OrderBy(c => c.type).ToList();
                    dgCategories.ItemsSource = _categories;
                }
            }
        }

        private void MiAdd_Click(object sender, RoutedEventArgs e)
        {
            gAddCategory.Visibility = Visibility.Visible;
            gShowCategory.Visibility = Visibility.Collapsed;
            gChangeCategory.Visibility = Visibility.Collapsed;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbType.SelectedIndex = 0;
            using (ApplicationContext db = new ApplicationContext())
            {
                _categories = db.categories.Where(c => c.user_id == _userId).OrderBy(c => c.type).ToList();
            }
            dgCategories.ItemsSource = _categories;
        }

        private void bChangeApply_Click(object sender, RoutedEventArgs e)
        {
            if (tbChangeCategory.Text.Length > 0)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    _category.category_name = tbChangeCategory.Text;
                    db.Attach(_category);
                    db.Update(_category);
                    db.SaveChanges();
                    _categories = db.categories.Where(c => c.user_id == _userId).OrderBy(c => c.type).ToList();
                    dgCategories.ItemsSource = _categories;
                }
                gAddCategory.Visibility = Visibility.Collapsed;
                gShowCategory.Visibility = Visibility.Visible;
                gChangeCategory.Visibility = Visibility.Collapsed;
            }
        }

        private void MiUpdate_Click(object sender, RoutedEventArgs e)
        {
            _category = dgCategories.SelectedItem as Category;
            if (_category != null)
            {
                gAddCategory.Visibility = Visibility.Collapsed;
                gShowCategory.Visibility = Visibility.Collapsed;
                gChangeCategory.Visibility = Visibility.Visible;
                tbChangeCategory.Text = _category.category_name;
            }
        }
    }
}
