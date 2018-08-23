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

namespace MobileApps
{
    class MyHolder
    {
        public TextView myTextView;
        public ImageView myImageView;

        public MyHolder(View v)
        {
            this.myTextView = v.FindViewById<TextView>(Resource.Id.myTextView);
            this.myImageView = v.FindViewById<ImageView>(Resource.Id.myImageView);
        }
    }

}