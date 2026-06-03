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
    /// Логика взаимодействия для ManagerOrdersWindow.xaml
    /// </summary>
    public partial class ManagerOrdersWindow : Window
    {
        private readonly User? _user;
        public ManagerOrdersWindow(User? user)
        {
            InitializeComponent();
            _user = user;
            var db = MebelMarketDbContext.GetContext().Orders
                .Include(p => p.Address)
                .Include(p => p.User)
                .ToList();
            DGridOrders.ItemsSource = db;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var window = new ManagerWindow(_user);
            window.Show();
            this.Close();
        }
    }
}
