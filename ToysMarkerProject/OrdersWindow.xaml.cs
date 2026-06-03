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
/*using ToysMarkerProject.Models;*/

namespace ToysMarkerProject
{
    /// <summary>
    /// Логика взаимодействия для OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        private readonly User? _user;
        public OrdersWindow(User? user)
        {
            InitializeComponent();
            _user = user;
            RefreshOrders();
        }

        private void RefreshOrders()
        {
            var db = MebelMarketDbContext.GetContext().Orders
                .Include(p => p.Address)
                .Include(p => p.User)
                .ToList();
            DGridOrders.ItemsSource = db;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddEditOrderWindow(null, _user);
            window.ShowDialog();
            RefreshOrders();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is Order order)
            {
                var window = new AddEditOrderWindow(order, _user);
                window.ShowDialog();
                RefreshOrders();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DGridOrders.SelectedItem is not Order order)
            {
                MessageBox.Show("Выберите заказ для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Удалить выбранный заказ?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var db = MebelMarketDbContext.GetContext();
                db.Orders.Remove(order);
                db.SaveChanges();
                RefreshOrders();
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var window = new ProductsWindow(_user);
            window.Show();
            this.Close();
        }
    }
}
