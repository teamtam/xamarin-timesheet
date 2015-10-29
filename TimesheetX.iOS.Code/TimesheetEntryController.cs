using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using UIKit;

namespace TimesheetX.iOS.Code
{
    public partial class TimesheetEntryController : UIViewController
    {
        public TimesheetEntryController() : base("TimesheetEntryController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var dateLabel = new UILabel()
            {
                Frame = new CGRect(20, 40, 80, 32),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Date"
            };
            View.AddSubview(dateLabel);

            var dateField = new UILabel()
            {
                Frame = new CGRect(100, 40, View.Bounds.Width - 120, 32),
                Font = UIFont.SystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "10/10/2015"
            };
            View.AddSubview(dateField);

            var customerLabel = new UILabel()
            {
                Frame = new CGRect(20, 80, 80, 32),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Customer"
            };
            View.AddSubview(customerLabel);

            var customerField = new UILabel()
            {
                Frame = new CGRect(100, 80, View.Bounds.Width - 120, 32),
                Font = UIFont.SystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Some customer"
            };
            View.AddSubview(customerField);

            var projectLabel = new UILabel()
            {
                Frame = new CGRect(20, 120, 80, 32),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Project"
            };
            View.AddSubview(projectLabel);

            var projectField = new UILabel()
            {
                Frame = new CGRect(100, 120, View.Bounds.Width - 120, 32),
                Font = UIFont.SystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Some project"
            };
            View.AddSubview(projectField);

            var hoursLabel = new UILabel()
            {
                Frame = new CGRect(20, 160, 80, 32),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Hours"
            };
            View.AddSubview(hoursLabel);

            var hoursField = new UIPickerView()
            {
                Frame = new CGRect(100, 160, View.Bounds.Width - 120, 40),
                //KeyboardType = UIKeyboardType.NumberPad,
                //BorderStyle = UITextBorderStyle.RoundedRect,
                //Placeholder = "Hours"
                
            };
            hoursField.Model = new TimesheetEntryHoursViewModel();
            View.AddSubview(hoursField);

            var commentLabel = new UILabel()
            {
                Frame = new CGRect(20, 200, 80, 32),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Comment"
            };
            View.AddSubview(commentLabel);

            var commentField = new UITextField()
            {
                Frame = new CGRect(100, 200, View.Bounds.Width - 120, 32),
                KeyboardType = UIKeyboardType.Default,
                BorderStyle = UITextBorderStyle.RoundedRect,
                Placeholder = ""
            };
            View.AddSubview(commentField);

            var sickLeaveLabel = new UILabel()
            {
                Frame = new CGRect(20, 240, 80, 32),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Sick Leave"
            };
            View.AddSubview(sickLeaveLabel);

            var sickLeaveField = new UISwitch()
            {
                Frame = CGRect.Empty
            };
            sickLeaveField.Transform = CGAffineTransform.MakeScale((nfloat)1, (nfloat)1);
            sickLeaveField.Frame = new CGRect(View.Bounds.Width - sickLeaveField.Frame.Size.Width - 20, 240, sickLeaveField.Frame.Size.Width, sickLeaveField.Frame.Size.Height);
            View.AddSubview(sickLeaveField);

            var submitButton = new UIButton(UIButtonType.System)
            {
                Frame = new CGRect(20, 280, View.Bounds.Width - 40, 32),
            };
            submitButton.SetTitle("Submit", UIControlState.Normal);
            View.AddSubview(submitButton);
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

        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {
            return Hours.ElementAt(Convert.ToInt32(row)).ToString();
        }
    }
}
