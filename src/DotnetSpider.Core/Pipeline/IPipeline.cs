using System;
using System.Collections.Generic;

namespace DotnetSpider.Core.Pipeline
{
	/// <summary>
	/// ���ݹܵ��ӿ�, ͨ�����ݹܵ��ѽ��������ݴ浽��ͬ�Ĵ洢��(�ļ������ݿ⣩
	/// </summary>
	public interface IPipeline : IDisposable
	{
		/// <summary>
		/// ����ҳ������������������ݽ��
		/// </summary>
		/// <param name="resultItems">���ݽ��</param>
		/// <param name="spider">����</param>
		void Process(IEnumerable<ResultItems> resultItems, ISpider spider);

		/// <summary>
		/// ��ʹ�����ݹܵ�ǰ, ����һЩ��ʼ������, �������е����ݹܵ�����Ҫ���г�ʼ��
		/// </summary>
		void Init();
	}
}