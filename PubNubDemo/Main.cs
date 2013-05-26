using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace PubNubDemo
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			try
			{
				// if you want to use a different Application Delegate class from "AppDelegate"
				// you can specify it here.
				UIApplication.Main (args, null, "AppDelegate");
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
	}
}
