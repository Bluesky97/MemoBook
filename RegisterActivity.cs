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
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        Button btnSubmit, btnCancel;
        EditText tbxFirstName, tbxLastName, tbxDOB, tbxEmail, tbxPassword;
        DBMgr dBMgr;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.RegisterPage);
            btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            btnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            tbxFirstName = FindViewById<EditText>(Resource.Id.tbxFirstName);
            tbxLastName = FindViewById<EditText>(Resource.Id.tbxLastName);
            tbxDOB = FindViewById<EditText>(Resource.Id.tbxDOB);
            tbxEmail = FindViewById<EditText>(Resource.Id.tbxEmail);
            tbxPassword = FindViewById<EditText>(Resource.Id.tbxPassword);
            btnSubmit.Click += BtnSubmit_Click;
            btnCancel.Click += BtnCancel_Click;

            dBMgr = new DBMgr();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            var cancel = new Intent(this, typeof(MainActivity));
            StartActivity(cancel);
        }
        //when submit button clicked
        //to add user into database
        //when user didn't put anything into the textbox and show the error message by using alert dialog

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            string inputemail = tbxEmail.Text.ToString();
            var emailvalidate = isValidEmail(inputemail);
            if (tbxFirstName.Text == "" && tbxLastName.Text == "" && tbxDOB.Text == "" && tbxEmail.Text == "" && tbxPassword.Text == "")
            {
                AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(this);
                alertDialog.SetTitle("This field is required");
                alertDialog.SetMessage("First name, last name, date of birth, email and password cannot be blanks!");
                alertDialog.SetNeutralButton("OK", delegate
                {
                    alertDialog.Dispose();
                });
                Dialog dialog = alertDialog.Create();
                alertDialog.Show();
            }
            else if (tbxFirstName.Text == "")
            {
                AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(this);
                alertDialog.SetTitle("This field is required");
                alertDialog.SetMessage("First name cannot be blanks!");
                alertDialog.SetNeutralButton("OK", delegate
                {
                    alertDialog.Dispose();
                });
                Dialog dialog = alertDialog.Create();
                alertDialog.Show();
            }
            else if (tbxLastName.Text == "")
            {
                AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(this);
                alertDialog.SetTitle("This field is required");
                alertDialog.SetMessage("Last name cannot be blanks!");
                alertDialog.SetNeutralButton("OK", delegate
                {
                    alertDialog.Dispose();
                });
                Dialog dialog = alertDialog.Create();
                alertDialog.Show();
            }
            else if (tbxDOB.Text == "")
            {
                AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(this);
                alertDialog.SetTitle("This field is required");
                alertDialog.SetMessage("Date of Birth cannot be blanks!");
                alertDialog.SetNeutralButton("OK", delegate
                {
                    alertDialog.Dispose();
                });
                Dialog dialog = alertDialog.Create();
                alertDialog.Show();
            }
            else if (inputemail == "" && emailvalidate == false)
            {
                AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(this);
                alertDialog.SetTitle("This field is required");
                alertDialog.SetMessage("Email cannot be blanks and format is incorrect!");
                alertDialog.SetNeutralButton("OK", delegate
                {
                    alertDialog.Dispose();
                });
                Dialog dialog = alertDialog.Create();
                alertDialog.Show();
            }
            else if (tbxPassword.Text == "")
            {
                AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(this);
                alertDialog.SetTitle("This field is required");
                alertDialog.SetMessage("Password cannot be blanks!");
                alertDialog.SetNeutralButton("OK", delegate
                {
                    alertDialog.Dispose();
                });
                Dialog dialog = alertDialog.Create();
                alertDialog.Show();
            }
            else
            {
                //to call database into user
                string dbFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "users.db");
                var db = new SQLiteConnection(dbFilePath);
                db.CreateTable<User>();
                User u = new User()
                {
                    FirstName = tbxFirstName.Text,
                    LastName = tbxLastName.Text,
                    DOB = tbxDOB.Text,
                    Email = tbxEmail.Text,
                    Password = tbxPassword.Text
                };
                //user created
                int id = dBMgr.insertUser(u);
                //to show added message by using toast
                Toast.MakeText(this, "Added!", ToastLength.Short).Show();
            }
        }
        //check email validation in correct format
        public bool isValidEmail(string email)
        {
            return Android.Util.Patterns.EmailAddress.Matcher(email).Matches();
        }

    }
}