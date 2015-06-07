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

namespace NetworkMonitor
{
	[Activity (Label = "NetworkMonitor", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		private TableLayout appDataTable;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
			appDataTable = FindViewById<TableLayout>(Resource.Id.appData);

			var tablerow = new TableRow(this) ;
			TextView t0 = new TextView (this);
			t0.SetText ("Icon", TextView.BufferType.Editable);
			TextView t1 = new TextView (this);
			t1.SetText ("Appname", TextView.BufferType.Editable);
			TextView t2 = new TextView (this);
			t2.SetText ("Updata", TextView.BufferType.Editable);
			TextView t3 = new TextView (this);
			t3.SetText ("DownData", TextView.BufferType.Editable);
			TextView t4 = new TextView (this);
			t4.SetText ("TotalData", TextView.BufferType.Editable);
			tablerow.AddView (t0);
			tablerow.AddView (t1);
			tablerow.AddView (t2);
			tablerow.AddView (t3);
			tablerow.AddView (t4);
			try {appDataTable.AddView (tablerow);}
			catch  (Exception e) {Console.WriteLine ("{0} Exception caught", e);}

			const string dirpath = "/proc/uid_stat";
			string[] subDirectories = Directory.GetDirectories(dirpath,"*");
			foreach (string subDirectory in subDirectories) 
			{
				string uidText = subDirectory.Split ('/')[3];
				int uid = Convert.ToInt32 (uidText);

				string downFile = subDirectory + "/tcp_rcv";
				string upFile = subDirectory + "/tcp_snd";

				string upDataText = File.ReadAllText (downFile);
				string downDataText = File.ReadAllText (upFile);
				string totalDataPerUidText = "";

				long upData = Convert.ToInt64 (upDataText);
				long downData = Convert.ToInt64 (downDataText);
				long totalDataPerUid = upData + downData;

				upDataText = unitConversion (upData);
				downDataText = unitConversion (downData);
				totalDataPerUidText = unitConversion (totalDataPerUid);

				Drawable appIcon = Resources.GetDrawable(Resource.Drawable.Icon);

				string appName = "";
				string packageName = "";
				string[] packagesName = { };
				if (uid == 2000 || uid == 1000) {
					appName = "System";
				} else {
					try {
						packagesName = PackageManager.GetPackagesForUid (uid);
					} catch (Exception e) {
						Console.WriteLine ("{0} Exception caught", e);
					}
					if (packagesName!=null && packagesName.Length > 1) {
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
							appIcon = PackageManager.GetApplicationIcon(PackageManager.GetApplicationInfo(packagesName[0],0));								
						} catch (Exception e) {
							Console.WriteLine ("{0} Exception caught", e);
						}
					}
				}
				var tr = new TableRow(this) ;
				ImageView c0 = new ImageView (this);
				c0.SetImageDrawable (appIcon);
				TextView c1 = new TextView (this);
				c1.SetText (appName, TextView.BufferType.Editable);
				TextView c2 = new TextView (this);
				c2.SetText (upDataText, TextView.BufferType.Editable);
				TextView c3 = new TextView (this);
				c3.SetText (downDataText, TextView.BufferType.Editable);
				TextView c4 = new TextView (this);
				c4.SetText (totalDataPerUidText, TextView.BufferType.Editable);
				tr.AddView (c0);
				tr.AddView (c1);
				tr.AddView (c2);
				tr.AddView (c3);
				tr.AddView (c4);
				try {appDataTable.AddView (tr);}
				catch  (Exception e) {Console.WriteLine ("{0} Exception caught", e);}

				Log.Debug (uidText, appName);
			}

		}

		public string unitConversion (long data)
		{
			string dataText = "";
			if (data < 1024) {
				dataText = data + "B";
			}
			if (data > 1024) {
				data = data / 1024;
				dataText = data + "KB";
			}
			if (data > 1024) {
				data = data / 1024;
				dataText = data + "MB";
			}
			if (data > 1024) {
				data = data / 1024;
				dataText = data + "GB";
			}
			return dataText;
		}
	}
}


