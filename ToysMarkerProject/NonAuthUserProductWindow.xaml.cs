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
    /// Логика взаимодействия для NonAuthUserProductWindow.xaml
    /// </summary>
    public partial class NonAuthUserProductWindow : Window
    {
        public NonAuthUserProductWindow()
        {
            InitializeComponent();
            var db = MebelMarketDbContext.GetContext().Products
               .Include(p => p.Category)
               .Include(p => p.Manufacturer)
               .Include(p => p.Provider)
               .Include(p => p.Unit)
               .ToList();
            ProductListView.ItemsSource = db;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
