﻿using eBillingSuite.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Globalization
{
	public class TimeAgoFormatter : eBillingSuite.Globalization.ITimeAgoFormatter
	{
		const int Hour = 1;
		const int HoursPerDay = 24 * Hour;
		const int HoursPerMonth = 30 * HoursPerDay;

		private IeBillingSuiteRequestContext _context;

		public TimeAgoFormatter(IeBillingSuiteRequestContext context)
		{
			_context = context;
		}

		public string ToTimeAgo(DateTime value)
		{
			var ts = DateTime.Now.Subtract(value);
			var delta = ts.TotalHours;

			if (delta < 0)
			{
				return Texts.TimeAgoNotYet;
			}
			if (delta < 1 * Hour)
			{
				return Texts.TimeAgoLessThanAnHour;
			}
			if (delta < 2 * Hour)
			{
				return Texts.TimeAgoAnHour;
			}
			if (delta < 24 * Hour)
			{
				return ts.Hours + Texts.TimeAgoHours;
			}
			if (delta < 48 * Hour)
			{
				return Texts.TimeAgoYesterday;
			}
			if (delta < 30 * HoursPerDay)
			{
				return ts.Days + Texts.TimeAgoDays;
			}
			if (delta < 12 * HoursPerMonth)
			{
				int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
				return months <= 1 ? Texts.TimeAgoOneMonth : months + Texts.TimeAgoMonths;
			}
			else
			{
				int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
				return years <= 1 ? Texts.TimeAgoOneYear : years + Texts.TimeAgoYears;
			}
		}
	}
}
