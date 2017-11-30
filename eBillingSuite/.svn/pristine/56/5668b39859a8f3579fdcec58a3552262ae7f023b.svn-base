using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using eBillingSuite.Model;
using Newtonsoft.Json;
using System.IO;


namespace eBillingSuite
{
	public static class Extensions
	{
		public static string ToHoursInJson(this int valueInSeconds)
		{
			return JsonConvert.SerializeObject(Math.Round(TimeSpan.FromSeconds(valueInSeconds).TotalHours, 2));
		}

		public static string ToDataTableLongFormat(this DateTime date)
		{
			return date.ToString("dd/MM/yyyy hh:mm:ss");
		}

		public static string ToDataTableShortFormat(this DateTime date)
		{
			return date.ToString("dd/MM/yyyy");
		}

		public static bool IsElapsed(this DateTime date)
		{
			return date < DateTime.Now.Date;
		}

		public static string ToDataTableLongFormat(this DateTime? date)
		{
			if (date == null)
				return String.Empty;
			else
				return date.GetValueOrDefault().ToDataTableLongFormat();
		}

		public static string ToDataTableShortFormat(this DateTime? date)
		{
			if (date == null)
				return String.Empty;
			else
				return date.GetValueOrDefault().ToDataTableShortFormat();
		}


		public static void DeleteOldLocalGroupByInstance(string instance)
		{

		}
	}
}
