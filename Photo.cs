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
    [Table("Message")]
    class Photo
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public byte[] ImageFile { get; set; }
        public string DateTime { get; set; }
        public Photo (int id, byte[] imageFile, string dateTime)
        {
            this.id = id;
            this.ImageFile = imageFile;
            this.DateTime = dateTime;
        }

        public Photo()
        {
        }

        public int Id
        {
            get { return Id; }
        }
        public int imagefile
        {
            get { return imagefile; }
        }
        public string dateTime
        {
            get { return dateTime; }
        }
        public override string ToString()
        {
            return id + " " + ImageFile + " " + DateTime;
        }
    }
}