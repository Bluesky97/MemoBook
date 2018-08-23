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
using Java.Lang;
using SQLite;

namespace MobileApps
{
    [Table("Message")]
    class Message 
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string TextMessage { get; set; }
        public string DateTime { get; set; }
        public Message(int id, string textMessage, string dateTime)
        {
            this.id = id;
            this.TextMessage = textMessage;
            this.DateTime = dateTime;
        }

        public Message()
        {
        }

        public int Id
        {
            get { return id; }
        }
        public string textMessage
        {
            get { return TextMessage; }
        }
        public string dateTime
        {
            get { return dateTime; }
        }
        public override string ToString()
        {
            return id + " " + TextMessage + " " + DateTime;
        }
    }
}