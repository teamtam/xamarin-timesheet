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

            CreateDateLabel();
            CreateDateField();
            CreateCustomerLabel();
            CreateCustomerField();
            CreateProjectLabel();
            CreateProjectField();
            CreateHoursLabel();
            CreateHoursField();
            CreateCommentLabel();
            CreateCommentField();
            CreateSickLeaveLabel();
            CreateSickLeaveField();
            CreateSubmitButton();
        }

        private void CreateDateLabel()
        {
            var dateLabel = new UILabel()
            {
                Frame = new CGRect(20, 40, 80, 32),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Date"
            };
            View.AddSubview(dateLabel);
        }

        private void CreateDateField()
        {
            var dateField = new UILabel()
            {
                Frame = new CGRect(100, 40, View.Bounds.Width - 120, 32),
                Font = UIFont.SystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "10/10/2015"
            };
            View.AddSubview(dateField);
        }

        private void CreateCustomerLabel()
        {
            var customerLabel = new UILabel()
            {
                Frame = new CGRect(20, 80, 80, 32),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Customer"
            };
            View.AddSubview(customerLabel);
        }

        private void CreateCustomerField()
        {
            var customerField = new UILabel()
            {
                Frame = new CGRect(100, 80, View.Bounds.Width - 120, 32),
                Font = UIFont.SystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Some customer"
            };
            View.AddSubview(customerField);
        }

        private void CreateProjectLabel()
        {
            var projectLabel = new UILabel()
            {
                Frame = new CGRect(20, 120, 80, 32),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Project"
            };
            View.AddSubview(projectLabel);
        }

        private void CreateProjectField()
        {
            var projectField = new UILabel()
            {
                Frame = new CGRect(100, 120, View.Bounds.Width - 120, 32),
                Font = UIFont.SystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Some project"
            };
            View.AddSubview(projectField);
        }

        private void CreateHoursLabel()
        {
            var hoursLabel = new UILabel()
            {
                Frame = new CGRect(20, 160, 80, 32),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Hours"
            };
            View.AddSubview(hoursLabel);
        }

        private void CreateHoursField()
        {
            var hoursField = new UIPickerView()
            {
                Frame = new CGRect(100, 160, View.Bounds.Width - 120, 40),
            };
            hoursField.Model = new TimesheetEntryHoursViewModel();
            hoursField.Select(32, 0, true);
            View.AddSubview(hoursField);
        }

        private void CreateCommentLabel()
        {
            var commentLabel = new UILabel()
            {
                Frame = new CGRect(20, 200, 80, 32),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Comment"
            };
            View.AddSubview(commentLabel);
        }

        private void CreateCommentField()
        {
            var commentField = new UITextField()
            {
                Frame = new CGRect(100, 200, View.Bounds.Width - 120, 32),
                KeyboardType = UIKeyboardType.Default,
                BorderStyle = UITextBorderStyle.RoundedRect,
                Placeholder = ""
            };
            View.AddSubview(commentField);
        }

        private void CreateSickLeaveLabel()
        {
            var sickLeaveLabel = new UILabel()
            {
                Frame = new CGRect(20, 240, 80, 32),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Left,
                Text = "Sick Leave"
            };
            View.AddSubview(sickLeaveLabel);
        }

        private void CreateSickLeaveField()
        {
            var sickLeaveField = new UISwitch()
            {
                Frame = CGRect.Empty
            };
            sickLeaveField.Transform = CGAffineTransform.MakeScale((nfloat)1, (nfloat)1);
            sickLeaveField.Frame = new CGRect(View.Bounds.Width - sickLeaveField.Frame.Size.Width - 20, 240, sickLeaveField.Frame.Size.Width, sickLeaveField.Frame.Size.Height);
            View.AddSubview(sickLeaveField);
        }

        private void CreateSubmitButton()
        {
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
    }
}
