using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DotnetSpider.Core
{
	/// <summary>
	/// �洢ҳ����������ݽ��
	/// �˶��������ҳ�������, ���������ݹܵ�������
	/// </summary>
	public class ResultItems
	{
		private readonly Dictionary<string, dynamic> _fields = new Dictionary<string, dynamic>();

		/// <summary>
		/// ��ȡ�������ݽ��
		/// </summary>
		public IReadOnlyDictionary<string, dynamic> Results => _fields;

		/// <summary>
		/// ��Ӧ��Ŀ��������Ϣ
		/// </summary>
		public Request Request { get; set; }

		/// <summary>
		/// �洢�����ݽ���Ƿ�Ϊ��
		/// </summary>
		public bool IsEmpty => _fields.Count == 0;

		/// <summary>
		/// ͨ����ֵȡ�����ݽ��
		/// </summary>
		/// <param name="key">��ֵ</param>
		/// <returns>���ݽ��</returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public dynamic GetResultItem(string key)
		{
			return _fields.ContainsKey(key) ? _fields[key] : null;
		}

		/// <summary>
		/// ��ӻ�������ݽ��
		/// </summary>
		/// <param name="key">��ֵ</param>
		/// <param name="value">���ݽ��</param>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void AddOrUpdateResultItem(string key, dynamic value)
		{
			if (_fields.ContainsKey(key))
			{
				_fields[key] = value;
			}
			else
			{
				_fields.Add(key, value);
			}
		}
	}
}