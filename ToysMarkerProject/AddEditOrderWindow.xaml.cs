using System;
using System.Windows;
using System.Windows.Controls;
using ToysMarkerProject.Models;

namespace ToysMarkerProject
{
    /// <summary>
    /// Логика взаимодействия для AddEditOrderWindow.xaml
    /// </summary>
    public partial class AddEditOrderWindow : Window
    {
        private readonly Order order;
        private readonly User? user;

        public AddEditOrderWindow(Order? currentOrder, User? currentUser)
        {
            InitializeComponent();
            order = currentOrder ?? new Order();
            user = currentUser;
            LoadComboBoxes();
            LoadOrder();
        }

        private void LoadComboBoxes()
        {
            AddressComboBox.ItemsSource = MebelMarketDbContext.GetContext().Pvzs.ToList();
        }

        private void LoadOrder()
        {
            if (order.Id != 0)
            {
                IdTextBox.Text = order.Id.ToString();
                ArticleTextBox.Text = order.Article;
                StatusComboBox.Text = order.Status;
                AddressComboBox.SelectedValue = order.AddressId;
                DateOrderPicker.SelectedDate = order.DateOrder.ToDateTime(TimeOnly.MinValue);
                DateDeliveryPicker.SelectedDate = order.DateDelivery.ToDateTime(TimeOnly.MinValue);
                CodeTextBox.Text = order.Code.ToString();
            }
            else
            {
                DateOrderPicker.SelectedDate = DateTime.Today;
                DateDeliveryPicker.SelectedDate = DateTime.Today;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ArticleTextBox.Text))
            {
                MessageBox.Show("Введите артикул товара.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(StatusComboBox.Text))
            {
                MessageBox.Show("Введите статус заказа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (AddressComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите адрес пункта выдачи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DateOrderPicker.SelectedDate == null || DateDeliveryPicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите даты заказа и выдачи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!short.TryParse(CodeTextBox.Text, out short code))
            {
                MessageBox.Show("Код должен быть числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            order.Article = ArticleTextBox.Text;
            order.Status = StatusComboBox.Text;
            order.AddressId = (int)AddressComboBox.SelectedValue;
            order.DateOrder = DateOnly.FromDateTime(DateOrderPicker.SelectedDate.Value);
            order.DateDelivery = DateOnly.FromDateTime(DateDeliveryPicker.SelectedDate.Value);
            order.Code = code;

            if (order.UserId == 0 && user != null)
                order.UserId = user.Id;

            var db = MebelMarketDbContext.GetContext();
            if (order.Id == 0)
                db.Orders.Add(order);

            db.SaveChanges();
            MessageBox.Show("Данные сохранены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
