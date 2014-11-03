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
        ListView listView;

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var timesheets = await TimesheetService.GetTimesheetEntries();

            SetContentView(Resource.Layout.OutstandingTimesheets);
            listView = FindViewById<ListView>(Resource.Id.List);
            listView.Adapter = new OutstandingTimesheetsAdapter(this, timesheets.ToList());

            //button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
        }
    }
}
