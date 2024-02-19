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

namespace LibraryApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для CreateBook.xaml
    /// </summary>
    public partial class CreateBook : Window
    {
        public CreateBook()
        {
            InitializeComponent();
        }

        private async void addBook_Click(object sender, RoutedEventArgs e)
        {
            Books book = new Books()
            {
                Author = bookAuthor.Text,
                Name = bookName.Text,
                PrintDate = bookPrintDate.SelectedDate,
                Description = bookDescription.Text
            };

            // Преобразование объекта в JSON-строку
            string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(book);

            // Создание контента запроса
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Отправка POST-запроса на сервер
            HttpResponseMessage response = await ApiHelper.client.PostAsync(ApiHelper.client.BaseAddress + "Books/AddBook", content);

            // Проверка успешности запроса
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show(response.Content.ReadAsStringAsync().Result, caption: "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show($"Error: {response.StatusCode} - {response.ReasonPhrase}", caption: "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
