using System;

namespace eBillingSuite.Globalization
{
	public interface ITimeAgoFormatter
	{
		string ToTimeAgo(DateTime value);
	}
}
