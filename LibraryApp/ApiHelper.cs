using LibraryApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace LibraryApp
{
    public static class ApiHelper
    {
        public static ObservableCollection<Books> booksCollection = new ObservableCollection<Books>();
        public static ObservableCollection<Users> usersCollection = new ObservableCollection<Users>();
        public static ObservableCollection<UserBooks> userBooksCollection = new ObservableCollection<UserBooks>();

        public static HttpClient client = new HttpClient();

        /// <summary>
        /// Метод для проверки подлкючения к WebAPI
        /// </summary>
        /// <returns></returns>
        public static bool IsApiReachable()
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("http://localhost:50923/");
                request.Method = "HEAD"; // Используем HEAD запрос для проверки доступности без загрузки тела ответа

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch
            {
                return false;
            }
        }
        public static void UpdateBooksCollection()
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

        public static void UpdateUsersCollection()
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

        public static void UpdateUserBooksCollection()
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
