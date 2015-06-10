using System;

namespace NetworkMonitor
{
	public class UnitConverter
	{
		public UnitConverter ()
		{
		}

		public static string unitConversion (double data, int type)
		{
			// type = 0 means data conversion
			// type = 1 means unit conversion

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
			if (type == 1) {
				dataText = dataText + "/s" ;
			}
			return dataText;
		}
	}
}

