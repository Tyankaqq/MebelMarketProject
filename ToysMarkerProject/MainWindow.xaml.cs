using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToysMarkerProject.Models;

namespace ToysMarkerProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            User AuthUser = null!;
            using (MebelMarketDbContext dbContext = new MebelMarketDbContext())
            {
                AuthUser = dbContext.Users
                    .Include(p => p.Role)
                    .Where(u => u.Login == LoginTextBox.Text && u.Password == PasswordTextBox.Password)
                    .FirstOrDefault();
            }
            if (AuthUser != null)
            {
                Window window;
                if (AuthUser.Role.Name == "Менеджер")
                {
                    window = new ManagerWindow(AuthUser);
                }
                else if (AuthUser.Role.Name == "Администратор")
                {
                    window = new ProductsWindow(AuthUser);
                }
                else
                {
                    window = new AuthUserProductWindow(AuthUser);
                }
                window.Show();
                this.Close();


            }
        }

        private void BtnLoginGuest_Click(object sender, RoutedEventArgs e)
        {
            var window = new NonAuthUserProductWindow();
            window.Show();
            this.Close();
        }
    }
}