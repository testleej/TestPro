﻿using Dapper;
using DotnetSpider.Core;
using DotnetSpider.Core.Infrastructure.Database;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Scheduler;
using DotnetSpider.Extension.Monitor;
using Xunit;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using DotnetSpider.Core.Downloader;
using DotnetSpider.Core.Infrastructure;

namespace DotnetSpider.Extension.Test
{
	internal class CountResult
	{
		public int Count { get; set; }
	}


	public class LogTest
	{
		public LogTest()
		{
			Env.EnterpiseService = false;
		}

		[Fact]
		public void DatebaseLogAndStatus()
		{
			var logger = DLog.GetLogger();
			string id = Guid.NewGuid().ToString("N");
			Env.NodeId = "DEFAULT";
			using (Spider spider = Spider.Create(new Site { EncodingName = "UTF-8" },
				id,
				new QueueDuplicateRemovedScheduler(),
				new TestPageProcessor()))
			{
				spider.Downloader = new TestDownloader();
				spider.TaskId = "1";
				spider.Monitor = new MySqlMonitor(spider.TaskId, spider.Identity, false, "Database='mysql';Data Source=localhost;User ID=root;Port=3306;SslMode=None;");
				spider.AddPipeline(new TestPipeline());
				for (int i = 0; i < 5; i++)
				{
					logger.NLog(id, "add start url" + i, Level.Info);
					spider.AddStartUrl("http://www.baidu.com/" + i);
				}
				spider.EmptySleepTime = 1000;
				spider.Run();
			}
			using (var conn = new MySqlConnection("Database='mysql';Data Source=localhost;User ID=root;Port=3306;SslMode=None;"))
			{
				var logs = conn.Query<Log>($"SELECT * FROM dotnetspider.log where identity='{id}'").ToList();
				Assert.StartsWith("Crawl complete, cost", logs[logs.Count - 1].message);
				Assert.Equal(1, conn.Query<CountResult>($"SELECT COUNT(*) as Count FROM dotnetspider.status where identity='{id}'").First().Count);
				Assert.Equal("Finished", conn.Query<statusObj>($"SELECT * FROM dotnetspider.status where identity='{id}'").First().status);
			}
		}

		private class TestDownloader : BaseDownloader
		{
			protected override Page DowloadContent(Request request, ISpider spider)
			{
				Console.WriteLine("ok:" + request.Url);
				return new Page(request)
				{
					Content = ""
				};
			}
		}

		class Log
		{
			public string level { get; set; }
			public string message { get; set; }
		}

		class statusObj
		{
			public string status { get; set; }
		}

		internal class TestPipeline : BasePipeline
		{
			public override void Process(IEnumerable<ResultItems> resultItems, ISpider spider)
			{
				foreach (var resultItem in resultItems)
				{
					foreach (var entry in resultItem.Results)
					{
						Console.WriteLine($"{entry.Key}:{entry.Value}");
					}
				}
			}
		}

		internal class TestPageProcessor : BasePageProcessor
		{
			protected override void Handle(Page page)
			{
				page.Skip = true;
			}
		}
	}
}
