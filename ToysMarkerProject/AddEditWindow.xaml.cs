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
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using ToysMarkerProject.Models;
using System.IO;

namespace ToysMarkerProject
{
    /// <summary>
    /// Логика взаимодействия для AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
        private readonly Product product;
        private string? selectedPhoto;

        public AddEditWindow(Product? currentProduct = null)
        {
            InitializeComponent();
            product = currentProduct ?? new Product();
            LoadComboBoxes();
            LoadProduct();
        }

        private void LoadComboBoxes()
        {
            var db = MebelMarketDbContext.GetContext();
            CategoryComboBox.ItemsSource = db.Categories.ToList();
            ManufacturerComboBox.ItemsSource = db.Manufacturers.ToList();
            ProviderComboBox.ItemsSource = db.Providers.ToList();
            UnitComboBox.ItemsSource = db.Units.ToList();
        }

        private void LoadProduct()
        {
            if (product.Id != 0)
            {
                IdTextBox.Text = product.Id.ToString();
                ArticleTextBox.Text = product.Article;
                NameTextBox.Text = product.Name;
                DescriptionTextBox.Text = product.Description;
                CostTextBox.Text = product.Cost.ToString();
                CountTextBox.Text = product.Count.ToString();
                DiscountTextBox.Text = product.Discount.ToString();
                CategoryComboBox.SelectedValue = product.CategoryId;
                ManufacturerComboBox.SelectedValue = product.ManufacturerId;
                ProviderComboBox.SelectedValue = product.ProviderId;
                UnitComboBox.SelectedValue = product.UnitId;
            }

            ProductImage.Source = new BitmapImage(new Uri(product.ImageSource ?? "/Resources/Image/picture.png", UriKind.RelativeOrAbsolute));
        }

        private void BtnPhoto_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Изображения|*.jpg;*.jpeg;*.png";

            if (dialog.ShowDialog() == true)
            {
                selectedPhoto = dialog.FileName;
                ProductImage.Source = new BitmapImage(new Uri(selectedPhoto));
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ArticleTextBox.Text) || string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Заполните артикул и наименование товара.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!short.TryParse(CostTextBox.Text, out short cost) || cost < 0)
            {
                MessageBox.Show("Цена должна быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!byte.TryParse(CountTextBox.Text, out byte count))
            {
                MessageBox.Show("Количество должно быть числом от 0 до 255.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!byte.TryParse(DiscountTextBox.Text, out byte discount) || discount > 100)
            {
                MessageBox.Show("Скидка должна быть числом от 0 до 100.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CategoryComboBox.SelectedValue == null || ManufacturerComboBox.SelectedValue == null ||
                ProviderComboBox.SelectedValue == null || UnitComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите категорию, производителя, поставщика и единицу измерения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            product.Article = ArticleTextBox.Text;
            product.Name = NameTextBox.Text;
            product.Description = DescriptionTextBox.Text;
            product.Cost = cost;
            product.Count = count;
            product.Discount = discount;
            product.CategoryId = (int)CategoryComboBox.SelectedValue;
            product.ManufacturerId = (int)ManufacturerComboBox.SelectedValue;
            product.ProviderId = (int)ProviderComboBox.SelectedValue;
            product.UnitId = (int)UnitComboBox.SelectedValue;

            if (selectedPhoto != null)
            {
                SavePhoto();
            }

            var db = MebelMarketDbContext.GetContext();
            if (product.Id == 0)
                db.Products.Add(product);

            db.SaveChanges();
            MessageBox.Show("Данные сохранены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void SavePhoto()
        {
            string folder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Image");
            Directory.CreateDirectory(folder);

            if (!string.IsNullOrWhiteSpace(product.Photo))
            {
                string oldPhoto = System.IO.Path.Combine(folder, product.Photo);
                if (File.Exists(oldPhoto))
                    File.Delete(oldPhoto);
            }

            string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(selectedPhoto);
            string newPhoto = System.IO.Path.Combine(folder, fileName);
            File.Copy(selectedPhoto!, newPhoto);
            product.Photo = fileName;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
