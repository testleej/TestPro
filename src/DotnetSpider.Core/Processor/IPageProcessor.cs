namespace DotnetSpider.Core.Processor
{
	/// <summary>
	/// ҳ�����������ȡ��
	/// </summary>
	public interface IPageProcessor
	{
		/// <summary>
		/// �������ݽ��, ����Ŀ������
		/// </summary>
		/// <param name="page">ҳ������</param>
		/// <param name="spider">�������</param>
		void Process(Page page, ISpider spider);
	}
}
