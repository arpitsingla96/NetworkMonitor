using System;
using Android.Graphics.Drawables;

namespace NetworkMonitor
{
	public class AppData
	{
		public Drawable appIcon;
		public double upData, downData, totalDataPerUid;
		public AppData (Drawable appIcon, double upData, double downData, double totalDataPerUid)
		{
			this.appIcon = appIcon;
			this.upData = upData;
			this.downData = downData;
			this.totalDataPerUid = totalDataPerUid;
		}

		public void increment(double up, double down, double totalPerUid)
		{
			this.upData += up;
			this.downData += down;
			this.totalDataPerUid += totalPerUid;
		}


	}
}

