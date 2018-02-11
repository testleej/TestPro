﻿using System;
using System.Collections.Generic;
using System.IO;
using DotnetSpider.Core;
using DotnetSpider.Core.Selector;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extension.Model.Attribute;

using Xunit;
using DotnetSpider.Extension.Processor;
using System.Linq;

namespace DotnetSpider.Extension.Test
{

	public class EntityExtractorTest
	{
		[Fact]
		public void Extract()
		{
			EntityExtractor<Product> extractor = new EntityExtractor<Product>();
			var results = extractor.Extract(new Page(new Request("http://list.jd.com/list.html?cat=9987,653,655&page=2&JL=6_0_0&ms=5#J_main", new Dictionary<string, dynamic>
			{
				{ "cat", "手机" },
				{ "cat3", "110" }
			})
			{ Site = new Site() })
			{
				Content = File.ReadAllText(Path.Combine(Env.BaseDirectory, "Jd.html"))
			});
			Assert.Equal(60, results.Count());
			Assert.Equal("手机", results.First().CategoryName);
			Assert.Equal(110, results.First().CategoryId);
			Assert.Equal("http://item.jd.com/3031737.html", results.First().Url);
			Assert.Equal("3031737", results.First().Sku);
			Assert.Equal("荣耀官方旗舰店", results.First().ShopName);
			Assert.Equal("荣耀 NOTE 8 4GB+32GB 全网通版 冰河银", results.First().Name);
			Assert.Equal("1000000904", results.First().VenderId);
			Assert.Equal("1000000904", results.First().JdzyShopId);
			Assert.Equal(DateTime.Now.ToString("yyyy-MM-dd"), results.First().RunId.ToString("yyyy-MM-dd"));
		}

		[Fact]
		public void TempEntityNoPrimaryInfo()
		{
			EntityProcessor<Entity1> processor = new EntityProcessor<Entity1>();
			var page = new Page(new Request("http://www.abcd.com") { Site = new Site() })
			{
				Content = "{'data':[{'age':'1'},{'age':'2'}]}"
			};
			processor.Process(page, new DefaultSpider());
			Assert.Equal(2, (page.ResultItems.GetResultItem("DotnetSpider.Extension.Test.EntityExtractorTest+Entity1") as List<Entity1>).Count);
		}

		[EntityTable("test", "sku2", EntityTable.Today)]
		[EntitySelector(Expression = "//li[@class='gl-item']/div[contains(@class,'j-sku-item')]")]
		private class Product : SpiderEntity
		{
			[PropertyDefine(Expression = "cat", Type = SelectorType.Enviroment)]
			public string CategoryName { get; set; }

			[PropertyDefine(Expression = "cat3", Type = SelectorType.Enviroment)]
			public int CategoryId { get; set; }

			[PropertyDefine(Expression = "./div[1]/a/@href")]
			public string Url { get; set; }

			[PropertyDefine(Expression = "./@data-sku")]
			public string Sku { get; set; }

			[PropertyDefine(Expression = "./div[5]/strong/a")]
			public long CommentsCount { get; set; }

			[PropertyDefine(Expression = ".//div[@class='p-shop']/@data-shop_name")]
			public string ShopName { get; set; }

			[PropertyDefine(Expression = ".//div[@class='p-name']/a/em")]
			public string Name { get; set; }

			[PropertyDefine(Expression = "./@venderid")]
			public string VenderId { get; set; }

			[PropertyDefine(Expression = "./@jdzy_shop_id")]
			public string JdzyShopId { get; set; }

			[PropertyDefine(Expression = "Today", Type = SelectorType.Enviroment)]
			public DateTime RunId { get; set; }
		}

		[EntitySelector(Expression = "$.data[*]", Type = SelectorType.JsonPath)]
		public class Entity1 : SpiderEntity
		{
			[PropertyDefine(Expression = "$.age", Type = SelectorType.JsonPath)]
			public int Age { get; set; }
		}
	}
}
