using System.Collections.Generic;

namespace DotnetSpider.Core.Pipeline
{
	/// <summary>
	/// �������ݽ�������ڴ���.
	/// </summary>
	public interface ICollectionPipeline : IPipeline
	{
		/// <summary>
		/// Get all results collected.
		/// </summary>
		/// <returns>All results collected</returns>
		IEnumerable<ResultItems> GetCollection(ISpider spider);
	}
}
