﻿using DotnetSpider.Core.Processor;
using System.Linq;
using Xunit;
using System.Net.Http;

namespace DotnetSpider.Core.Test.Processor
{

	public class TargetUrlTests
	{
		public class CnblogsProcessor1 : BasePageProcessor
		{
			public CnblogsProcessor1()
			{
				TargetUrlsExtractor = new RegionAndPatternTargetUrlsExtractor(".//div[@class='pager']", "/sitehome/p/\\d+", "^http://www\\.cnblogs\\.com/$");
			}

			protected override void Handle(Page page)
			{
				page.ResultItems.AddOrUpdateResultItem("test", true);
			}
		}

		[Fact]
		public void UrlVerifyAndExtract1()
		{
			HttpClient client = new HttpClient();
			var html = client.GetStringAsync("http://www.cnblogs.com").Result;

			Page page = new Page(new Request("http://www.cnblogs.com/") { Site = new Site() });
			page.Content = html;

			CnblogsProcessor1 processor = new CnblogsProcessor1();

			processor.Process(page, new DefaultSpider());

			Assert.True(page.ResultItems.GetResultItem("test"));
			Assert.Equal(12, page.TargetRequests.Count);
			Assert.Equal("http://www.cnblogs.com/", page.TargetRequests.ElementAt(11).Url.ToString());
		}

		public class CnblogsProcessor2 : BasePageProcessor
		{
			public CnblogsProcessor2()
			{
				TargetUrlsExtractor = new RegionAndPatternTargetUrlsExtractor(".//div[@class='pager']", "/sitehome/p/\\d+", "^http://www\\.cnblogs\\.com/$");
			}

			protected override void Handle(Page page)
			{
				page.ResultItems.AddOrUpdateResultItem("test", true);
			}
		}

		[Fact]
		public void UrlVerifyAndExtract2()
		{
			HttpClient client = new HttpClient();
			var html = client.GetStringAsync("http://www.cnblogs.com").Result;

			Page page = new Page(new Request("http://www.cnblogs.com/") { Site = new Site() });
			page.Content = html;

			CnblogsProcessor2 processor = new CnblogsProcessor2();
			processor.Process(page, new DefaultSpider());

			Assert.True(page.ResultItems.GetResultItem("test"));
			Assert.Equal(12, page.TargetRequests.Count);
			Assert.Equal("http://www.cnblogs.com/", page.TargetRequests.ElementAt(11).Url.ToString());
		}

		public class CnblogsProcessor3 : BasePageProcessor
		{
			public CnblogsProcessor3()
			{
				TargetUrlsExtractor = new RegionAndPatternTargetUrlsExtractor(".", "/sitehome/p/\\d+");
			}

			protected override void Handle(Page page)
			{
				page.ResultItems.AddOrUpdateResultItem("test", true);
			}
		}

		[Fact]
		public void ProcessorFilterDefaultRequest()
		{
			Env.ProcessorFilterDefaultRequest = false;
			HttpClient client = new HttpClient();
			var html = client.GetStringAsync("http://www.cnblogs.com").Result;
			Site site = new Site();
			Page page = new Page(new Request("http://www.cnblogs.com/") { Site = site });
			page.Content = html;

			CnblogsProcessor3 processor = new CnblogsProcessor3();
			processor.Process(page, new DefaultSpider());

			Assert.True(page.ResultItems.GetResultItem("test"));
			Assert.Equal(11, page.TargetRequests.Count);
		}

		public class CnblogsProcessor4 : BasePageProcessor
		{
			public CnblogsProcessor4()
			{
				TargetUrlsExtractor = new RegionAndPatternTargetUrlsExtractor(".", "/sitehome/p/\\d+");
			}

			protected override void Handle(Page page)
			{
				page.ResultItems.AddOrUpdateResultItem("test", true);
			}
		}

		[Fact]
		public void UrlVerifyAndExtract4()
		{
			HttpClient client = new HttpClient();
			var html = client.GetStringAsync("http://www.cnblogs.com/sitehome/p/2/").Result;

			Page page = new Page(new Request("http://www.cnblogs.com/sitehome/p/2/") { Site = new Site() });
			page.Content = html;

			CnblogsProcessor4 processor = new CnblogsProcessor4();
			processor.Process(page, new DefaultSpider());

			Assert.True(page.ResultItems.GetResultItem("test"));
			Assert.Equal(12, page.TargetRequests.Count);
			Assert.Equal("http://www.cnblogs.com/sitehome/p/2/", page.TargetRequests.ElementAt(0).Url.ToString());
		}
	}
}
