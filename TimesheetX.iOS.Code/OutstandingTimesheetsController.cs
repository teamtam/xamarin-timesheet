using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using TimesheetX.Models;
using TimesheetX.Services;

namespace TimesheetX.iOS.Code
{
    public partial class OutstandingTimesheetsController : UITableViewController
    {
        private IList<TimesheetEntry> Timesheets;

        public OutstandingTimesheetsController() : base("OutstandingTimesheetsController", null)
        {
        }

        public async override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TableView.ContentInset = new UIEdgeInsets(20, 0, 0, 0);
            // TODO: progress indicator
            try
            {
                Timesheets = (await TimesheetService.GetTimesheetEntries()).ToList();
            }
            catch (Exception)
            {
                // TODO: modal error message
            }
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
            return cell;
        }
    }
}
