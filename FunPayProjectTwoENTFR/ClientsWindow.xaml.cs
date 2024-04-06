using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FunPayProjectTwoENTFR
{
    public partial class ClientsWindow : Window
    {
        private readonly LastChangeFourPrgFunPayEntities context = new LastChangeFourPrgFunPayEntities();

        public ClientsWindow()
        {
            InitializeComponent();
            RefreshData();
        }

        private void RefreshData()
        {
            try
            {
                TableWindow.ItemsSource = context.Clients.ToList();
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

        private void InsertClientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsValidEmail(ClientEmailTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите корректный адрес электронной почты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!IsValidPhoneNumber(ClientPhoneTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите корректный номер телефона.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int newClientId = 1;
                while (context.Clients.Any(c => c.ClientID == newClientId))
                {
                    newClientId++;
                }

                Clients newClient = new Clients
                {
                    ClientID = newClientId,
                    ClientFirstName = ClientFirstNameTextBox.Text,
                    ClientLastName = ClientLastNameTextBox.Text,
                    ClientEmail = ClientEmailTextBox.Text,
                    ClientPhone = ClientPhoneTextBox.Text
                };
                context.Clients.Add(newClient);
                context.SaveChanges();
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении клиента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateClientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clients selectedClient = TableWindow.SelectedItem as Clients;
                if (selectedClient != null)
                {
                    selectedClient.ClientFirstName = ClientFirstNameTextBox.Text;
                    selectedClient.ClientLastName = ClientLastNameTextBox.Text;
                    selectedClient.ClientEmail = ClientEmailTextBox.Text;
                    selectedClient.ClientPhone = ClientPhoneTextBox.Text;
                    context.Entry(selectedClient).State = EntityState.Modified;
                    context.SaveChanges();
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Выберите клиента для обновления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении клиента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clients selectedClient = TableWindow.SelectedItem as Clients;
                if (selectedClient != null)
                {
                    context.Clients.Remove(selectedClient);
                    context.SaveChanges();
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Выберите клиента для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (TableWindow.SelectedItem is Clients selectedClient)
            {
                ClientFirstNameTextBox.Text = selectedClient.ClientFirstName;
                ClientLastNameTextBox.Text = selectedClient.ClientLastName;
                ClientEmailTextBox.Text = selectedClient.ClientEmail;
                ClientPhoneTextBox.Text = selectedClient.ClientPhone;
            }
            else
            {
                ClientFirstNameTextBox.Text = "";
                ClientLastNameTextBox.Text = "";
                ClientEmailTextBox.Text = "";
                ClientPhoneTextBox.Text = "";
            }
        }
    }
}