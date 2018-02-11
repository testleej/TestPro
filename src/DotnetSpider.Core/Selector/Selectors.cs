using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DotnetSpider.Core.Selector
{
	/// <summary>
	/// ��ѯ���Ĺ���������, ��ͬ�Ĳ�ѯ���Ỻ������.
	/// </summary>
	public class Selectors
	{
		private static readonly Dictionary<string, ISelector> Cache = new Dictionary<string, ISelector>();
		private static readonly DefaultSelector DefaultSelector = new DefaultSelector();

		/// <summary>
		/// ���������ѯ��
		/// </summary>
		/// <param name="expr">������ʽ</param>
		/// <returns>��ѯ��</returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static ISelector Regex(string expr)
		{
			var key = $"r_{expr}";
			if (!Cache.ContainsKey(key))
			{
				Cache.Add(key, new RegexSelector(expr));
			}
			return Cache[key];
		}

		/// <summary>
		/// ���������ѯ��
		/// </summary>
		/// <param name="expr">������ʽ</param>
		/// <param name="group"></param>
		/// <returns>��ѯ��</returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static ISelector Regex(string expr, int group)
		{
			var key = $"r_{expr}_{group}";
			if (!Cache.ContainsKey(key))
			{
				Cache.Add(key, new RegexSelector(expr, group));
			}
			return Cache[key];
		}

		/// <summary>
		/// ����Css��ѯ��
		/// </summary>
		/// <param name="expr">Css���ʽ</param>
		/// <returns>��ѯ��</returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static ISelector Css(string expr)
		{
			var key = $"c_{expr}";
			if (!Cache.ContainsKey(key))
			{
				Cache.Add(key, new CssSelector(expr));
			}
			return Cache[key];
		}

		/// <summary>
		/// ����Css��ѯ��
		/// </summary>
		/// <param name="expr">Css���ʽ</param>
		/// <param name="attrName">��������</param>
		/// <returns>��ѯ��</returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static ISelector Css(string expr, string attrName)
		{
			var key = $"c_{expr}_{attrName}";
			if (!Cache.ContainsKey(key))
			{
				Cache.Add(key, new CssSelector(expr, attrName));
			}
			return Cache[key];
		}

		/// <summary>
		/// ����XPath��ѯ��
		/// </summary>
		/// <param name="expr">Xpath���ʽ</param>
		/// <returns>��ѯ��</returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static ISelector XPath(string expr)
		{
			var key = $"x_{expr}";
			if (!Cache.ContainsKey(key))
			{
				Cache.Add(key, new XPathSelector(expr));
			}
			return Cache[key];
		}

		/// <summary>
		///  �����ղ�ѯ��
		/// </summary>
		/// <returns>��ѯ��</returns>
		public static ISelector Default()
		{
			return DefaultSelector;
		}

		/// <summary>
		/// ������������ֵ��ѯ��
		/// </summary>
		/// <param name="expr">��ֵ</param>
		/// <returns>��ѯ��</returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static ISelector Enviroment(string expr)
		{
			var key = $"e_{expr}";
			if (!Cache.ContainsKey(key))
			{
				Cache.Add(key, new EnviromentSelector(expr));
			}
			return Cache[key];
		}

		/// <summary>
		/// ����JsonPath��ѯ��
		/// </summary>
		/// <param name="expr">JsonPath���ʽ</param>
		/// <returns>��ѯ��</returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static ISelector JsonPath(string expr)
		{
			var key = $"j_{expr}";
			if (!Cache.ContainsKey(key))
			{
				Cache.Add(key, new JsonPathSelector(expr));
			}
			return Cache[key];
		}
	}
}