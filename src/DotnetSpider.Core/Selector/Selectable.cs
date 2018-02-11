using System;
using System.Collections.Generic;
using DotnetSpider.Core.Infrastructure;
using HtmlAgilityPack;

namespace DotnetSpider.Core.Selector
{
	/// <summary>
	/// ��ѯ�ӿ�
	/// </summary>
	public class Selectable : AbstractSelectable
	{
		/// <summary>
		/// ���췽��
		/// </summary>
		/// <param name="text">����ѯ���ı�</param>
		/// <param name="urlOrPadding">URL���·���������Json padding��ȥ��</param>
		/// <param name="contentType">�ı����ݸ�ʽ: Html, Json</param>
		/// <param name="domain">����, ����ȥ������</param>
		public Selectable(string text, string urlOrPadding, ContentType contentType, params string[] domain)
		{
			switch (contentType)
			{
				case ContentType.Html:
					{
						HtmlDocument document = new HtmlDocument { OptionAutoCloseOnEnd = true };
						document.LoadHtml(text);

						if (!string.IsNullOrEmpty(urlOrPadding))
						{
							FixAllRelativeHrefs(document, urlOrPadding);
						}

						if (domain != null && domain.Length > 0)
						{
							RemoveOutboundLinks(document, domain);
						}

						Elements = new List<dynamic> { document.DocumentNode.OuterHtml };
						break;
					}
				case ContentType.Json:
					{
						string json = string.IsNullOrEmpty(urlOrPadding) ? text : RemovePadding(text, urlOrPadding);
						Elements = new List<dynamic> { json };
						break;
					}
			}
		}

		/// <summary>
		/// ���췽��
		/// </summary>
		/// <param name="nodes">����ѯ��Ԫ��</param>
		public Selectable(List<dynamic> nodes)
		{
			Elements = nodes;
		}

		/// <summary>
		/// ͨ��Css ѡ�������ҽ��
		/// </summary>
		/// <param name="css">Css ѡ����</param>
		/// <returns>��ѯ�ӿ�</returns>
		public override ISelectable Css(string css)
		{
			return Select(Selectors.Css(css));
		}

		/// <summary>
		/// ͨ��Css ѡ��������Ԫ��, ��ȡ�����Ե�ֵ
		/// </summary>
		/// <param name="css">Css ѡ����</param>
		/// <param name="attrName">��ѯ����Ԫ�ص�����</param>
		/// <returns>��ѯ�ӿ�</returns>
		public override ISelectable Css(string css, string attrName)
		{
			var cssSelector = Selectors.Css(css, attrName);
			return Select(cssSelector);
		}

		/// <summary>
		/// �������е�����
		/// </summary>
		/// <returns>��ѯ�ӿ�</returns>
		public override ISelectable Links()
		{
			var tmplinks = XPath("./descendant-or-self::a/@href").GetValues();
			var links = new List<dynamic>();
			foreach (var link in tmplinks)
			{
				if (Uri.TryCreate(link, UriKind.RelativeOrAbsolute, out _))
				{
					links.Add(link);
				}
			}
			return new Selectable(links);
		}

		/// <summary>
		/// ͨ��XPath���ҽ��
		/// </summary>
		/// <param name="xpath">XPath ���ʽ</param>
		/// <returns>��ѯ�ӿ�</returns>
		public override ISelectable XPath(string xpath)
		{
			return SelectList(Selectors.XPath(xpath));
		}

		/// <summary>
		/// ͨ����ѯ�����ҽ��
		/// </summary>
		/// <param name="selector">��ѯ��</param>
		/// <returns>��ѯ�ӿ�</returns>
		public override ISelectable Select(ISelector selector)
		{
			if (selector != null)
			{
				List<dynamic> resluts = new List<dynamic>();
				foreach (var selectedNode in Elements)
				{
					var result = selector.Select(selectedNode);
					if (result != null)
					{
						resluts.Add(result);
					}
				}
				return new Selectable(resluts);
			}
			throw new SpiderException("Selector is null");
		}

		/// <summary>
		/// ͨ����ѯ�����ҽ��
		/// </summary>
		/// <param name="selector">��ѯ��</param>
		/// <returns>��ѯ�ӿ�</returns>
		public override ISelectable SelectList(ISelector selector)
		{
			if (selector != null)
			{
				List<dynamic> resluts = new List<dynamic>();
				foreach (var selectedNode in Elements)
				{
					var result = selector.SelectList(selectedNode);
					if (result != null)
					{
						resluts.AddRange(result);
					}
				}
				return new Selectable(resluts);
			}

			throw new SpiderException("Selector is null");
		}

		/// <summary>
		/// ȡ�ò�ѯ�������еĽ��
		/// </summary>
		/// <returns>��ѯ�ӿ�</returns>
		public override IEnumerable<ISelectable> Nodes()
		{
			List<ISelectable> reslut = new List<ISelectable>();
			foreach (var element in Elements)
			{
				reslut.Add(new Selectable(new List<dynamic>() { element }));
			}
			return reslut;
		}

		/// <summary>
		/// ͨ��JsonPath���ҽ��
		/// </summary>
		/// <param name="jsonPath">JsonPath ���ʽ</param>
		/// <returns>��ѯ�ӿ�</returns>
		public override ISelectable JsonPath(string jsonPath)
		{
			JsonPathSelector jsonPathSelector = new JsonPathSelector(jsonPath);
			return SelectList(jsonPathSelector);
		}

		/// <summary>
		/// Remove padding for JSON
		/// </summary>
		/// <param name="text"></param>
		/// <param name="padding"></param>
		/// <returns></returns>
		private string RemovePadding(string text, string padding)
		{
			if (string.IsNullOrWhiteSpace(padding))
			{
				return text;
			}

			XTokenQueue tokenQueue = new XTokenQueue(text);
			tokenQueue.ConsumeWhitespace();
			tokenQueue.Consume(padding);
			tokenQueue.ConsumeWhitespace();
			return tokenQueue.ChompBalancedNotInQuotes('(', ')');
		}

		private void FixAllRelativeHrefs(HtmlDocument document, string url)
		{
			var nodes = document.DocumentNode.SelectNodes("//a[not(starts-with(@href,'http') or starts-with(@href,'https'))]");
			if (nodes != null)
			{
				foreach (var node in nodes)
				{
					if (node.Attributes["href"] != null)
					{
						node.Attributes["href"].Value = UrlUtil.CanonicalizeUrl(node.Attributes["href"].Value, url);
					}
				}
			}

			var images = document.DocumentNode.SelectNodes(".//img");
			if (images != null)
			{
				foreach (var image in images)
				{
					if (image.Attributes["src"] != null)
					{
						image.Attributes["src"].Value = UrlUtil.CanonicalizeUrl(image.Attributes["src"].Value, url);
					}
				}
			}
		}

		private void RemoveOutboundLinks(HtmlDocument document, params string[] domains)
		{
			var nodes = document.DocumentNode.SelectNodes(".//a");
			if (nodes != null)
			{
				List<HtmlNode> deleteNodes = new List<HtmlNode>();
				foreach (var node in nodes)
				{
					bool isMatch = false;
					foreach (var domain in domains)
					{
						var href = node.Attributes["href"]?.Value;
						if (!string.IsNullOrWhiteSpace(href) && System.Text.RegularExpressions.Regex.IsMatch(href, domain))
						{
							isMatch = true;
							break;
						}
					}
					if (!isMatch)
					{
						deleteNodes.Add(node);
					}
				}
				foreach (var node in deleteNodes)
				{
					node.Remove();
				}
			}
		}
	}
}