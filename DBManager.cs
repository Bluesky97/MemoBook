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
    class DBManager
    {
        //creating database for message
        public static readonly string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "message.db");
        private SQLiteConnection db = null;
        //create table for message
        public DBManager()
        {
            db = new SQLiteConnection(dbPath);
            db.CreateTable<Message>();
        }
        //insert message into database
        public int insertMessage(Message c)
        {
            db.Insert(c);
            return c.id;
        }
        //delete message from database 
        public int deleteMessage(Message c)
        {
            db.Delete(c);
            return c.id;
        }
        //insert photo into database
        public int insertMessage(Photo p)
        {
            db.Insert(p);
            return p.id;
        }
    }
}