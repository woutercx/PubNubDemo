using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PubNubMessaging.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using MonoTouch.Dialog;

namespace PubNubDemo
{
	public partial class PubNubDemoViewController : UIViewController
	{
		private object _timestampLock = new object();
		private object _logLock = new object();
		Pubnub _pubnub;
		const string CHANNELNAME = "pubnubwoutercxdemo";
		const string PUBLISH_KEY = "pub-c-4746ca37-3480-427a-891f-fb01dbcd388b";
		const string SUBSCRIBE_KEY = "sub-c-cf6399f0-ba1c-11e2-89ba-02ee2ddab7fe";
		const string SECRET_KEY = "";
		private bool _subscribed;
		private TableSource _tableSource;

		List<string> _tableItems = new List<string>();

		public PubNubDemoViewController () : base ("PubNubDemoViewController", null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			//tblItems.Source = new TableSource(NavigationController, results.ToArray());
			_tableSource = new TableSource(NavigationController, _tableItems.ToArray());
			tblItems.Source = _tableSource;
			_subscribed = false;
			SetShouldReturnOnTextFields();
			tblItems.ScrollEnabled = true;
			tblItems.ShowsVerticalScrollIndicator = true;
			//RectangleF rf = tblItems.Frame;
			//tblItems.Frame = new RectangleF(rf.X,rf.Y,rf.Width,600);
			//tblItems.s
			//CGRect cgRct = CGRectMake(0, 0, 320, 600);          
			//yourTable = [[UITableView alloc] initWithFrame:cgRct style:UITableViewStyleGrouped];

			this.btnSubscribe.TouchUpInside += btnSubscribe_Click;
			this.btnUnsubscribe.TouchUpInside += btnUnSubscribe_Click;
			this.btnPublish.TouchUpInside += btnPublish_Click;

			ToggleButtonPublish(false);
			// Perform any additional setup after loading the view, typically from a nib.

			//Log(TimeStamp());

			InitPubNub();

			//HereNow(); // Only works when you've enabled "Channel presence" in the Admin portal
			//History(); // Only works when you've enabled "Storage & Playback" in the Admin portal
		}

	    //http://www.codeproject.com/Articles/432078/An-Introduction-and-Thoughts-on-Developing-iOS-App
		public void SetTableItems(List<string> results)
		{
			_tableSource.SetTableItems = results.ToArray();

			tblItems.ReloadData();
		}

		void btnSubscribe_Click (object sender, EventArgs e)
		{
			Subscribe();
			_subscribed = true;
			ToggleButtonPublish(true);
		}

		void btnUnSubscribe_Click (object sender, EventArgs e)
		{
			Unsubscribe();
			_subscribed = false;
			ToggleButtonPublish(false);
		}

		void btnPublish_Click (object sender, EventArgs e)
		{
			Publish(txtMessage.Text);
		}

		//- Set the button to disabled by setting Enabled property.
		//- Make the button turn grey if it is disabled.
		void ToggleButtonPublish(bool enable)
		{
			this.btnPublish.SetTitleColor(UIColor.Gray, UIControlState.Disabled);
			this.btnPublish.Enabled = enable;
		}

		void InitPubNub()
		{
			Log("Running Init()");

			_pubnub = new Pubnub(
				PUBLISH_KEY,
				SUBSCRIBE_KEY,
				SECRET_KEY
				);
		}

		void Subscribe()
		{
			//_tableSource = new TableSource(NavigationController, _tableItems.ToArray());
			//tblItems.Source = _tableSource;


			//Log("Running Subscribe()");

//			_pubnub.Subscribe<string>(CHANNELNAME, 
//			                          DisplayReturnMessageSubscribe, 
//			                          DisplayConnectStatusMessageSubscribe);


			try
			{
				_pubnub.Subscribe<string>(CHANNELNAME, 
				                          DisplayReturnMessageSubscribe, 
				                          DisplayConnectStatusMessageSubscribe);
			}
			catch(Exception exc)
			{
				Console.WriteLine(exc.Message);
				Console.WriteLine(exc.StackTrace);
				if(exc.InnerException != null)
				{
					Console.WriteLine(exc.InnerException.Message);
					Console.WriteLine(exc.InnerException.StackTrace);
				}
			}
		}

		private void DisplayReturnMessageSubscribe(string result)
		{
			Log(TimeStamp());
			Log("Subscribe result:");
			Log(result);
		}

		private void DisplayConnectStatusMessageSubscribe(string result)
		{
			Log(TimeStamp());
			Log("ConnectStatus subscribe result:");
			Log(result);
		}

		void Publish(string message)
		{
			Log("Running Publish()");
			Log(DateTime.Now.ToLongTimeString());

			_pubnub.Publish<string>(CHANNELNAME, message, DisplayReturnMessagePublish);
		}

		/// <summary>
		/// Callback method captures the response in JSON string format for all operations
		/// </summary>
		/// <param name="result"></param>
	    void DisplayReturnMessagePublish(string result)
		{
			Log(TimeStamp());
			Log("Publish result:");
			Log(result);
		}

		/// <summary>
		/// Enable "Storage & Playback" in the admin portal to make this work.
		/// </summary>
		void History()
		{
			Log("Running History()");

			_pubnub.DetailedHistory<string>(CHANNELNAME, 10, DisplayReturnMessageHistory);
		}

		private  void DisplayReturnMessageHistory(string result)
		{
			Log(TimeStamp());
			Log("History result:");
			Log(result);
		}

		/// <summary>
		/// Enable "Channel Presence" in the admin portal to make this work.
		/// </summary>
		 void HereNow()
		{
			Log("Running HereNow()");

			_pubnub.HereNow<string>(CHANNELNAME, DisplayReturnMessageHereNow);
		}

		private void DisplayReturnMessageHereNow(string result)
		{
			Log(TimeStamp());
			Log("HereNow result:");
			Log(result);
		}

		 void Unsubscribe()
		{
			Log("Running unsubscribe()");
			_pubnub.Unsubscribe<string>(CHANNELNAME, 
			                            DisplayReturnMessageUnsubscribe, 
			                            DisplayConnectStatusMessageUnsubscribe, 
			                            DisplayDisconnectStatusMessageUnsubscribe);

			ToggleButtonPublish(false);
		}

		private void DisplayReturnMessageUnsubscribe(string result)
		{
			Log(TimeStamp());
			Log("Return Message Unsubscribe result:");
			Log(result);
		}

		private void DisplayConnectStatusMessageUnsubscribe(string result)
		{
			Log(TimeStamp());
			Log("Connectstatus Unsubscribe result:");
			Log(result);
		}

		private void DisplayDisconnectStatusMessageUnsubscribe(string result)
		{
			Log(TimeStamp());
			Log("Disconnectstatus Unsubscribe result:");
			Log(result);
		}

		string TimeStamp()
		{
			//Custom Date and Time Format Strings
			//http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx

			lock (_timestampLock)
			{
				return "\r\n" + DateTime.Now.ToString("H:mm:ss.ffff");
			}
		}

		/// <summary>
		/// If the user is entering text in a textfield with the IPhone keyboard,
		/// and presses [Enter], the keyboard should be dismissed.
		/// </summary>
		private void SetShouldReturnOnTextFields ()
		{
			//http://www.rqna.net/qna/iusnxz-how-can-you-search-a-uiviewcontroller-for-number-of-uitextfields.html
			foreach (UIView subview in this.View.Subviews)
			{
				if(subview is UITextField)
				{
					//((UITextView)subview).ShouldReturn = DismissKeyboard;
					((UITextField)subview).ShouldReturn = DismissKeyboard;
				}
			}
		}

		/// <summary>
		/// Dismisses the keyboard when the user taps the [enter] key on the keyboard
		/// </summary>
		/// <returns><c>true</c>, if keyboard was dismissed, <c>false</c> otherwise.</returns>
		/// <param name="textField">Text field.</param>
		bool DismissKeyboard (UITextField textField)
		{
			textField.ResignFirstResponder();
			return true;
		}

		public void Log(string message)
		{
			lock(_logLock)
			{
				InvokeOnMainThread (delegate { 
					_tableItems.Add(message);
					SetTableItems(_tableItems);
				} );
			}
		}

		#region Apple methods

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

//		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
//		{
//			// Return true for supported orientations
//			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
//		}

		#endregion Apple methods
	}

	public class TextViewDelegate : UITextViewDelegate
	{
		public override bool ShouldChangeText (UITextView textView, NSRange range, string text)
		{
			if(!text.Contains ( Environment.NewLine ))
			{
				return true;
			}

			textView.ResignFirstResponder();

			return false;
		}
	}

	public class TableSource : UITableViewSource
	{
		private string[] _tableItems;
		private static readonly string _cellIdentifier = "TableCell";
		private UINavigationController _navigationController;

		public TableSource(UINavigationController navigationController, string[] items)
		{
			_tableItems = items;
			_navigationController = navigationController;
		}

		public string[] SetTableItems
		{
			get
			{
				return _tableItems;
			}
			set
			{
				_tableItems = value;
				
			}
		}

		public override int RowsInSection(UITableView tableview, int section)
		{
			return _tableItems.Length;
		}

		public override UITableViewCell GetCell(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(_cellIdentifier);
			// if there are no cells to reuse, create a new one
			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Subtitle, _cellIdentifier);
			}
			var text = _tableItems[indexPath.Row];
			cell.TextLabel.Text = text;
			//cell.DetailTextLabel.Text = tweet.Title;
			//cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			//cell.ImageView.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(tweet.ProfileImageUrl)));
			return cell;
		}
	}
}

//			var context = TaskScheduler.FromCurrentSynchronizationContext ();
//
//			Task.Factory.StartNew (() => {
//				//DoSomeExpensiveTask();
//				return "Hi Mom";
//			}).ContinueWith (t => { 
//				txtOutput.Text = message;
//				//DoSomethingInUI(t.Result);             
//			}, context);

//			InvokeOnMainThread (delegate { 
//				txtOutput.Text = txtOutput.Text + message + Environment.NewLine;
//			} );

//			UIApplication.SharedApplication.InvokeOnMainThread(
//
//
//				UIApplication.SharedApplication.InvokeOnMainThread(() => {
//						txtOutput.Text = message;
//				});

//		Task.Factory.StartNew (
//			// tasks allow you to use the lambda syntax to pass work
//			() => {
//			//Thread.Sleep(3000);
//			var response = CloudService.Client.KlantRegistreren(bsn, deviceId, platform);
//			isRegistered = response.Geregistreerd;
//			errorMessages = response.Errors;
//
//		}
//		// ContinueWith allows you to specify an action that runs after the previous thread
//		// completes
//		// 
//		// By using TaskScheduler.FromCurrentSyncrhonizationContext, we can make sure that 
//		// this task now runs on the original calling thread, in this case the UI thread
//		// so that any UI updates are safe. in this example, we want to hide our overlay, 
//		// but we don't want to update the UI from a background thread.
//		).ContinueWith ( 
//		                t => {
//			loadingOverlay.Hide ();
//			//				if(errorMessages != null && errorMessages.Any()) {
//			//					lblRegistrationErrorField.Text = "Helaas, registratie ging niet goed....De fouten zijn:\n" + 
//			//						string.Join("\n", errorMessages);
//			//				}
//			//				else {
//			//					lblRegistrationErrorField.Text = "het ging goed! De klant is " + (isRegistered ? "wel" : "niet") + " geregistreerd.";
//			//					
//			//				}
//			this.NavigationController.PushViewController(new RegistrationCompleted(), true);
//		}, TaskScheduler.FromCurrentSynchronizationContext()
//		);