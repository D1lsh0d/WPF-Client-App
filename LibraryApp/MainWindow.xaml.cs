﻿using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibraryApp.Models;
using LibraryApp.Windows;
using LibraryApp.Views;
using System.Xml;
using LibraryApp.Views.Users;

namespace LibraryApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            booksDataGrid.ItemsSource = ApiHelper.booksCollection;
            usersDataGrid.ItemsSource = ApiHelper.usersCollection;
            userBooksDataGrid.ItemsSource = ApiHelper.userBooksCollection;
            ApiHelper.client.BaseAddress = new Uri("http://localhost:50923/api/");
            
            if (!ApiHelper.IsApiReachable())
            {
                MessageBox.Show("Server is not reachable. ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }
        

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource is TabControl) // Проверяем точно ли событие вызвано открытием TabControl
            {
                TabItem selectedTab = Tabs.SelectedItem as TabItem;

                switch (selectedTab.Header)
                {
                    case "Книги":
                        ApiHelper.UpdateBooksCollection();
                        break;

                    case "Читатели":
                        ApiHelper.UpdateUsersCollection();
                        break;

                    case "Книги читателей":
                        ApiHelper.UpdateUserBooksCollection();
                        break;
                }
            }
        }
        #region "Books tab" 
        private void addBook_Click(object sender, RoutedEventArgs e)
        {
            CreateBook window = new CreateBook();
            window.ShowDialog();
        }

        private void editBook_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, есть ли выделенные ячейки
            if (booksDataGrid.SelectedCells.Count > 0)
            {
                // Получаем выбранный объект из коллекции
                var selectedObject = booksDataGrid.SelectedItem;

                if (selectedObject is Books book)
                {
                    EditBook editBook = new EditBook(book);
                    editBook.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Для редактирования сначала выберите строку книги, затем нажмите на кнопку \"Редактировать\" ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void deleteBookBtn_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, есть ли выделенные ячейки
            if (booksDataGrid.SelectedCells.Count > 0)
            {
                // Получаем выбранный объект из коллекции
                var selectedObject = booksDataGrid.SelectedItem;

                if (selectedObject is Books book)
                {
                    MessageBoxResult confirmDelete = MessageBox.Show($"Вы уверены что хотите удалить книгу \"{book.Name}\"?", "Подтвердите удаление", MessageBoxButton.YesNo, MessageBoxImage.Hand);
                    if (confirmDelete == MessageBoxResult.Yes)
                    {
                        // Отправка DELETE-запроса на сервер
                        HttpResponseMessage response = ApiHelper.client.DeleteAsync(ApiHelper.client.BaseAddress + "Books/" + book.Id.ToString()).Result;

                        // Проверка успешности запроса
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show(response.Content.ReadAsStringAsync().Result, caption: "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            ApiHelper.booksCollection.Remove(book);
                        }
                        else
                        {
                            MessageBox.Show($"Error: {response.StatusCode} - {response.ReasonPhrase}", caption: "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Для удаления сначала выберите строку книги, затем нажмите на кнопку \"Удалить\" ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }
        #endregion

        #region "Users tab"
        private void addUserBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateUser createUser = new CreateUser();
            createUser.ShowDialog();
        }

        private void deleteUserBtn_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, есть ли выделенные ячейки
            if (usersDataGrid.SelectedCells.Count > 0)
            {
                // Получаем выбранный объект из коллекции
                var selectedObject = usersDataGrid.SelectedItem;

                if (selectedObject is Users user)
                {
                    MessageBoxResult confirmDelete = MessageBox.Show($"Вы уверены что хотите удалить пользователя?", "Подтвердите удаление", MessageBoxButton.YesNo, MessageBoxImage.Hand);
                    if (confirmDelete == MessageBoxResult.Yes)
                    {
                        // Отправка DELETE-запроса на сервер
                        HttpResponseMessage response = ApiHelper.client.DeleteAsync(ApiHelper.client.BaseAddress + "Users/" + user.Id.ToString()).Result;

                        // Проверка успешности запроса
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show(response.Content.ReadAsStringAsync().Result, caption: "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            ApiHelper.usersCollection.Remove(user);
                        }
                        else
                        {
                            MessageBox.Show($"Error: {response.StatusCode} - {response.ReasonPhrase}", caption: "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Для удаления сначала выберите строку пользователя в таблиуе, затем нажмите на кнопку \"Удалить\" ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }
        

        private void editUserBtn_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, есть ли выделенные ячейки
            if (usersDataGrid.SelectedCells.Count > 0)
            {
                // Получаем выбранный объект из коллекции
                var selectedObject = usersDataGrid.SelectedItem;

                if (selectedObject is Users user)
                {
                    EditUser editUser = new EditUser(user);
                    editUser.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Для редактирования сначала выберите строку книги, затем нажмите на кнопку \"Редактировать\" ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        #endregion

        #region "Users-Books tab"
        private void addUserBookBtn_Click(object sender, RoutedEventArgs e)
        {
            Views.UserBooks.Create create = new Views.UserBooks.Create();
            create.Show();
        }


        private void editUserBookClick(object sender, RoutedEventArgs e)
        {

            // Проверяем, есть ли выделенные ячейки
            if (userBooksDataGrid.SelectedCells.Count > 0)
            {
                // Получаем выбранный объект из коллекции
                var selectedObject = userBooksDataGrid.SelectedItem;

                if (selectedObject is UserBooks record)
                {
                    Views.UserBooks.Edit edit = new Views.UserBooks.Edit(record);
                    edit.Show();
                }
            }
            else
            {
                MessageBox.Show("Для редактирования сначала выберите строку книги, затем нажмите на кнопку \"Редактировать\" ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            
        }

        private void deleteRecordClick(object sender, RoutedEventArgs e)
        {
            // Проверяем, есть ли выделенные ячейки
            if (userBooksDataGrid.SelectedCells.Count > 0)
            {
                // Получаем выбранный объект из коллекции
                var selectedObject = userBooksDataGrid.SelectedItem;

                if (selectedObject is UserBooks record)
                {
                    MessageBoxResult confirmDelete = MessageBox.Show($"Вы уверены что хотите удалить книгу эту запись?", "Подтвердите удаление", MessageBoxButton.YesNo, MessageBoxImage.Hand);
                    if (confirmDelete == MessageBoxResult.Yes)
                    {
                        // Отправка DELETE-запроса на сервер
                        HttpResponseMessage response = ApiHelper.client.DeleteAsync(ApiHelper.client.BaseAddress + "UserBooks/" + record.UserBookID.ToString()).Result;

                        // Проверка успешности запроса
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show(response.Content.ReadAsStringAsync().Result, caption: "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            ApiHelper.userBooksCollection.Remove(record);
                        }
                        else
                        {
                            MessageBox.Show($"Error: {response.StatusCode} - {response.ReasonPhrase}", caption: "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Для удаления сначала выберите строку книги, затем нажмите на кнопку \"Удалить\" ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }
        #endregion
    }
}
