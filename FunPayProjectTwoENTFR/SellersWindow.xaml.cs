using System;
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
using System.Text.RegularExpressions;
using System.Data.Entity;

namespace FunPayProjectTwoENTFR
{
    public partial class SellersWindow : Window
    {
        private readonly LastChangeFourPrgFunPayEntities context = new LastChangeFourPrgFunPayEntities();

        public SellersWindow()
        {
            InitializeComponent();
            RefreshData();
        }

        private void RefreshData()
        {
            try
            {
                TableWindow.ItemsSource = context.Sellers.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void InsertSellerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsValidEmail(SellerEmailTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите корректный адрес электронной почты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int newSellerId = 1;
                while (context.Sellers.Any(s => s.SellerID == newSellerId))
                {
                    newSellerId++;
                }

                Sellers newSeller = new Sellers
                {
                    SellerID = newSellerId,
                    SellerFirstName = SellerFirstNameTextBox.Text,
                    SellerLastName = SellerLastNameTextBox.Text,
                    SellerEmail = SellerEmailTextBox.Text,
                    SellerPhone = SellerPhoneTextBox.Text
                };
                context.Sellers.Add(newSeller);
                context.SaveChanges();
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении продавца: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateSellerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Sellers selectedSeller = TableWindow.SelectedItem as Sellers;
                if (selectedSeller != null)
                {
                    selectedSeller.SellerFirstName = SellerFirstNameTextBox.Text;
                    selectedSeller.SellerLastName = SellerLastNameTextBox.Text;
                    selectedSeller.SellerEmail = SellerEmailTextBox.Text;
                    selectedSeller.SellerPhone = SellerPhoneTextBox.Text;
                    context.Entry(selectedSeller).State = EntityState.Modified;
                    context.SaveChanges();
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Выберите продавца для обновления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении продавца: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteSellerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Sellers selectedSeller = TableWindow.SelectedItem as Sellers;
                if (selectedSeller != null)
                {
                    context.Sellers.Remove(selectedSeller);
                    context.SaveChanges();
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Выберите продавца для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении продавца: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(email, pattern);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^[0-9-]*$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        private void TableWindow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TableWindow.SelectedItem is Sellers selectedSeller)
            {
                SellerFirstNameTextBox.Text = selectedSeller.SellerFirstName;
                SellerLastNameTextBox.Text = selectedSeller.SellerLastName;
                SellerEmailTextBox.Text = selectedSeller.SellerEmail;
                SellerPhoneTextBox.Text = selectedSeller.SellerPhone;
            }
            else
            {
                SellerFirstNameTextBox.Text = "";
                SellerLastNameTextBox.Text = "";
                SellerEmailTextBox.Text = "";
                SellerPhoneTextBox.Text = "";
            }
        }
    }
}
