using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics.Drawables;
using Android.Util;
using System.IO;
using System.Collections.Generic;

namespace NetworkMonitor
{
	[Activity (Label = "NetworkMonitor", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		private static int id = 1;
		protected TableLayout appDataTable;
		public Dictionary<string, object> d = new Dictionary<string, object>();


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			appDataTable = FindViewById<TableLayout> (Resource.Id.appData);

			// Set headings of table
			AppData data = new AppData(this,0, "",0,0,0);
			data.setTableHeading(appDataTable);

			mainFunction ();
		}

		public void mainFunction()
		{
			const string dirpath = "/proc/uid_stat";
			// Getting all the subdirectories /proc/uid_stat/*
			string[] subDirectories = Directory.GetDirectories (dirpath, "*");
			foreach (string subDirectory in subDirectories) {
				// Get uid from the name of each subdirectory
				string uidText = subDirectory.Split ('/') [3];
				int uid = Convert.ToInt32 (uidText);

				// Reading files which contain data in each subdirectory
				string downFile = subDirectory + "/tcp_rcv";
				string upFile = subDirectory + "/tcp_snd";

				string upDataText = File.ReadAllText (downFile);
				string downDataText = File.ReadAllText (upFile);

				double upData = Convert.ToInt64 (upDataText);
				double downData = Convert.ToInt64 (downDataText);
				double totalDataPerUid = upData + downData;

				// Get appName and appIcon for each uid
				string appName = getAppNameForUid (uid);
				Drawable appIcon = getIconForUid (uid);

				// Appending items to view
				setTableRow (appName, upData, downData, totalDataPerUid, appIcon);

				Log.Debug (uidText, appName);
			}
		}

		public string getAppNameForUid (int uid)
		{
			string appName = "";
			string packageName = "";
			string[] packagesName = { };
			if (uid == 1000 || uid==2000) {
				appName = "System";
			}else {
				try {
					packagesName = PackageManager.GetPackagesForUid (uid);
				} catch (Exception e) {
					Console.WriteLine ("{0} Exception caught", e);
				}
				if (packagesName != null && packagesName.Length > 1) {
					try {
						packageName = PackageManager.GetNameForUid (uid);
					} catch (Exception e) {
						Console.WriteLine ("{0} Exception caught", e);
					}
					if (packageName.Split (':') [0] == "android.media") {
						appName = "Media";
					} else if (packageName.Split (':') [0] == "com.google.android.calendar.uid.shared") {
						appName = "Calender";
					} else if (packageName.Split (':') [0] == "com.google.uid.shared") {
						appName = "Contacts";
					} else {
						appName = "System";
					}
				} else {
					try {
						appName = PackageManager.GetApplicationLabel (PackageManager.GetApplicationInfo (packagesName [0], 0));
					} catch (Exception e) {
						Console.WriteLine ("{0} Exception caught", e);
					}
				}
			}
			return appName;
		}

		public Drawable getIconForUid (int uid)
		{
			string[] packagesName = { };
			Drawable appIcon = Resources.GetDrawable (Resource.Drawable.Icon);
			if (uid != 1000 && uid != 2000) {
				try {
					packagesName = PackageManager.GetPackagesForUid (uid);
				} catch (Exception e) {
					Console.WriteLine ("{0} Exception caught", e);
				}
				if (packagesName.Length == 1) {
					try {
						appIcon = PackageManager.GetApplicationIcon (PackageManager.GetApplicationInfo (packagesName [0], 0));
					} catch (Exception e) {
						Console.WriteLine ("{0} Exception caught", e);
					}
				}
			}
			return appIcon;
		}

		public void setTableRow (string appName = "AppName", double upData = 0, double downData = 0, double totalDataPerUid = 0, Drawable appIcon = null)
		{
			AppData data;
			if (d.ContainsKey(appName)) {
				// If the dictionary contains appName
				data = (AppData)d [appName];
				data.increment (upData, downData, totalDataPerUid);
				data.appendTableRow (appDataTable);
			} else {
				// If the dictionary does not contain appName
				data = new AppData (this, id, appName, upData, downData, totalDataPerUid, appIcon);
				d.Add (appName, data);
				data.addTableRowToTableLayout (appDataTable);
				id++;
			}
		}

	}
}