using System;

namespace NetworkMonitor
{
	public class UnitConverter
	{
		public UnitConverter ()
		{
		}

		public static string unitConversion (double data)
		{
			string dataText = "";
			if (data < 1024) {
				data = Math.Round (data, 2);
				dataText = data + "B";
			}
			if (data > 1024) {
				data = data / 1024;
				data = Math.Round (data, 2);
				dataText = data + "KB";
			}
			if (data > 1024) {
				data = data / 1024;
				data = Math.Round (data, 2);
				dataText = data + "MB";
			}
			if (data > 1024) {
				data = data / 1024;
				data = Math.Round (data, 2);
				dataText = data + "GB";
			}
			return dataText;
		}
	}
}

