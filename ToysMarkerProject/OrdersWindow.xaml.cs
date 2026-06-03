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
            var db = MebelMarketDbContext.GetContext().Orders
                .Include(p => p.Address)
                .Include(p => p.User)
                .ToList();
            DGridOrders.ItemsSource = db;
        }
    }
}
