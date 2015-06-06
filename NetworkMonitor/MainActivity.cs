using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using System.IO;

namespace NetworkMonitor
{
	[Activity (Label = "NetworkMonitor", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			string dirpath = "/proc/uid_stat";
			string[] subDirectories = Directory.GetDirectories(dirpath,"*");
			foreach (string subDirectory in subDirectories) 
			{
				Log.Debug ("dirname", subDirectory);
			}

		}
	}
}


