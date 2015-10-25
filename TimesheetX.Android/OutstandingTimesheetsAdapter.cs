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
            var view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.OutstandingTimesheetsRow, parent, false);
                var dateView = view.FindViewById<TextView>(Resource.Id.Date);
                var customerView = view.FindViewById<TextView>(Resource.Id.Customer);
                var projectView = view.FindViewById<TextView>(Resource.Id.Project);
                view.Tag = new TimesheetEntryViewHolder() { Date = dateView, Customer = customerView, Project = projectView };
            }
            var viewHolder = (TimesheetEntryViewHolder) view.Tag;
            viewHolder.Date.Text = item.Date.ToString("dd MMM yyyy");
            viewHolder.Customer.Text = item.Customer;
            viewHolder.Project.Text = item.Project;
            return view;
        }
    }
}
