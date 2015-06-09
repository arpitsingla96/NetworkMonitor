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
	public class AppData : Activity
	{
		private int upId, downId, totalId;
		private TableRow tr = null;
		private Drawable appIcon;
		private double upData, downData, totalDataPerUid;
		private string upDataText, downDataText, totalDataPerUidText, appName;
		private Context context;
		public AppData()
		{
		}

		public AppData (Context context,int id, string appName, double upData, double downData, double totalDataPerUid, Drawable appIcon = null)
		{
			// constructor
			this.context = context;
			this.tr = new TableRow(this.context);
			this.appName = appName;
			this.appIcon = appIcon;
			this.upData = upData;
			this.downData = downData;
			this.totalDataPerUid = totalDataPerUid;
			this.upId = id + 100;
			this.downId = id + 200;
			this.totalId = id + 300;
		}

		public void increment(double up, double down, double totalPerUid)
		{
			// called if the appName exists in the dictionary
			this.upData += up;
			this.downData += down;
			this.totalDataPerUid += totalPerUid;
		}

		public void addTableRowToTableLayout(TableLayout appDataTable)
		{
			//Assign values to texts
			setTexts ();

			// Add new row in table view

			ImageView c0 = new ImageView (this.context);
			c0.SetPadding (7, 7, 7, 7);
			c0.SetImageDrawable (this.appIcon);
			tr.AddView (c0);

			TextView c1 = new TextView (this.context);
			c1.SetPadding (7, 7, 7, 7);
			c1.TextSize = 20;
			c1.SetWidth (225);
			c1.SetText (this.appName, TextView.BufferType.Editable);
			tr.AddView (c1);

			TextView c2 = new TextView (this.context);
			c2.SetPadding (7, 7, 7, 7);
			c2.SetText (this.upDataText, TextView.BufferType.Editable);
			c2.Id = this.upId;
			tr.AddView (c2);

			TextView c3 = new TextView (this.context);
			c3.SetPadding (7, 7, 7, 7);
			c3.SetText (this.downDataText, TextView.BufferType.Editable);
			c3.Id = this.downId;
			tr.AddView (c3);

			TextView c4 = new TextView (this.context);
			c4.SetPadding (7, 7, 7, 7);
			c4.SetText (this.totalDataPerUidText, TextView.BufferType.Editable);
			c4.Id = this.totalId;
			tr.AddView (c4);

			try {
				appDataTable.AddView (tr);
			}catch (Exception e) {
				Console.WriteLine ("{0} Exception caught", e);
			}
		}

		public void appendTableRow(TableLayout appDataTable)
		{
			//Assign values to texts
			setTexts ();

			// Append data in existing row

			TextView c2 = FindViewById<TextView> (this.upId);
			c2.SetText (this.upDataText, TextView.BufferType.Editable);
			tr.AddView (c2);

			TextView c3 = FindViewById<TextView> (this.upId);
			c3.SetText (this.downDataText, TextView.BufferType.Editable);
			tr.AddView (c3);

			TextView c4 = FindViewById<TextView> (this.upId);
			c4.SetText (this.totalDataPerUidText, TextView.BufferType.Editable);
			tr.AddView (c4);

		}

		private void setTexts()
		{
			this.downDataText = UnitConverter.unitConversion (this.downData);
			this.upDataText = UnitConverter.unitConversion (this.upData);
			this.totalDataPerUidText = UnitConverter.unitConversion (this.totalDataPerUid);
		}

		public void setTableHeading(TableLayout appDataTable)
		{
			// set table headings to tablelayout

			TextView c0 = new TextView (this.context);
			c0.SetPadding (7, 7, 7, 7);
			c0.SetText ("AppIcon", TextView.BufferType.Editable);
			tr.AddView (c0);

			TextView c1 = new TextView (this.context);
			c1.SetPadding (7, 7, 7, 7);
			c1.SetText ("AppName", TextView.BufferType.Editable);
			tr.AddView (c1);

			TextView c2 = new TextView (this.context);
			c2.SetPadding (7, 7, 7, 7);
			c2.SetText ("Upload", TextView.BufferType.Editable);
			tr.AddView (c2);

			TextView c3 = new TextView (this.context);
			c3.SetPadding (7, 7, 7, 7);
			c3.SetText ("Download", TextView.BufferType.Editable);
			tr.AddView (c3);

			TextView c4 = new TextView (this.context);
			c4.SetPadding (7, 7, 7, 7);
			c4.SetText ("Total", TextView.BufferType.Editable);
			tr.AddView (c4);

			try {
				appDataTable.AddView (tr);
			}catch (Exception e) {
				Console.WriteLine ("{0} Exception caught", e);
			}
		}
	}
}

