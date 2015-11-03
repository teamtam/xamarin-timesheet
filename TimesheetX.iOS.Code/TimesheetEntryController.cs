using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using UIKit;
using TimesheetX.Models;
using TimesheetX.Services;

namespace TimesheetX.iOS.Code
{
    public partial class TimesheetEntryController : UIViewController
    {
        private LoadingOverlay LoadingOverlay;
        public TimesheetEntry TimesheetEntry;

        public TimesheetEntryController() : base("TimesheetEntryController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var dateLabel = CreateDateLabel();
            var dateField = CreateDateField();
            var customerLabel = CreateCustomerLabel();
            var customerField = CreateCustomerField();
            var projectLabel = CreateProjectLabel();
            var projectField = CreateProjectField();
            var hoursLabel = CreateHoursLabel();
            var hoursField = CreateHoursField();
            var commentLabel = CreateCommentLabel();
            var commentField = CreateCommentField();
            var sickLeaveLabel = CreateSickLeaveLabel();
            var sickLeaveField = CreateSickLeaveField();
            var submitButton = CreateSubmitButton(hoursField, commentField, sickLeaveField);
            View.AddSubviews(dateLabel, dateField, customerLabel, customerField, projectLabel, projectField, hoursLabel, hoursField, commentLabel, commentField, sickLeaveLabel, sickLeaveField, submitButton);

            var gestureRecognizer = new UITapGestureRecognizer(() => View.EndEditing(true));
            gestureRecognizer.CancelsTouchesInView = false; // NOTE: for iOS5
            View.AddGestureRecognizer(gestureRecognizer);
        }

        private UILabel CreateDateLabel()
        {
            return new UILabel()
            {
                Frame = new CGRect(20, 80, 80, 32),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Date"
            };
        }

        private UILabel CreateDateField()
        {
            return new UILabel()
            {
                Frame = new CGRect(100, 80, View.Bounds.Width - 120, 32),
                Font = UIFont.SystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = TimesheetEntry.Date.ToString("dd MMM yyyy")
            };
        }

        private UILabel CreateCustomerLabel()
        {
            return new UILabel()
            {
                Frame = new CGRect(20, 120, 80, 32),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Customer"
            };
        }

        private UILabel CreateCustomerField()
        {
            return new UILabel()
            {
                Frame = new CGRect(100, 120, View.Bounds.Width - 120, 32),
                Font = UIFont.SystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = TimesheetEntry.Customer
            };
        }

        private UILabel CreateProjectLabel()
        {
            return new UILabel()
            {
                Frame = new CGRect(20, 160, 80, 32),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Project"
            };
        }

        private UILabel CreateProjectField()
        {
            return new UILabel()
            {
                Frame = new CGRect(100, 160, View.Bounds.Width - 120, 32),
                Font = UIFont.SystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = TimesheetEntry.Project
            };
        }

        private UILabel CreateHoursLabel()
        {
            return new UILabel()
            {
                Frame = new CGRect(20, 200, 80, 32),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Hours"
            };
        }

        private UIPickerView CreateHoursField()
        {
            var hoursField = new UIPickerView()
            {
                Frame = new CGRect(100, 200, View.Bounds.Width - 120, 40),
            };
            hoursField.Model = new TimesheetEntryHoursViewModel();
            hoursField.Select(32, 0, true);
            return hoursField;
        }

        private UILabel CreateCommentLabel()
        {
            return new UILabel()
            {
                Frame = new CGRect(20, 240, 80, 32),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Comment"
            };
        }

        private UITextField CreateCommentField()
        {
            var commentField = new UITextField()
            {
                Frame = new CGRect(100, 240, View.Bounds.Width - 120, 32),
                KeyboardType = UIKeyboardType.Default,
                BorderStyle = UITextBorderStyle.RoundedRect,
                Placeholder = ""
            };
            commentField.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };
            return commentField;
        }

        private UILabel CreateSickLeaveLabel()
        {
            return new UILabel()
            {
                Frame = new CGRect(20, 280, 80, 32),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Sick Leave"
            };
        }

        private UISwitch CreateSickLeaveField()
        {
            var sickLeaveField = new UISwitch()
            {
                Frame = CGRect.Empty
            };
            sickLeaveField.Transform = CGAffineTransform.MakeScale((nfloat)1, (nfloat)1);
            sickLeaveField.Frame = new CGRect(View.Bounds.Width - sickLeaveField.Frame.Size.Width - 20, 280, sickLeaveField.Frame.Size.Width, sickLeaveField.Frame.Size.Height);
            return sickLeaveField;
        }

        private UIButton CreateSubmitButton(UIPickerView hoursField, UITextField commentField, UISwitch sickLeaveField)
        {
            var submitButton = new UIButton(UIButtonType.System)
            {
                Frame = new CGRect(20, 320, View.Bounds.Width - 40, 32),
            };
            submitButton.SetTitle("Submit", UIControlState.Normal);
            submitButton.TouchUpInside += async (sender, e) =>
            {
                var timesheetEntryHoursViewModel = (TimesheetEntryHoursViewModel)hoursField.Model;
                var selectedHours = timesheetEntryHoursViewModel.SelectedValue(hoursField, hoursField.SelectedRowInComponent(0), 0);
                TimesheetEntry.Hours = Convert.ToDecimal(selectedHours);
                TimesheetEntry.Comment = commentField.Text;
                TimesheetEntry.SickLeave = sickLeaveField.On;
                ShowLoadingOverlay();
                try
                {
                    await TimesheetService.SubmitTimesheetEntry(TimesheetEntry);
                    NavigationController.PopViewController(true);
                }
                catch (Exception ex)
                {
                    var okAlertController = UIAlertController.Create("Error", ex.Message, UIAlertControllerStyle.Alert);
                    okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                    PresentViewController(okAlertController, true, null);
                }
                LoadingOverlay.Hide();
            };
            return submitButton;
        }

        private void ShowLoadingOverlay()
        {
            var bounds = UIScreen.MainScreen.Bounds;
            if (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight)
                bounds.Size = new CGSize(bounds.Size.Height, bounds.Size.Width);
            LoadingOverlay = new LoadingOverlay(bounds, "Saving...");
            View.Add(LoadingOverlay);
        }
    }

    public class TimesheetEntryHoursViewModel : UIPickerViewModel
    {
        private List<double> Hours;

        public TimesheetEntryHoursViewModel()
        {
            Hours = new List<double>();
            for (double i = 0; i <= 24; i += 0.25)
            {
                Hours.Add(i);
            }
        }

        public override nint GetComponentCount(UIPickerView picker)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView picker, nint component)
        {
            return Hours.Count;
        }

        public override UIView GetView(UIPickerView pickerView, nint row, nint component, UIView view)
        {
            var label = new UILabel();
            label.Text = Hours.ElementAt(Convert.ToInt32(row)).ToString();
            label.BackgroundColor = UIColor.Clear;
            label.TextAlignment = UITextAlignment.Center;
            label.Font = UIFont.SystemFontOfSize(12);
            label.TextColor = UIColor.Black;
            return label;
        }

        public double SelectedValue(UIPickerView picker, nint row, nint component)
        {
            return Hours.ElementAt(Convert.ToInt32(row));
        }
    }
}
