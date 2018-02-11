using System;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Scheduler;

namespace DotnetSpider.Core
{
	/// <summary>
	/// Ĭ������, ���ڲ��Ժ�һЩĬ�����ʹ��, ���ʹ���߿ɺ���
	/// </summary>
	public class DefaultSpider : Spider
	{
		/// <summary>
		/// ���췽��
		/// </summary>
		public DefaultSpider() : this(Guid.NewGuid().ToString("N"), new Site())
		{
		}

		/// <summary>
		/// ���췽��
		/// </summary>
		/// <param name="id">�����ʶ</param>
		/// <param name="site">��վ��Ϣ</param>
		public DefaultSpider(string id, Site site) : base(site, id, new QueueDuplicateRemovedScheduler(), new[] { new SimplePageProcessor() }, new[] { new ConsolePipeline() })
		{
		}

		/// <summary>
		/// ���췽��
		/// </summary>
		/// <param name="id">�����ʶ</param>
		/// <param name="site">��վ��Ϣ</param>
		/// <param name="scheduler">URL����</param>
		public DefaultSpider(string id, Site site, IScheduler scheduler) : base(site, id, scheduler, new[] { new SimplePageProcessor() }, new[] { new ConsolePipeline() })
		{
		}
	}
}
