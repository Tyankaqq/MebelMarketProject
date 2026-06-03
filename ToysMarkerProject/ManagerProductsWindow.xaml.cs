using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToysMarkerProject.Models;

namespace ToysMarkerProject
{
    /// <summary>
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        private readonly User? user;
        private List<Product> products = new List<Product>();
        public ManagerWindow(User? _currentuser)
        {
            InitializeComponent();
            user = _currentuser;
            RoleNameBox.Text = user.Role.Name;
            FullNameBox.Text = user.FullName;

            SortComboBox.SelectedIndex = 0;
            DiscountComboBox.SelectedIndex = 0;
            RefreshProducts();
        }

        private void RefreshProducts()
        {
            products = MebelMarketDbContext.GetContext().Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.Provider)
                .Include(p => p.Unit)
                .ToList();
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (ProductListView == null) return;

            var result = products.AsEnumerable();
            string search = SearchTextBox.Text.ToLower();

            if (!string.IsNullOrWhiteSpace(search))
            {
                result = result.Where(p =>
                    p.Name.ToLower().Contains(search) ||
                    p.Article.ToLower().Contains(search) ||
                    (p.Description != null && p.Description.ToLower().Contains(search)) ||
                    p.Category.Name.ToLower().Contains(search) ||
                    p.Manufacturer.Name.ToLower().Contains(search) ||
                    p.Provider.Name.ToLower().Contains(search) ||
                    p.Unit.Name.ToLower().Contains(search));
            }

            if (DiscountComboBox.SelectedIndex == 1)
                result = result.Where(p => p.Discount >= 0 && p.Discount < 11);
            else if (DiscountComboBox.SelectedIndex == 2)
                result = result.Where(p => p.Discount >= 11 && p.Discount < 15);
            else if (DiscountComboBox.SelectedIndex == 3)
                result = result.Where(p => p.Discount >= 15);

            if (SortComboBox.SelectedIndex == 1)
                result = result.OrderBy(p => p.Cost);
            else if (SortComboBox.SelectedIndex == 2)
                result = result.OrderByDescending(p => p.Cost);
            else if (SortComboBox.SelectedIndex == 3)
                result = result.OrderBy(p => p.Count);
            else if (SortComboBox.SelectedIndex == 4)
                result = result.OrderByDescending(p => p.Count);

            ProductListView.ItemsSource = result.ToList();
        }
        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            var window = new ManagerOrdersWindow(user);
            window.Show();
            this.Close();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void DiscountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
