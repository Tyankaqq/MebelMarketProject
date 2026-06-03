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
    /// Логика взаимодействия для AuthUserProductWindow.xaml
    /// </summary>
    public partial class AuthUserProductWindow : Window
    {
        private readonly User? _user;
        public AuthUserProductWindow(User? user)
        {
            InitializeComponent();
            _user = user;
            RoleNameBox.Text = user.Role.Name;
            FullNameBox.Text = user.FullName;

            var db = MebelMarketDbContext.GetContext().Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.Provider)
                .ToList();
            ProductListView.ItemsSource = db;
        }
    }
}
