using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace MobileApps
{
    [Table ("User")]
    class User
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public override string ToString()
        {
            return FirstName + " " + LastName + " " + DOB + " " + Email + " " + Password;
        }
    }
}