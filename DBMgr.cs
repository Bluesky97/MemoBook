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
using System.IO;
using SQLite;

namespace MobileApps
{
    class DBMgr
    {
        //create user database
        public static readonly string dbFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "users.db");
        private SQLiteConnection db = null;
        //create table for user 
        public DBMgr()
        {
            db = new SQLiteConnection(dbFilePath);
            db.CreateTable<User>();
        }
        //insert user into database
        public int insertUser(User c)
        {
            db.Insert(c);
            return c.id;
        }
        //update user into database
        public int updateUser(User c)
        {
            db.Update(c);
            return c.id;
        }
    }
}