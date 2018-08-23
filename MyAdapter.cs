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
    class MyAdapter : ArrayAdapter
    {
        private Context c;
        private List<Message> messages;
        private List<Photo> photos;
        private LayoutInflater inflater;
        private int resource;
        public MyAdapter(Context context, int resource, List<Message> messages) : base(context, resource, messages)
        {
            this.c = context;
            this.resource = resource;
            this.messages = messages;
        }
        public MyAdapter(Context context, int resource, List<Photo> photos) : base(context, resource, photos)
        {
            this.c = context;
            this.resource = resource;
            this.photos = photos;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (inflater == null)
            {
                inflater = (LayoutInflater)c.GetSystemService(Context.LayoutInflaterService);
            }
            if (convertView == null)
            {
                convertView = inflater.Inflate(resource, parent, false);
            }

            MyHolder holder = new MyHolder(convertView)
            {
                myTextView = { Text = messages[position].TextMessage },
                myImageView = { ImageAlpha = Convert.ToInt32(photos[position].ImageFile) },

            };
            holder.myTextView.SetTextAppearance(Convert.ToInt32(messages[position].TextMessage));
            holder.myImageView.SetImageResource(Convert.ToInt32(photos[position].ImageFile));

            if (position % 2 == 0)
            {
                convertView.SetBackgroundColor(Android.Graphics.Color.LightBlue);
            }
            return convertView;
        }
    }
}