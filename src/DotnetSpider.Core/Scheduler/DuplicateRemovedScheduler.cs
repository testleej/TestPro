using DotnetSpider.Core.Scheduler.Component;
using System.Collections.Generic;
using DotnetSpider.Core.Redial;

namespace DotnetSpider.Core.Scheduler
{
	/// <summary>
	/// Remove duplicate urls and only push urls which are not duplicate.
	/// </summary>
	public abstract class DuplicateRemovedScheduler : Named, IScheduler
	{
		/// <summary>
		/// ȥ����
		/// </summary>
		protected IDuplicateRemover DuplicateRemover { get; set; } = new HashSetDuplicateRemover();

		/// <summary>
		/// �������
		/// </summary>
		protected ISpider Spider { get; set; }

		/// <summary>
		/// �ɼ��ɹ����������� 1
		/// </summary>
		public abstract void IncreaseSuccessCount();

		/// <summary>
		/// �ɼ�ʧ�ܵĴ����� 1
		/// </summary>
		public abstract void IncreaseErrorCount();

		/// <summary>
		/// ��������
		/// </summary>
		/// <param name="requests">�������</param>
		public abstract void Import(IEnumerable<Request> requests);

		/// <summary>
		/// �Ƿ��ʹ�û�����
		/// </summary>
		protected abstract bool UseInternet { get; set; }

		/// <summary>
		/// ʣ��������
		/// </summary>
		public abstract long LeftRequestsCount { get; }

		/// <summary>
		/// �ܵ�������
		/// </summary>
		public virtual long TotalRequestsCount => DuplicateRemover.TotalRequestsCount;

		/// <summary>
		/// �ɼ��ɹ���������
		/// </summary>
		public abstract long SuccessRequestsCount { get; }

		/// <summary>
		/// �ɼ�ʧ�ܵĴ���, ����������, ���һ�����Ӳɼ���ζ�ʧ�ܻ��¼���
		/// </summary>
		public abstract long ErrorRequestsCount { get; }

		/// <summary>
		/// �Ƿ��������
		/// </summary>
		public bool DepthFirst { get; set; } = true;

		/// <summary>
		/// ���������󵽶���
		/// </summary>
		/// <param name="request">�������</param>
		public void Push(Request request)
		{
			if (UseInternet)
			{
				NetworkCenter.Current.Execute("sch-push", () =>
				{
					DoPush(request);
				});
			}
			else
			{
				DoPush(request);
			}
		}

		/// <summary>
		/// ��ʼ������
		/// </summary>
		/// <param name="spider">�������</param>
		public virtual void Init(ISpider spider)
		{
			if (Spider == null)
			{
				Spider = spider;
			}
			else
			{
				throw new SpiderException("Scheduler already init");
			}
		}

		/// <summary>
		/// Reset duplicate check.
		/// </summary>
		public abstract void ResetDuplicateCheck();

		/// <summary>
		/// ȡ��һ����Ҫ������������
		/// </summary>
		/// <returns>�������</returns>
		public virtual Request Poll()
		{
			return null;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public virtual void Dispose()
		{
			DuplicateRemover.Dispose();
		}

		/// <summary>
		/// ������������
		/// </summary>
		public virtual void Export()
		{
		}

		/// <summary>
		/// �����������
		/// </summary>
		public virtual void Clear()
		{
		}

		/// <summary>
		/// ������Ӳ����ظ��ľ���ӵ�������
		/// </summary>
		/// <param name="request">�������</param>
		protected virtual void PushWhenNoDuplicate(Request request)
		{
		}

		private bool ShouldReserved(Request request)
		{
			return request.CycleTriedTimes > 0 && request.CycleTriedTimes <= Spider.Site.CycleRetryTimes;
		}

		private void DoPush(Request request)
		{
			if (!DuplicateRemover.IsDuplicate(request) || ShouldReserved(request))
			{
				PushWhenNoDuplicate(request);
			}
		}
	}
}