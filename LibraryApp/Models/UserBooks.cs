using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Models
{
    public partial class UserBooks
    {
        public int UserBookID { get; set; }
        public int UserID { get; set; }
        public int BookID { get; set; }
        public Nullable<System.DateTime> CheckoutDate { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
    }
}
