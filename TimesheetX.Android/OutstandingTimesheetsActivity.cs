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
    public class MainActivity : ListActivity
    {
        int count = 1;

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var hello = (await TimesheetService.GetTimesheetEntries()).Select(ts => ts.Date.ToString()).ToArray();
            this.ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.OutstandingTimesheets, hello);

            //// Set our view from the "main" layout resource
            //SetContentView(Resource.Layout.Main);

            //// Get our button from the layout resource,
            //// and attach an event to it
            //Button button = FindViewById<Button>(Resource.Id.MyButton);

            //button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
        }
    }
}
