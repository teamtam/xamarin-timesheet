using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;
using TimesheetX.Models;
using TimesheetX.Services;

namespace TimesheetX.iOS.Code
{
    public partial class OutstandingTimesheetsController : UIViewController
    {
        //private LoadingOverlay LoadingOverlay;

        public OutstandingTimesheetsController() : base("OutstandingTimesheetsController", null)
        {
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();

            var tableView = new UITableView(View.Bounds);
            tableView.ContentInset = new UIEdgeInsets(0, 0, 0, 0);
            // TODO: loading overlay changes the inset
            // ShowLoadingOverlay();
            try
            {
                var timesheets = await TimesheetService.GetTimesheetEntries();
                tableView.Source = new OutstandingTimesheetsSource(NavigationController, timesheets);
            }
            catch (Exception ex)
            {
                var okAlertController = UIAlertController.Create("Error", ex.Message, UIAlertControllerStyle.Alert);
                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                PresentViewController(okAlertController, true, null);
            }
            // LoadingOverlay.Hide();
            View.AddSubview(tableView);
        }

        /*
        public void ShowLoadingOverlay()
        {
            var bounds = UIScreen.MainScreen.Bounds;
            if (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight)
                bounds.Size = new CGSize(bounds.Size.Height, bounds.Size.Width);
            LoadingOverlay = new LoadingOverlay(bounds);
            View.Add(LoadingOverlay);
        }
        */

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
