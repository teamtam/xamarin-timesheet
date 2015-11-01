using CoreGraphics;
using UIKit;

namespace TimesheetX.iOS.Code
{
    public partial class ModalPopupController : UIViewController
    {
        public ModalPopupController() : base("ModalPopupController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var modalPopup = new UILabel()
            {
                Frame = new CGRect(20, 20, View.Bounds.Width - 40, View.Bounds.Height - 40),
                Font = UIFont.BoldSystemFontOfSize(12),
                TextAlignment = UITextAlignment.Center,
                Text = "Hello"
            };
            View.AddSubview(modalPopup);
        }
    }
}
