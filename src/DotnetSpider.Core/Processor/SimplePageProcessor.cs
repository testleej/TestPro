namespace DotnetSpider.Core.Processor
{
	/// <summary>
	/// A simple PageProcessor.
	/// </summary>
	public class SimplePageProcessor : BasePageProcessor
	{
		/// <summary>
		/// ������ҳ���title��html
		/// </summary>
		/// <param name="page">ҳ������</param>
		protected override void Handle(Page page)
		{
			page.AddResultItem("title", page.Selectable.XPath("//title"));
			page.AddResultItem("html", page.Content);
		}
	}
}
