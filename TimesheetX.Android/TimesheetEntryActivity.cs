using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TimesheetX.Android
{
    [Activity(Label = "Timesheet Entry")]
    public class TimesheetEntryActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.TimesheetEntry);            
            var dateText = FindViewById<TextView>(Resource.Id.DateText);
            dateText.Text = Intent.GetStringExtra("Date");
            var customerText = FindViewById<TextView>(Resource.Id.CustomerText);
            customerText.Text = Intent.GetStringExtra("Customer");
            var projectText = FindViewById<TextView>(Resource.Id.ProjectText);
            projectText.Text = Intent.GetStringExtra("Project");
            var hoursInput = FindViewById<Spinner>(Resource.Id.HoursInput);
            hoursInput.Adapter = new ArrayAdapter<string>(this, Resource.Layout.TimesheetEntryHoursRow, CreateHoursInput());
            hoursInput.SetSelection(32);

        }

        private string[] CreateHoursInput()
        {
            List<string> hours = new List<string>();
            for (double i = 0; i <= 24; i += 0.25)
            {
                hours.Add(i.ToString());
            }
            return hours.ToArray();
        }
    }
}
