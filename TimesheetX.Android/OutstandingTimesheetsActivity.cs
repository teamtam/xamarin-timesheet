using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using TimesheetX.Models;
using TimesheetX.Services;

namespace TimesheetX.Android
{
    [Activity(Label = "TimesheetX.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private ListView listView;
        private OutstandingTimesheetsAdapter adapter;

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var timesheets = await TimesheetService.GetTimesheetEntries();

            SetContentView(Resource.Layout.OutstandingTimesheets);
            listView = FindViewById<ListView>(Resource.Id.List);
            adapter = new OutstandingTimesheetsAdapter(this, timesheets.ToList());
            listView.Adapter = adapter;
            listView.ItemClick += OnListViewRowClick;
        }

        protected void OnListViewRowClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var timesheet = adapter[e.Position];
            var intent = new Intent(this, typeof(TimesheetEntryActivity));
            intent.PutExtra("Date", timesheet.Date.ToString("dd MMM yyyy"));
            intent.PutExtra("Customer", timesheet.Customer);
            intent.PutExtra("Project", timesheet.Project);
            StartActivity(intent);
        }
    }
}
