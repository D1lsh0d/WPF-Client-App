using LibraryApp.Models;
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

namespace LibraryApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditBook.xaml
    /// </summary>
    public partial class EditBook : Window
    {
        public EditBook(Books book)
        {
            InitializeComponent();
            DataContext = book;
            Quantity.Value = book.Quantity;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Books book = new Books()
            {
                Id = Int32.Parse(bookId.Text),
                Author = bookAuthor.Text,
                Name = bookName.Text,
                PrintDate = bookPrintDate.SelectedDate,
                Description = bookDescription.Text,
                Quantity = Quantity.Value
            };

            // Преобразование объекта в JSON-строку
            string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(book);

            // Создание контента запроса
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Отправка PUT-запроса на сервер
            HttpResponseMessage response = await ApiHelper.client.PutAsync(ApiHelper.client.BaseAddress + "Books/UpdateBook", content);

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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ApiHelper.UpdateBooksCollection();
        }
    }
}
