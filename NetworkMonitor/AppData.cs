using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Util;

namespace NetworkMonitor
{
	public class AppData
	{
		private TableRow tr = null;
		private Drawable appIcon;
		private double upData, downData, totalDataPerUid, upSpeed, downSpeed, totalSpeedPerUid;
		private string appName, upDataText, downDataText, totalDataPerUidText, upSpeedText, downSpeedText, totalSpeedPerUidText;
		private Context context;
		private TextView upTextView, downTextView, totalTextView;

		public AppData(Context context)
		{
			this.context = context;
			this.tr = new TableRow(this.context);
		}

		public AppData (Context context,string appName, double upData, double downData, double totalDataPerUid, Drawable appIcon = null)
		{
			// constructor
			this.context = context;
			this.tr = new TableRow(this.context);
			this.appName = appName;
			this.appIcon = appIcon;
			this.upData = upData;
			this.downData = downData;
			this.totalDataPerUid = totalDataPerUid;
			this.upSpeed = 0;
			this.downSpeed = 0;
			this.totalSpeedPerUid = 0;
			this.upSpeedText = "";
			this.downSpeedText = "";
			this.totalSpeedPerUidText = "";
		}

		public void setSpeedAndIncrement(double up, double down, double totalPerUid)
		{
			// called if the appName exists in the dictionary
			this.upData  = up;
			this.downData = down;
			this.totalDataPerUid = totalPerUid;

			// getting speeds
			this.upSpeed = up - this.upData;
			this.downSpeed = down - this.downData;
			this.totalSpeedPerUid = totalPerUid - this.totalDataPerUid;

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

			upTextView = new TextView (this.context);
			upTextView.SetPadding (7, 7, 7, 7);
			upTextView.SetText (this.upDataText, TextView.BufferType.Editable);
			tr.AddView (upTextView);

			downTextView = new TextView (this.context);
			downTextView.SetPadding (7, 7, 7, 7);
			downTextView.SetText (this.downDataText, TextView.BufferType.Editable);
			tr.AddView (downTextView);

			totalTextView = new TextView (this.context);
			totalTextView.SetPadding (7, 7, 7, 7);
			totalTextView.SetText (this.totalDataPerUidText, TextView.BufferType.Editable);
			tr.AddView (totalTextView);

			try {
				appDataTable.AddView (tr);
			}catch (Exception e) {
				Console.WriteLine ("{0} Exception caught", e);
			}

			Log.Debug (upSpeedText, downSpeedText);
		}

		public void appendTableRow(TableLayout appDataTable)
		{
			//Assign values to texts
			setTexts ();

			// Append data in existing row

			upTextView.Text = this.upDataText;
			downTextView.Text = this.downDataText;
			totalTextView.Text = this.totalDataPerUidText;

			Log.Debug (upSpeedText, downSpeedText);
		}

		private void setTexts()
		{
			this.downDataText = UnitConverter.unitConversion (this.downData, 0);
			this.upDataText = UnitConverter.unitConversion (this.upData, 0);
			this.totalDataPerUidText = UnitConverter.unitConversion (this.totalDataPerUid, 0);

			this.upSpeedText = UnitConverter.unitConversion (this.upSpeed, 1);
			this.downSpeedText = UnitConverter.unitConversion (this.downSpeed, 1);
			this.totalSpeedPerUidText = UnitConverter.unitConversion (this.totalSpeedPerUid, 1);
		}

		public void setTableHeading(TableLayout appDataTable)
		{
			// set table headings to tablelayout

			TextView o0 = new TextView (this.context);
			o0.SetPadding (7, 7, 7, 7);
			o0.SetText ("AppIcon", TextView.BufferType.Editable);
			tr.AddView (o0);

			TextView o1 = new TextView (this.context);
			o1.SetPadding (7, 7, 7, 7);
			o1.SetText ("AppName", TextView.BufferType.Editable);
			tr.AddView (o1);

			TextView o2 = new TextView (this.context);
			o2.SetPadding (7, 7, 7, 7);
			o2.SetText ("Upload", TextView.BufferType.Editable);
			tr.AddView (o2);

			TextView o3 = new TextView (this.context);
			o3.SetPadding (7, 7, 7, 7);
			o3.SetText ("Download", TextView.BufferType.Editable);
			tr.AddView (o3);

			TextView o4 = new TextView (this.context);
			o4.SetPadding (7, 7, 7, 7);
			o4.SetText ("Total", TextView.BufferType.Editable);
			tr.AddView (o4);

			try {
				appDataTable.AddView (tr);
			}catch (Exception e) {
				Console.WriteLine ("{0} Exception caught", e);
			}
		}
	}
}

