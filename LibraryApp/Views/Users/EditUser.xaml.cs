using LibraryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
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

namespace LibraryApp.Views.Users
{
    /// <summary>
    /// Логика взаимодействия для EditUser.xaml
    /// </summary>
    public partial class EditUser : Window
    {
        private Models.Users oldUser;
        public EditUser(Models.Users user)
        {
            InitializeComponent();
            DataContext = user; 
            oldUser = user;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Models.Users updatedUser = new Models.Users()
            {
                Id = oldUser.Id,
                FullName = FullNameTextBox.Text,
                DateOfBirth = DateOfBirthPicker.SelectedDate,
                Address = AddressTextBox.Text,
                Email = EmailTextBox.Text,
                Phone = PhoneTextBox.Text
            };

            // Преобразование объекта в JSON-строку
            string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(updatedUser);

            // Создание контента запроса
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Отправка PUT-запроса на сервер
            HttpResponseMessage response = await ApiHelper.client.PutAsync(ApiHelper.client.BaseAddress + "Users/", content);

            // Проверка успешности запроса
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show(response.Content.ReadAsStringAsync().Result, caption: "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show($"Error: {response.StatusCode} - {response.Content.ReadAsStringAsync().Result}", caption: "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
