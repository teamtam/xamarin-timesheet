using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using TimesheetX.Services;

namespace TimesheetX.Android
{
    [Activity(Label = "Outstanding Timesheets", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private ListView listView;
        private OutstandingTimesheetsAdapter adapter;

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.OutstandingTimesheets);
            listView = FindViewById<ListView>(Resource.Id.List);
            listView.ItemClick += OnListViewRowClick;
            var progress = CreateProgressDialog();
            progress.Show();
            try
            {
                adapter = new OutstandingTimesheetsAdapter(this, (await TimesheetService.GetTimesheetEntries()).ToList());
                listView.Adapter = adapter;
            }
            catch
            {
                CreateExceptionDialog();
            }
            progress.Hide();
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

        private ProgressDialog CreateProgressDialog()
        {
            var progress = new ProgressDialog(this)
            {
                Indeterminate = true
            };
            progress.SetCancelable(false);
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage("Contacting server. Please wait...");
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
