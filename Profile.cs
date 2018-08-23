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
using System.IO;

namespace MobileApps
{
    [Activity(Label = "Profile")]
    public class Profile : Activity
    {
        Button btnUpdate;
        EditText tbxFirstName, tbxLastName, tbxDOB, tbxEmail, tbxPassword;
        TextView lblOutput;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ProfilePage);
            btnUpdate = FindViewById<Button>(Resource.Id.btnUpdate);
            tbxFirstName = FindViewById<EditText>(Resource.Id.tbxFirstName);
            tbxLastName = FindViewById<EditText>(Resource.Id.tbxLastName);
            tbxDOB = FindViewById<EditText>(Resource.Id.tbxDOB);
            tbxEmail = FindViewById<EditText>(Resource.Id.tbxEmail);
            tbxPassword = FindViewById<EditText>(Resource.Id.tbxPassword);
            lblOutput = FindViewById<TextView>(Resource.Id.lblOutput);
            btnUpdate.Click += BtnUpdate_Click;
            
            //to call database for users
            string dbFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "users.db");
            var db = new SQLiteConnection(dbFilePath);
            var data = db.Table<User>();
            int id = 1;
            //to select the data from database and pull out into profile page
            var data1 = (from values in data where values.id == id select new User { FirstName = values.FirstName, LastName = values.LastName, DOB = values.DOB, Email = values.Email, Password = values.Password }).ToList<User>();
            if (data1.Count > 0)
            {
                foreach (var val in data1)
                {
                    tbxFirstName.Text = val.FirstName;
                    tbxLastName.Text = val.LastName;
                    tbxDOB.Text = val.DOB;
                    tbxEmail.Text = val.Email;
                    tbxPassword.Text = val.Password;
                }
            }
            else
            {
                Toast.MakeText(this, "Data Not Available", ToastLength.Short).Show();
            }
            
        }
        //when update button clicked
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            string dbFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "users.db");
            var db = new SQLiteConnection(dbFilePath);
            var data = db.Table<User>();
            int id = 1;
            var data1 = (from values in data where values.id == id select values).Single();
            data1.FirstName = tbxFirstName.Text;
            data1.LastName = tbxLastName.Text;
            data1.DOB = tbxDOB.Text;
            data1.Email = tbxEmail.Text;
            data1.Password = tbxPassword.Text;
            db.Update(data1);
            Toast.MakeText(this, "Updated Successfully!", ToastLength.Short).Show();
        }
        
        }
    }
