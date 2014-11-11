using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using TimesheetX.Models;

namespace TimesheetX.Android
{
    public class OutstandingTimesheetsAdapter : BaseAdapter<TimesheetEntry>
    {
        private readonly Activity context;
        private readonly List<TimesheetEntry> items;

        public OutstandingTimesheetsAdapter(Activity context, List<TimesheetEntry> items)
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
            var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.OutstandingTimesheetsRow, null);
            view.FindViewById<TextView>(Resource.Id.Date).Text = item.Date.ToString("dd MMM yyyy");
            view.FindViewById<TextView>(Resource.Id.Customer).Text = item.Customer;
            view.FindViewById<TextView>(Resource.Id.Project).Text = item.Project;
            return view;
        }
    }
}
