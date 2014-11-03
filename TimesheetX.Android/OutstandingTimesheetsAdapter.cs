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
using TimesheetX.Models;

namespace TimesheetX.Android
{
    public class OutstandingTimesheetsAdapter : BaseAdapter<TimesheetEntry>
    {
        Activity context;
        List<TimesheetEntry> items;

        public OutstandingTimesheetsAdapter(Activity context, List<TimesheetEntry> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override TimesheetEntry this[int position]
        {
            get { return items[position]; }
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.OutstandingTimesheetsRow, null);
            view.FindViewById<TextView>(Resource.Id.Date).Text = item.Date.ToString("dd MMM yyyy");
            view.FindViewById<TextView>(Resource.Id.Customer).Text = item.Customer;
            view.FindViewById<TextView>(Resource.Id.Project).Text = item.Project;
            return view;
        }
    }
}
