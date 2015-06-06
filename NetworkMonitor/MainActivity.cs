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
				string downFile = subDirectory + "/tcp_rcv";
				string upFile = subDirectory + "/tcp_snd";
				string upDataText = File.ReadAllText (downFile);
				string downDataText = File.ReadAllText (upFile);
				Log.Debug ("updatatext", upDataText);
			}

		}
	}
}


