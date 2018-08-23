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
using Android.Graphics;
using Android.Locations;
using Android.Util;
using System.Threading.Tasks;

namespace MobileApps
{
    [Activity(Label = "MainPage")]
    public class MainPage : Activity, ILocationListener
    {
        Button btnTimeline, btnPhoto, btnLocation, btnProfile;
        EditText dialogEditText;
        TextView myTextView;
        ListView myListView;
        ImageView imageView;
        byte[] imageArray;
        private MyAdapter _adapter;
        private List<Message> messages;
        private List<Photo> photos;
        DBManager databaseManager;
        Location currentLocation;
        static readonly string TAG = "X" + typeof(MainPage).Name;
        LocationManager _locationManager;
        string locationProvider;
        int x = 0;
        int y = 0;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.MainPage);
            btnPhoto = FindViewById<Button>(Resource.Id.btnPhoto);
            btnTimeline = FindViewById<Button>(Resource.Id.btnTimeline);
            btnLocation = FindViewById<Button>(Resource.Id.btnLocation);
            btnProfile = FindViewById<Button>(Resource.Id.btnProfile);
            dialogEditText = FindViewById<EditText>(Resource.Id.dialogEditText);
            myListView = FindViewById<ListView>(Resource.Id.myListView);
            myTextView = FindViewById<TextView>(Resource.Id.myTextView);
            imageView = FindViewById<ImageView>(Resource.Id.myImageView);
            myListView.ItemLongClick += MyListView_ItemLongClick;
            myListView.Adapter = _adapter;
            btnTimeline.Click += BtnTimeline_Click;
            btnProfile.Click += BtnProfile_Click;
            btnPhoto.Click += BtnPhoto_Click;
            btnLocation.Click += BtnLocation_Click;
            databaseManager = new DBManager();

            RefreshPageImg();
            InitializeLocationManager();
            RefreshPage();
        }
        //to get location from google maps API
        private void BtnLocation_Click(object sender, EventArgs e)
        {
            y = 1;
            var geo = new Geocoder(this);
            dialogEditText.Text = "";
            //if location is null
            if (currentLocation == null)
                dialogEditText.Text = "No location yet.";
            else
            {
                //to get current address from google maps API
                //Sometimes get error and cannot be detected to get current address
                //I will commented this else statement to make it everything works fine and no error

                //var address = await geo.GetFromLocationAsync(currentLocation.Latitude, currentLocation.Longitude, 1);
                //if (address.Any())
                //{
                //    address.ToList().ForEach(addr => dialogEditText.Append(addr + "\n"));
                //}
                //else
                //    dialogEditText.Text = "Could not find address";
            }
        }
        //when photo button click and user can choose from the phone gallery
        //for the first time uploaded picture can works and second time cannot see the picture anymore

        private void BtnPhoto_Click(object sender, EventArgs e)
        {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), 0);
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "message.db");
            var db = new SQLiteConnection(dbPath);
            var data = db.CreateTable<Photo>();
            imageView.SetImageResource(Resource.Id.myImageView);
            myTextView.Append(DateTime.Now.ToString());
            Photo p = new Photo()
            {
                ImageFile = imageArray,
                DateTime = DateTime.Now.ToShortDateString()
            };
            int id = databaseManager.insertMessage(p);
            RefreshPageImg();
        }
        //to get the current location by latitude and longitude
        async Task<Address> ReverseGeocodeCurrentLocation()
        {
            Geocoder geocoder = new Geocoder(this);
            IList<Address> addressList =
                await geocoder.GetFromLocationAsync(currentLocation.Latitude, currentLocation.Longitude, 10);

            Address address = addressList.FirstOrDefault();
            return address;
        }
        //to get the location by this function
        void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria crit = new Criteria();
            crit.Accuracy = Accuracy.Fine;
            locationProvider = _locationManager.GetBestProvider(crit, false);
        }
        //when message long click, it will pop up alert dialog to confirm delete a message from the database
        private void MyListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(this);
            alertDialog.SetTitle("Confirm delete");
            alertDialog.SetPositiveButton("Yes", (senderAlert, args) =>
            {
                int id = databaseManager.deleteMessage(messages[e.Position]);
                Toast.MakeText(this, "Deleted!", ToastLength.Short).Show();
                RefreshPage();
            });
            alertDialog.SetNegativeButton("No", (senderAlert, args) => { Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show(); });
            Dialog dialog = alertDialog.Create();
            alertDialog.Show();
        }
        //to refresh page of messages into listview being updated 
        public void RefreshPage()
        {
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "message.db");
            var db = new SQLiteConnection(dbPath);
            messages = db.Table<Message>().ToList();
            var data2 = messages.Where(x => x.TextMessage == dialogEditText.Text);
            ArrayAdapter<Message> adapter = new ArrayAdapter<Message>(this, Android.Resource.Layout.SimpleListItem1, messages);
            //_adapter = new MyAdapter(this, Resource.Layout.abc_action_mode_bar, messages);
            myListView.Adapter = adapter;
            x = 0;
        }
        //to refresh page of photo into listview being updated
        public void RefreshPageImg()
        {

            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "message.db");
            var db = new SQLiteConnection(dbPath);
            photos = db.Table<Photo>().ToList();
            var data2 = photos.Where(x => x.ImageFile == imageArray);

            //_adapter = new MyAdapter(this, Resource.Layout.abc_action_mode_bar, data);
            ArrayAdapter<Photo> adapter = new ArrayAdapter<Photo>(this, Android.Resource.Layout.SimpleListItem1, photos);
            myListView.Adapter = adapter;
        }
        //convert photo into bitmap
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                Stream stream = ContentResolver.OpenInputStream(data.Data);
                Bitmap bitmap = BitmapFactory.DecodeStream(stream);
                imageView.SetImageBitmap(bitmap);

                MemoryStream memStream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Webp, 100, memStream);
                imageArray = memStream.ToArray();
            }
        }
        //when profile button clicked
        private void BtnProfile_Click(object sender, EventArgs e)
        {
            var profileActivity = new Intent(this, typeof(Profile));
            StartActivity(profileActivity);
        }
        //when timeline button clicked
        //to show alert dialog by confirm posting to timeline
        private void BtnTimeline_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(this);
            alertDialog.SetTitle("Confirm posting to timeline");
            alertDialog.SetMessage(dialogEditText.Text);
            alertDialog.SetPositiveButton("Yes", (senderAlert, args) =>
            {
                string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "message.db");
                var db = new SQLiteConnection(dbPath);
                var data = db.CreateTable<Message>();

                Message m = new Message()
                {
                    TextMessage = dialogEditText.Text,
                    DateTime = DateTime.Now.ToShortDateString(),
                };

                int id = databaseManager.insertMessage(m);
                Toast.MakeText(this, "Uploaded!", ToastLength.Short).Show();
                dialogEditText.Text = "";
                RefreshPage();
                y = 0;
            });
            alertDialog.SetNegativeButton("No", (senderAlert, args) => { Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show(); });
            Dialog dialog = alertDialog.Create();
            alertDialog.Show();
        }

        protected override void OnResume()
        {
            base.OnResume();
            _locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
        }
        //to determine the location from the google maps API
        public void OnLocationChanged(Location location)
        {
            if (y != 0)
            {
                currentLocation = location;
                //if location is null
                if (location == null)
                {
                    //to show cannot determine the location
                    dialogEditText.Text = "Unable to determine location yet. ";
                }
                else
                {
                    if (x < 1)
                    {
                        //to show the location by latitude and longitude
                        dialogEditText.Text = location.Latitude + " " + location.Longitude;
                        x++;
                    }
                }
            }

        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
        }
    }
}