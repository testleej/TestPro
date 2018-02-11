using System;
using System.Collections.Generic;
using DotnetSpider.Extension.Model;
using System.Linq;
using DotnetSpider.Core;
using Newtonsoft.Json;

namespace DotnetSpider.Extension.Pipeline
{
	/// <summary>
	/// Print datas in console
	/// Usually used in test.
	/// </summary>
	public class ConsoleEntityPipeline : BaseEntityPipeline
	{
		/// <summary>
		/// �������ʵ����Ķ���, Console pipeline����Ҫ
		/// </summary>
		/// <param name="entityDefine">����ʵ����Ķ���</param>
		public override void AddEntity(IEntityDefine entityDefine)
		{
		}

		/// <summary>
		/// ��ӡ����ʵ���������������ʵ�����ݽ��������̨
		/// </summary>
		/// <param name="entityName">����ʵ���������</param>
		/// <param name="datas">ʵ��������</param>
		/// <param name="spider">����</param>
		/// <returns>����Ӱ��������(�����ݿ�Ӱ������)</returns>
		public override int Process(string entityName, IEnumerable<dynamic> datas, ISpider spider)
		{
			foreach (var data in datas)
			{
				Console.WriteLine($"{entityName}: {JsonConvert.SerializeObject(data)}");
			}
			return datas.Count();
		}
	}
}
