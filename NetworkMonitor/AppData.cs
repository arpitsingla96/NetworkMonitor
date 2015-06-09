using System;

using Android.Graphics.Drawables;
using Android.Widget;
using Android.Content;

namespace NetworkMonitor
{
	public class AppData
	{
		private TableRow tr = null;
		private Drawable appIcon;
		private double upData, downData, totalDataPerUid;
		private string upDataText, downDataText, totalDataPerUidText, appName;
		private Context context;
		public AppData (Context context, string appName, Drawable appIcon, double upData, double downData, double totalDataPerUid)
		{
			this.context = context;
			this.tr = new TableRow(this.context);
			this.appName = appName;
			this.appIcon = appIcon;
			this.upData = upData;
			this.downData = downData;
			this.totalDataPerUid = totalDataPerUid;
			setTexts ();
		}

		public void increment(double up, double down, double totalPerUid)
		{
			this.upData += up;
			this.downData += down;
			this.totalDataPerUid += totalPerUid;
		}

		public void setTableRowToTableLayout(TableLayout appDataTable)
		{
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
			tr.AddView (c2);

			TextView c3 = new TextView (this.context);
			c3.SetPadding (7, 7, 7, 7);
			c3.SetText (this.downDataText, TextView.BufferType.Editable);
			tr.AddView (c3);

			TextView c4 = new TextView (this.context);
			c4.SetPadding (7, 7, 7, 7);
			c4.SetText (this.totalDataPerUidText, TextView.BufferType.Editable);
			tr.AddView (c4);

			try {
				appDataTable.AddView (tr);
			}catch (Exception e) {
				Console.WriteLine ("{0} Exception caught", e);
			}
		}

		private void setTexts()
		{
			this.downDataText = UnitConverter.unitConversion (this.downData);
			this.upDataText = UnitConverter.unitConversion (this.upData);
			this.totalDataPerUidText = UnitConverter.unitConversion (this.totalDataPerUid);
		}

	}
}

