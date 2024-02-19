using System;
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
using System.Xml;

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
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ApiHelper.UpdateBooksDG();
        }


        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem selectedTab = Tabs.SelectedItem as TabItem;
            switch (selectedTab.Header)
            {
                case "Книги":
                    ApiHelper.UpdateBooksDG();
                    break;

                case "Читатели":
                    ApiHelper.UpdateUsersDG();
                    break;
                case "Книги читателей":
                    ApiHelper.UpdateUserBooksDG();
                    break;
            }
        }

        private void addBook_Click(object sender, RoutedEventArgs e)
        {
            CreateBook window = new CreateBook();
            window.ShowDialog();
        }
    }
}
