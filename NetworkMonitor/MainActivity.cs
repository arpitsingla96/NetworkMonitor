using System;

using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Timers;

namespace NetworkMonitor
{
	[Activity (Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected TableLayout appDataTable;
		public Dictionary<string, object> d = new Dictionary<string, object>();
		private Dictionary<string, List<int>> appNameToUid = new Dictionary<string, List<int>>();
		private const string dirpath = "/proc/uid_stat/";

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			appDataTable = FindViewById<TableLayout> (Resource.Id.appData);

			// Set headings of table
			AppData data = new AppData(this);
			data.setTableHeading(appDataTable);

			createDictionary ();

			mainFunction ();
		}

		public void createDictionary()
		{
			string[] subDirectories = Directory.GetDirectories (dirpath, "*");
			// Getting all the subdirectories /proc/uid_stat/*
			foreach (string subDirectory in subDirectories) {
				// Get uid from the name of each subdirectory
				string uidText = subDirectory.Split ('/') [3];
				int uid = Convert.ToInt32 (uidText);

				List<int> uids = new List<int> ();
				string appName = getAppNameForUid (uid);
				if (!this.appNameToUid.ContainsKey (appName)) {
					uids.Add(uid);
					this.appNameToUid.Add (appName, uids);
				} else {
					uids = (List<int>)appNameToUid [appName];
					uids.Add (uid);
					this.appNameToUid [appName] = uids;
				}
			}

		}

		protected double[] getDataFromUid(int uid)
		{
			// set paths to file
			string downFile = dirpath + uid + "/tcp_rcv";
			string upFile = dirpath + uid + "/tcp_snd";

			// read data from file as strings
			string upDataText = File.ReadAllText (downFile);
			string downDataText = File.ReadAllText (upFile);

			//convert data read from file to double
			double upData = Convert.ToInt64 (upDataText);
			double downData = Convert.ToInt64 (downDataText);
			double totalDataPerUid = upData + downData;

			double[] data = { upData, downData, totalDataPerUid };
			return data;
		}

		public void mainFunction()
		{
			foreach (string appName in this.appNameToUid.Keys) 
			{
				double upData = 0, downData = 0, totalDataPerUid = 0;
				Drawable appIcon = null;
				
				foreach (int uid in this.appNameToUid[appName]) {
					double[] data = getDataFromUid (uid);
					upData = data [0];
					downData = data [1];
					totalDataPerUid = data [2];
					appIcon = getIconForUid (uid);
				}
				// Appending items to view
				setTableRow (appName, upData, downData, totalDataPerUid, appIcon);
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
				if (packagesName == null) {
					appName = "Uninstalled";
				}
				else if (packagesName.Length > 1) {
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
				if (packagesName!=null && packagesName.Length == 1) {
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
				data.setSpeedAndIncrement (upData, downData, totalDataPerUid);
				data.appendTableRow (appDataTable);
			} else {
				// If the dictionary does not contain appName
				data = new AppData (this,appName, upData, downData, totalDataPerUid, appIcon);
				d.Add (appName, data);
				data.addTableRowToTableLayout (appDataTable);
			}
		}

	}
}