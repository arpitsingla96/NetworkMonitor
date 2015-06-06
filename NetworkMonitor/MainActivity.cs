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
				string uidText = subDirectory.Split ('/')[3];
				int uid = Convert.ToInt32 (uidText);

				string downFile = subDirectory + "/tcp_rcv";
				string upFile = subDirectory + "/tcp_snd";

				string upDataText = File.ReadAllText (downFile);
				string downDataText = File.ReadAllText (upFile);

				long upData = Convert.ToInt64 (upDataText);
				long downData = Convert.ToInt64 (downDataText);
				long totalDataPerUid = upData + downData;

				string[] packagesName = PackageManager.GetPackagesForUid (uid);
				foreach(string packageName in packagesName)
				{
					string appName = "";
					try
					{
						appName = PackageManager.GetApplicationLabel(PackageManager.GetApplicationInfo (packageName, 0));
					}
					catch (Exception e)
					{
						Console.WriteLine ("{0} Exception caught", e);
					}
					Log.Debug ("uid", uidText);
					Log.Debug ("appname", appName);
				}

			}

		}
	}
}


