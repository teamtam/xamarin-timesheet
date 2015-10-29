using CoreGraphics;
using UIKit;

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

            this.View.BackgroundColor = UIColor.Cyan;

            var emailEntry = new UITextField()
            {
                Frame = new CGRect(10, 20, View.Bounds.Width - 20, 35),
                KeyboardType = UIKeyboardType.EmailAddress,
                BorderStyle = UITextBorderStyle.RoundedRect,
                Placeholder = "Email  Address"
            };
            View.AddSubview(emailEntry);
        }
    }
}
