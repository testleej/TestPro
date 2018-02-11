﻿using System;
using DotnetSpider.Core;

namespace DotnetSpider.Extension.Model.Formatter
{
	/// <summary>
	/// Replaces one or more format items in a specified string with the string representation of a specified object.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class StringFormater : Formatter
	{
		/// <summary>
		/// A composite format string.
		/// </summary>
		public string Format { get; set; }

		/// <summary>
		/// 实现数值的转化
		/// </summary>
		/// <param name="value">数值</param>
		/// <returns>被格式化后的数值</returns>
		protected override object FormateValue(object value)
		{
			return string.Format(Format, value.ToString());
		}

		/// <summary>
		/// 校验参数是否设置正确
		/// </summary>
		protected override void CheckArguments()
		{
			if (string.IsNullOrWhiteSpace(Format))
			{
				throw new SpiderException("FormatString should not be null or empty");
			}
		}
	}
}
