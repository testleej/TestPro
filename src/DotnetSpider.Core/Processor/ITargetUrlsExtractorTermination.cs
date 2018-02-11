﻿namespace DotnetSpider.Core.Processor
{
	/// <summary>
	/// 目标链接解析器的中止器
	/// </summary>
	public interface ITargetUrlsExtractorTermination
	{
		/// <summary>
		/// Return true, skip all urls from target urls extractor.
		/// </summary>
		/// <param name="page">页面数据</param>
		/// <returns>是否到了最终一个链接</returns>
		bool IsTermination(Page page);
	}
}
