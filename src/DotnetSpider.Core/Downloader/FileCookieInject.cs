﻿using System;
using System.IO;
using System.Net;

namespace DotnetSpider.Core.Downloader
{
	/// <summary>
	/// Read cookie from specified file and inject to <see cref="ISpider"/>
	/// It support two formats as followings:
	/// baidu.com
	/// a=b;c=e
	/// 
	/// baidu.com
	/// /
	/// a=b;c=e
	/// </summary>
	/// <summary xml:lang="zh-CN">
	/// 从指定文件中读取Cookie注入到爬虫中, 文件格式支持两种：
	/// baidu.com
	/// a=b;c=e
	/// 
	/// baidu.com
	/// /
	/// a=b;c=e
	/// </summary>
	public class FileCookieInject : CookieInjector
	{
		private readonly string _cookiePath;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <summary xml:lang="zh-CN">
		/// 构造方法
		/// </summary>
		public FileCookieInject()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <summary xml:lang="zh-CN">
		/// 构造方法
		/// </summary>
		/// <param name="path">Cookie文件路径 Cookie File path</param>
		public FileCookieInject(string path)
		{
			if (!File.Exists(path))
			{
				throw new ArgumentException("Cookie file unfound.");
			}
			_cookiePath = path;
		}

		/// <summary>
		/// Obtain new cookies
		/// </summary>
		/// <summary xml:lang="zh-CN">
		/// 取得新的Cookies
		/// </summary>
		/// <param name="spider">爬虫 <see cref="ISpider"/></param>
		/// <returns>Cookies</returns>
		protected override CookieCollection GetCookies(ISpider spider)
		{
			var cookiePath = string.IsNullOrWhiteSpace(_cookiePath) ? $"{spider.Identity}.cookies" : _cookiePath;
			var cookies = new CookieCollection();
			if (File.Exists(cookiePath))
			{
				var datas = File.ReadAllLines(cookiePath);
				if (datas.Length == 2)
				{
					var domain = datas[0];
					var cookiesStr = datas[1];
					var pairs = cookiesStr.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
					foreach (var pair in pairs)
					{
						var keyValue = pair.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
						var name = keyValue[0];
						string value = keyValue.Length > 1 ? keyValue[1] : string.Empty;
						cookies.Add(new Cookie(name, value, "/", domain));
					}

				}
				if (datas.Length == 3)
				{
					var domain = datas[0];
					var path = datas[1];
					var cookiesStr = datas[2];
					var pairs = cookiesStr.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
					foreach (var pair in pairs)
					{
						var keyValue = pair.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
						var name = keyValue[0];
						string value = keyValue.Length > 1 ? keyValue[1] : string.Empty;
						cookies.Add(new Cookie(name, value, path, domain));
					}
				}

			}

			return cookies;
		}
	}
}
