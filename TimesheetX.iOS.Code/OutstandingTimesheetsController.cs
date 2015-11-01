using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using TimesheetX.Models;
using TimesheetX.Services;

namespace TimesheetX.iOS.Code
{
    public partial class OutstandingTimesheetsController : UIViewController
    {
        public OutstandingTimesheetsController() : base("OutstandingTimesheetsController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var tableView = new UITableView(View.Bounds);
            tableView.ContentInset = new UIEdgeInsets(0, 0, 0, 0);
            // TODO: progress indicator & await
            try
            {
                var timesheets = Task.Run(TimesheetService.GetTimesheetEntries).Result;
                tableView.Source = new OutstandingTimesheetsSource(NavigationController, timesheets);
            }
            catch (Exception)
            {
                // TODO: modal error message
            }
            View.AddSubview(tableView);
        }

        public class OutstandingTimesheetsSource : UITableViewSource
        {
            private readonly UINavigationController NavigationController;
            private readonly IList<TimesheetEntry> Timesheets;

            public OutstandingTimesheetsSource(UINavigationController navigationController, IEnumerable<TimesheetEntry> timesheets)
            {
                NavigationController = navigationController;
                Timesheets = timesheets.ToList();
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return Timesheets.Count;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                // TODO: custom table cell https://developer.xamarin.com/guides/ios/user_interface/tables/part_3_-_customizing_a_table's_appearance/
                var timesheet = Timesheets.ElementAt(indexPath.Row);
                var cell = new UITableViewCell(UITableViewCellStyle.Subtitle, null);
                cell.TextLabel.Text = timesheet.Date.ToString("dd MMM yyyy");
                cell.DetailTextLabel.Text = timesheet.Customer + ": " + timesheet.Project;
                cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                return cell;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                var timesheetEntryController = new TimesheetEntryController();
                timesheetEntryController.TimesheetEntry = Timesheets.ElementAt(indexPath.Row);
                NavigationController.PushViewController(timesheetEntryController, true);
                tableView.DeselectRow(indexPath, true);
            }
        }
    }
}
