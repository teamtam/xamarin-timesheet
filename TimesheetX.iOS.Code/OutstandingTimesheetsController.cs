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
        private LoadingOverlay LoadingOverlay;

        public OutstandingTimesheetsController() : base("OutstandingTimesheetsController", null)
        {
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();

            AutomaticallyAdjustsScrollViewInsets = false; // NOTE: for iOS7+
            var tableView = new UITableView(View.Bounds);
            tableView.ContentInset = new UIEdgeInsets(64, 0, 0, 0);
            tableView.SeparatorInset = new UIEdgeInsets(0, 20, 0, 0);
            ShowLoadingOverlay();
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
            LoadingOverlay.Hide();
            View.AddSubview(tableView);
        }

        public void ShowLoadingOverlay()
        {
            var bounds = UIScreen.MainScreen.Bounds;
            if (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight)
                bounds.Size = new CGSize(bounds.Size.Height, bounds.Size.Width);
            LoadingOverlay = new LoadingOverlay(bounds, "Loading...");
            View.Add(LoadingOverlay);
        }

        public class OutstandingTimesheetsSource : UITableViewSource
        {
            private readonly UINavigationController NavigationController;
            private readonly IList<TimesheetEntry> Timesheets;

            private readonly NSString CELL_IDENTIFIER = new NSString("OutstandingTimesheetsCell");

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
                var timesheet = Timesheets.ElementAt(indexPath.Row);
                var cell = tableView.DequeueReusableCell(CELL_IDENTIFIER) as OutstandingTimesheetsCell;
                if (cell == null)
                    cell = new OutstandingTimesheetsCell(CELL_IDENTIFIER);
                cell.UpdateCell(timesheet.Date, timesheet.Customer, timesheet.Project);
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

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return 58;
            }
        }
    }

    public class OutstandingTimesheetsCell : UITableViewCell
    {
        private UILabel DateField;
        private UILabel CustomerField;
        private UILabel ProjectField;

        public OutstandingTimesheetsCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
        {
            SelectionStyle = UITableViewCellSelectionStyle.Gray;
            DateField = new UILabel() { Font = UIFont.BoldSystemFontOfSize(14) };
            CustomerField = new UILabel() { Font = UIFont.SystemFontOfSize(12) };
            ProjectField = new UILabel() { Font = UIFont.SystemFontOfSize(12) };
            ContentView.AddSubviews(new UIView[] { DateField, CustomerField, ProjectField });
        }

        public void UpdateCell(DateTime date, string customer, string project)
        {
            DateField.Text = date.ToString("dd MMM yyyy");
            CustomerField.Text = customer;
            ProjectField.Text = project;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            DateField.Frame = new CGRect(20, 0, ContentView.Bounds.Width - 40, 20);
            CustomerField.Frame = new CGRect(20, 20, ContentView.Bounds.Width - 40, 19);
            ProjectField.Frame = new CGRect(20, 39, ContentView.Bounds.Width - 40, 19);
        }
    }
}
