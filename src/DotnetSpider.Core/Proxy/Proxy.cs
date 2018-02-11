using DotnetSpider.Core.Infrastructure;

namespace DotnetSpider.Core.Proxy
{
	/// <summary>
	/// ������Ϣ
	/// </summary>
	public class Proxy
	{
		private double _lastBorrowTime = DateTimeUtil.GetCurrentUnixTimeNumber();

		/// <summary>
		/// ʵ�ʴ�����Ϣ
		/// </summary>
		public readonly UseSpecifiedUriWebProxy WebProxy;

		/// <summary>
		/// ��һ�ο�ʹ�õ�ʱ��
		/// </summary>
		public double CanReuseTime { get; set; }

		/// <summary>
		/// ͨ���������һ�����ز������ĵ�ʱ��
		/// </summary>
		public double ResponseTime { get; private set; }

		/// <summary>
		/// ʹ�ô˴����������ݵ�ʧ�ܴ���
		/// </summary>
		public int FailedNum { get; private set; }

		/// <summary>
		/// ���췽��
		/// </summary>
		/// <param name="proxy">ʵ�ʴ�����Ϣ</param>
		/// <param name="reuseTimeInterval">�������ٴ�ʹ�õļ��</param>
		public Proxy(UseSpecifiedUriWebProxy proxy, int reuseTimeInterval = 1500)
		{
			WebProxy = proxy;

			CanReuseTime = DateTimeUtil.GetCurrentUnixTimeNumber() + reuseTimeInterval * 100;
		}

		/// <summary>
		/// ��ȡ��һ��ʹ�õ�ʱ��
		/// </summary>
		/// <returns>��һ��ʹ�õ�ʱ��</returns>
		public double GetLastUseTime()
		{
			return _lastBorrowTime;
		}

		/// <summary>
		/// ������һ��ʹ�õ�ʱ��
		/// </summary>
		/// <param name="lastBorrowTime">��һ��ʹ�õ�ʱ��</param>
		public void SetLastBorrowTime(double lastBorrowTime)
		{
			_lastBorrowTime = lastBorrowTime;
		}

		/// <summary>
		/// ����ͨ���������һ�����ز������ĵ�ʱ��
		/// </summary>
		public void RecordResponse()
		{
			ResponseTime = (DateTimeUtil.GetCurrentUnixTimeNumber() - _lastBorrowTime + ResponseTime) / 2;
			_lastBorrowTime = DateTimeUtil.GetCurrentUnixTimeNumber();
		}

		/// <summary>
		/// ��¼һ��ʹ�ô˴����������ݵ�ʧ��
		/// </summary>
		public void Fail()
		{
			FailedNum++;
		}

		/// <summary>
		/// ȡ��ʵ�ʴ�����Ϣ
		/// </summary>
		/// <returns>ʵ�ʴ�����Ϣ</returns>
		public UseSpecifiedUriWebProxy GetWebProxy()
		{
			return WebProxy;
		}

		/// <summary>
		/// ����ʹ�ô˴����������ݵ�ʧ�ܴ���
		/// </summary>
		/// <param name="num">����</param>
		public void SetFailedNum(int num)
		{
			FailedNum = num;
		}

		/// <summary>
		/// ������һ�ο�ʹ�õ�ʱ��
		/// </summary>
		/// <param name="reuseTimeInterval">�������ٴ�ʹ�õļ��</param>
		public void SetReuseTime(int reuseTimeInterval)
		{
			CanReuseTime = DateTimeUtil.GetCurrentUnixTimeNumber() + reuseTimeInterval * 100;
		}
	}
}