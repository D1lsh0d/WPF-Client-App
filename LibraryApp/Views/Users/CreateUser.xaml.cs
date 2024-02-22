using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using LibraryApp.Models;

namespace LibraryApp.Views.Users
{
    /// <summary>
    /// Логика взаимодействия для CreateUser.xaml
    /// </summary>
    public partial class CreateUser : Window
    {
        public CreateUser()
        {
            InitializeComponent();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Models.Users newUser = new Models.Users
            {
                FullName = FullNameTextBox.Text,
                DateOfBirth = DateOfBirthPicker.SelectedDate,
                Address = AddressTextBox.Text,
                Email = EmailTextBox.Text,
                Phone = PhoneTextBox.Text
            };

            // Преобразование объекта в JSON-строку
            string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(newUser);

            // Создание контента запроса
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Отправка POST-запроса на сервер
            HttpResponseMessage response = await ApiHelper.client.PostAsync(ApiHelper.client.BaseAddress + "Users", content);

            // Проверка успешности запроса
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show(response.Content.ReadAsStringAsync().Result, caption: "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ApiHelper.UpdateUsersCollection();
                this.Close();
            }
            else
            {
                MessageBox.Show($"Error: {response.StatusCode} - {response.ReasonPhrase}", caption: "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
