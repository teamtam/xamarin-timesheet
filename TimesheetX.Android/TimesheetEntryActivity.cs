using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Widget;
using TimesheetX.Models;
using TimesheetX.Services;

namespace TimesheetX.Android
{
    [Activity(Label = "Timesheet Entry")]
    public class TimesheetEntryActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.TimesheetEntry);
            var dateText = BindDateText();
            var customerText = BindCustomerText();
            var projectText = BindProjectText();
            var hoursInput = BindHoursInput();
            var commentInput = FindViewById<EditText>(Resource.Id.CommentInput);
            var sickLeaveInput = FindViewById<Switch>(Resource.Id.SickLeaveInput);
            var submitButton = FindViewById<Button>(Resource.Id.SubmitButton);
            submitButton.Click += async (sender, e) =>
            {
                await OnSubmitClick(dateText, customerText, projectText, hoursInput, commentInput, sickLeaveInput);
            };
        }

        private TextView BindDateText()
        {
            var dateText = FindViewById<TextView>(Resource.Id.DateText);
            dateText.Text = Intent.GetStringExtra("Date");
            return dateText;
        }

        private TextView BindCustomerText()
        {
            var customerText = FindViewById<TextView>(Resource.Id.CustomerText);
            customerText.Text = Intent.GetStringExtra("Customer");
            return customerText;
        }

        private TextView BindProjectText()
        {
            var projectText = FindViewById<TextView>(Resource.Id.ProjectText);
            projectText.Text = Intent.GetStringExtra("Project");
            return projectText;
        }

        private Spinner BindHoursInput()
        {
            var hoursInput = FindViewById<Spinner>(Resource.Id.HoursInput);
            hoursInput.Adapter = new ArrayAdapter<string>(this, Resource.Layout.TimesheetEntryHoursRow, CreateHoursInput());
            hoursInput.SetSelection(32);
            return hoursInput;
        }

        private string[] CreateHoursInput()
        {
            var hours = new List<string>();
            for (double i = 0; i <= 24; i += 0.25)
            {
                hours.Add(i.ToString());
            }
            return hours.ToArray();
        }

        private async Task OnSubmitClick(TextView dateText, TextView customerText, TextView projectText, Spinner hoursInput, EditText commentInput, Switch sickLeaveInput)
        {
            var timesheet = BindTimesheetEntry(dateText, customerText, projectText, hoursInput, commentInput, sickLeaveInput);
            var progress = CreateProgressDialog();
            progress.Show();
            try
            {
                await TimesheetService.SubmitTimesheetEntry(timesheet);
                Finish();
            }
            catch
            {
                CreateExceptionDialog();
            }
            progress.Hide();
        }

        private static TimesheetEntry BindTimesheetEntry(TextView dateText, TextView customerText, TextView projectText, Spinner hoursInput, EditText commentInput, Switch sickLeaveInput)
        {
            var timesheet = new TimesheetEntry()
            {
                Date = DateTime.ParseExact(dateText.Text, "dd MMM yyyy", CultureInfo.InvariantCulture),
                Customer = customerText.Text,
                Project = projectText.Text,
                Hours = Convert.ToDecimal(hoursInput.SelectedItem.ToString()),
                Comment = commentInput.Text,
                SickLeave = sickLeaveInput.Checked
            };
            return timesheet;
        }

        private ProgressDialog CreateProgressDialog()
        {
            var progress = new ProgressDialog(this)
            {
                Indeterminate = true
            };
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage("Contacting server. Please wait...");
            progress.SetCancelable(false);
            return progress;
        }

        private void CreateExceptionDialog()
        {
            var alert = new AlertDialog.Builder(this);
            alert.SetTitle("Network Error");
            alert.SetMessage("Could not contact the server - please try again later.");
            alert.SetNeutralButton("OK", (senderAlert, args) => { });
            alert.Show();
        }
    }
}
