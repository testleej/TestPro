using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotnetSpider.Core.Selector
{
	/// <summary>
	/// JsonPath selector.
	/// Used to extract content from JSON.
	/// </summary>
	public class JsonPathSelector : ISelector
	{
		private readonly string _jsonPath;

		/// <summary>
		/// ���췽��
		/// </summary>
		/// <param name="jsonPath">JsonPath</param>
		public JsonPathSelector(string jsonPath)
		{
			_jsonPath = jsonPath;
		}

		/// <summary>
		/// ��JSON�ı��в�ѯ�������
		/// ������������Ľ���ж��, �����ص�һ��
		/// </summary>
		/// <param name="json">��Ҫ��ѯ��Json�ı�</param>
		/// <returns>��ѯ���</returns>
		public dynamic Select(dynamic json)
		{
			IList<dynamic> result = SelectList(json);
			if (result.Count > 0)
			{
				return result[0];
			}
			return null;
		}

		/// <summary>
		/// ��JSON�ı��в�ѯ���н��
		/// </summary>
		/// <param name="json">��Ҫ��ѯ��Json�ı�</param>
		/// <returns>��ѯ���</returns>
		public IEnumerable<dynamic> SelectList(dynamic json)
		{
			if (json != null)
			{
				List<dynamic> list = new List<dynamic>();
				if (json is string)
				{
					if (JsonConvert.DeserializeObject(json) is JObject )
					{
                        JObject o = JsonConvert.DeserializeObject(json);

                        var items = o.SelectTokens(_jsonPath).Select(t => t.ToString()).ToList();
						if (items.Count > 0)
						{
							list.AddRange(items);
						}
					}
					else
					{
						JArray array = JsonConvert.DeserializeObject(json) as JArray;
						var items = array?.SelectTokens(_jsonPath).Select(t => t.ToString()).ToList();
						if (items != null && items.Count > 0)
						{
							list.AddRange(items);
						}
					}
				}
				else
				{
					dynamic realText = json is HtmlNode  ? JsonConvert.DeserializeObject<JObject>(json.InnerText) : json;

					if (realText is JObject )
					{
                        JObject o = realText;

                        var items = o.SelectTokens(_jsonPath).Select(t => t.ToString()).ToList();
						if (items.Count > 0)
						{
							list.AddRange(items);
						}
					}
					else
					{
						JArray array = json as JArray;
						var items = array?.SelectTokens(_jsonPath).Select(t => t.ToString()).ToList();
						if (items != null && items.Count > 0)
						{
							list.AddRange(items);
						}
					}
				}
				return list;
			}
			else
			{
				return Enumerable.Empty<dynamic>();
			}
		}
	}
}