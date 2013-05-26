// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace PubNubDemo
{
	[Register ("PubNubDemoViewController")]
	partial class PubNubDemoViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextField txtMessage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnPublish { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnSubscribe { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnUnsubscribe { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextView txtOutput { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableView tblItems { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (txtMessage != null) {
				txtMessage.Dispose ();
				txtMessage = null;
			}

			if (btnPublish != null) {
				btnPublish.Dispose ();
				btnPublish = null;
			}

			if (btnSubscribe != null) {
				btnSubscribe.Dispose ();
				btnSubscribe = null;
			}

			if (btnUnsubscribe != null) {
				btnUnsubscribe.Dispose ();
				btnUnsubscribe = null;
			}

			if (txtOutput != null) {
				txtOutput.Dispose ();
				txtOutput = null;
			}

			if (tblItems != null) {
				tblItems.Dispose ();
				tblItems = null;
			}
		}
	}
}
