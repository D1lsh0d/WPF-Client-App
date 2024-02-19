using LibraryApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryApp
{
    public static class ApiHelper
    {
        public static ObservableCollection<Books> booksCollection = new ObservableCollection<Books>();
        public static ObservableCollection<Users> usersCollection = new ObservableCollection<Users>();
        public static ObservableCollection<UserBooks> userBooksCollection = new ObservableCollection<UserBooks>();

        public static HttpClient client = new HttpClient();

        public static void UpdateBooksDG()
        {
            try
            {
                booksCollection.Clear();
                //HTTP GET
                var responseTask = client.GetAsync("Books");
                responseTask.Wait();

                var GetResult = responseTask.Result;
                if (GetResult.IsSuccessStatusCode)
                {
                    var readTask = GetResult.Content.ReadAsAsync<Books[]>();
                    readTask.Wait();

                    var books = new ObservableCollection<Books>(readTask.Result);

                    foreach (Books book in books)
                    {
                        booksCollection.Add(book);
                    }
                }
                else
                {
                    MessageBox.Show(responseTask.Result.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void UpdateUsersDG()
        {
            try
            {
                usersCollection.Clear();
                //HTTP GET
                var responseTask = client.GetAsync("Users");
                responseTask.Wait();

                var GetResult = responseTask.Result;
                if (GetResult.IsSuccessStatusCode)
                {
                    var readTask = GetResult.Content.ReadAsAsync<Users[]>();
                    readTask.Wait();

                    var users = new ObservableCollection<Users>(readTask.Result);

                    foreach (Users user in users)
                    {
                        usersCollection.Add(user);
                    }
                }
                else
                {
                    MessageBox.Show(responseTask.Result.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void UpdateUserBooksDG()
        {
            try
            {
                userBooksCollection.Clear();
                //HTTP GET
                var responseTask = client.GetAsync("UserBooks");
                responseTask.Wait();

                var GetResult = responseTask.Result;
                if (GetResult.IsSuccessStatusCode)
                {
                    var readTask = GetResult.Content.ReadAsAsync<UserBooks[]>();
                    readTask.Wait();

                    var userBooks = new ObservableCollection<UserBooks>(readTask.Result);

                    foreach (UserBooks userBook in userBooks)
                    {
                        userBooksCollection.Add(userBook);
                    }
                }
                else
                {
                    MessageBox.Show(responseTask.Result.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
