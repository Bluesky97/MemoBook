using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.IO;
using SQLite;
using System.Linq;
using System.Collections.Generic;

namespace MobileApps
{
    [Activity(Label = "MemoBook", MainLauncher = true, Icon ="@drawable/Accelrys" )]
    public class MainActivity : Activity
    {
        Button btnLogin, btnRegister1;
        TextView lblOutput;
        EditText tbxEmail, tbxPassword;
        ImageView imageView1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            btnRegister1 = FindViewById<Button>(Resource.Id.btnRegister1);
            imageView1 = FindViewById<ImageView>(Resource.Id.imageView1);
            tbxEmail = FindViewById<EditText>(Resource.Id.tbxEmail);
            tbxPassword = FindViewById<EditText>(Resource.Id.tbxPassword);
            lblOutput = FindViewById<TextView>(Resource.Id.lblOutput);
            btnRegister1.Click += BtnRegister1_Click;

            //when login button click
            btnLogin.Click += delegate
            {
                string inputemail = tbxEmail.Text.ToString();
                TextView txtValDisplay = FindViewById<TextView>(Resource.Id.txtValDisplay);
                TextView txtValDispPw = FindViewById<TextView>(Resource.Id.txtValDispPw);
                //email validation 
                var emailvalidate = isValidEmail(inputemail);
                //validation check if no input from user 
                if (inputemail == "")
                {
                    txtValDisplay.Text = "Enter the Email!";
                }
                else
                {
                    //to check email is correct format
                    if (emailvalidate == true)
                    {
                        txtValDisplay.Text = "Email is valid";
                    }
                    else
                    {
                        txtValDisplay.Text = "Email is not valid";
                    }
                }
                if (tbxPassword.Text == "")

                {
                    txtValDispPw.Text = "Password cannot be blanks!";
                }
                else
                {
                    //calling database for user 
                    string dbFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "users.db");
                    var db = new SQLiteConnection(dbFilePath);
                    var data = db.Table<User>();
                    //checking user login from database
                    var data1 = data.Where(x => x.Email == tbxEmail.Text && x.Password == tbxPassword.Text).FirstOrDefault();
                    if (data1 != null)
                    {
                        Toast.MakeText(this, "Login successfully!", ToastLength.Short).Show();
                        var activity = new Intent(this, typeof(MainPage));
                        StartActivity(activity);
                    }
                    else
                    {
                        Toast.MakeText(this, "Username or Password invalid", ToastLength.Short).Show();
                    }
                    
                }
            };
        }
      //when register button click and move to the another layout called RegisterPage
        private void BtnRegister1_Click(object sender, System.EventArgs e)
        {
            var register = new Intent(this, typeof(RegisterActivity));
            StartActivity(register);
        }
        //email validation 
        public bool isValidEmail(string email)
        {
            return Android.Util.Patterns.EmailAddress.Matcher(email).Matches();
        }

    }
}

