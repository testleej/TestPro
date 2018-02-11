using DotnetSpider.Core.Downloader;
using DotnetSpider.Core.Monitor;
using System;
using System.Net;

namespace DotnetSpider.Core
{
	/// <summary>
	/// ����ӿڶ���
	/// </summary>
	public interface ISpider : IDisposable, IControllable, IAppBase
	{
		/// <summary>
		/// �ɼ�վ�����Ϣ����
		/// </summary>
		Site Site { get; }

		/// <summary>
		/// ��ؽӿ�
		/// </summary>
		IMonitor Monitor { get; set; }
	}
}
