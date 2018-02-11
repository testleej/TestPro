using DotnetSpider.Core;
using DotnetSpider.Core.Infrastructure;
using DotnetSpider.Extension.Model.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotnetSpider.Extension.Model
{
	/// <summary>
	/// ����ʵ����Ķ���
	/// </summary>
	public interface IEntityDefine
	{
		/// <summary>
		/// ����ʵ���������, ���ڽ����������ݹܵ�֮��ƥ��. Ĭ��������ʵ�����ȫ��
		/// </summary>
		string Name { get; }

		/// <summary>
		/// ����ʵ�����ѡ����
		/// </summary>
		SelectorAttribute SelectorAttribute { get; }

		/// <summary>
		/// ʵ�����Ƿ���
		/// </summary>
		bool Multi { get; }

		/// <summary>
		/// ����ʵ���������
		/// </summary>
		Type Type { get; }

		/// <summary>
		/// ����ʵ���Ӧ�����ݿ����Ϣ
		/// </summary>
		EntityTable TableInfo { get; }

		/// <summary>
		/// ����ʵ�嶨������ݿ�����Ϣ
		/// </summary>
		List<Column> Columns { get; }

		/// <summary>
		/// �����ս������Ľ����ȡǰ Take ��ʵ��
		/// </summary>
		int Take { get; }

		/// <summary>
		/// ���� Take �ķ���, Ĭ���Ǵ�ͷ��ȡ
		/// </summary>
		bool TakeFromHead { get; set; }

		/// <summary>
		/// Ŀ�����ӵ�ѡ����
		/// </summary>
		List<TargetUrlsSelector> TargetUrlsSelectors { get; }

		/// <summary>
		/// ����ֵ��ѡ����
		/// </summary>
		List<SharedValueSelector> SharedValues { get; }
	}

	/// <summary>
	/// ����ʵ����Ķ���
	/// </summary>
	/// <typeparam name="T">����ʵ���������</typeparam>
	public class EntityDefine<T> : IEntityDefine
	{
		/// <summary>
		/// ����ʵ���������, ���ڽ����������ݹܵ�֮��ƥ��. Ĭ��������ʵ�����ȫ��
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// ����ʵ�����ѡ����
		/// </summary>
		public SelectorAttribute SelectorAttribute { get; set; }

		/// <summary>
		/// ʵ�����Ƿ���
		/// </summary>
		public bool Multi { get; set; }

		/// <summary>
		/// ����ʵ���������
		/// </summary>
		public Type Type { get; }

		/// <summary>
		/// ����ʵ���Ӧ�����ݿ����Ϣ
		/// </summary>
		public EntityTable TableInfo { get; set; }

		/// <summary>
		/// ����ʵ�嶨������ݿ�����Ϣ
		/// </summary>
		public List<Column> Columns { get; set; } = new List<Column>();

		/// <summary>
		/// �����ս������Ľ����ȡǰ Take ��ʵ��
		/// </summary>
		public int Take { get; set; }

		/// <summary>
		/// ���� Take �ķ���, Ĭ���Ǵ�ͷ��ȡ
		/// </summary>
		public bool TakeFromHead { get; set; } = true;

		/// <summary>
		/// Ŀ�����ӵ�ѡ����
		/// </summary>
		public List<TargetUrlsSelector> TargetUrlsSelectors { get; set; }

		/// <summary>
		/// ��Processor�Ľṹ�����һ���ӹ�����
		/// </summary>
		public DataHandler<T> DataHandler { get; set; }

		/// <summary>
		/// ����ֵ��ѡ����
		/// </summary>
		public List<SharedValueSelector> SharedValues { get; internal set; }

		/// <summary>
		/// ���췽��
		/// </summary>
		public EntityDefine()
		{
			Type = typeof(T);

			var typeName = Type.FullName;
			Name = typeName;

			TableInfo = Type.GetCustomAttribute<EntityTable>();

			if (TableInfo != null)
			{
				if (TableInfo.Indexs != null)
				{
					TableInfo.Indexs = new HashSet<string>(TableInfo.Indexs.Select(i => i.Replace(" ", ""))).ToArray();
				}
				if (TableInfo.Uniques != null)
				{
					TableInfo.Uniques = new HashSet<string>(TableInfo.Uniques.Select(i => i.Replace(" ", ""))).ToArray();
				}
			}
			EntitySelector entitySelector = Type.GetCustomAttribute<EntitySelector>();
			if (entitySelector != null)
			{
				Multi = true;
				Take = entitySelector.Take;
				TakeFromHead = entitySelector.TakeFromHead;
				SelectorAttribute = new SelectorAttribute { Expression = entitySelector.Expression, Type = entitySelector.Type };
			}
			else
			{
				Multi = false;
			}
			var targetUrlsSelectors = Type.GetCustomAttributes<TargetUrlsSelector>();
			TargetUrlsSelectors = targetUrlsSelectors.ToList();
			var sharedValueSelectorAttributes = Type.GetCustomAttributes<SharedValueSelector>();
			SharedValues = sharedValueSelectorAttributes.Select(e => new SharedValueSelector
			{
				Name = e.Name,
				Expression = e.Expression,
				Type = e.Type
			}).ToList();

			GenerateEntityColumns();

			ValidateEntityDefine();
		}

		private void GenerateEntityColumns()
		{
			var properties = Type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

			foreach (var property in properties)
			{
				var propertyDefine = property.GetCustomAttribute<PropertyDefine>();
				if (propertyDefine == null)
				{
					continue;
				}

				var column = new Column(property, propertyDefine);
				Columns.Add(column);
			}
		}

		private void ValidateEntityDefine()
		{
			var columns = Columns.Where(c => !c.IgnoreStore).ToList();

			if (columns.Count == 0)
			{
				throw new SpiderException($"Columns is necessary for {Name}");
			}
			if (TableInfo == null)
			{
				return;
			}

			if (TableInfo.UpdateColumns != null && TableInfo.UpdateColumns.Length > 0)
			{
				foreach (var column in TableInfo.UpdateColumns)
				{
					if (columns.All(c => c.Name != column))
					{
						throw new SpiderException("Columns set to update are not a property of your entity");
					}
				}
				var updateColumns = new List<string>(TableInfo.UpdateColumns);
				foreach (var id in Env.IdColumns)
				{
					updateColumns.Remove(id);
				}

				TableInfo.UpdateColumns = updateColumns.ToArray();

				if (TableInfo.UpdateColumns.Length == 0)
				{
					throw new SpiderException("There is no column need update");
				}
			}

			if (TableInfo.Indexs != null && TableInfo.Indexs.Length > 0)
			{
				for (int i = 0; i < TableInfo.Indexs.Length; ++i)
				{
					var items = new HashSet<string>(TableInfo.Indexs[i].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim()));

					if (items.Count == 0)
					{
						throw new SpiderException("Index should contain more than a column");
					}
					if (items.Count == 1 && Env.IdColumns.Contains(items.First()))
					{
						throw new SpiderException("Primary is no need to create another index");
					}
					foreach (var item in items)
					{
						var column = columns.FirstOrDefault(c => c.Name == item);
						if (column == null)
						{
							throw new SpiderException("Columns set as index are not a property of your entity");
						}
						if (column.DataType.FullName == DataTypeNames.String && (column.Length <= 0 || column.Length > 256))
						{
							throw new SpiderException("Column length of index should not large than 256");
						}
					}
					TableInfo.Indexs[i] = string.Join(",", items);
				}
			}
			if (TableInfo.Uniques != null && TableInfo.Uniques.Length > 0)
			{
				for (int i = 0; i < TableInfo.Uniques.Length; ++i)
				{
					var items = new HashSet<string>(TableInfo.Uniques[i].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim()));

					if (items.Count == 0)
					{
						throw new SpiderException("Unique should contain more than a column");
					}
					if (items.Count == 1 && Env.IdColumns.Contains(items.First()))
					{
						throw new SpiderException("Primary is no need to create another unique");
					}
					foreach (var item in items)
					{
						var column = columns.FirstOrDefault(c => c.Name == item);
						if (column == null)
						{
							throw new SpiderException("Columns set as unique are not a property of your entity");
						}
						if (column.DataType.FullName == DataTypeNames.String && (column.Length <= 0 || column.Length > 256))
						{
							throw new SpiderException("Column length of unique should not large than 256");
						}
					}
					TableInfo.Uniques[i] = string.Join(",", items);
				}
			}
		}
	}

	/// <summary>
	/// ���ݿ��еĶ���
	/// </summary>
	public class Column
	{
		/// <summary>
		/// ���췽��
		/// </summary>
		/// <param name="property">���Ե���Ϣ</param>
		/// <param name="propertyDefine">���ԵĶ���</param>
		public Column(PropertyInfo property, PropertyDefine propertyDefine)
		{
			Property = property;
			PropertyDefine = propertyDefine;

			if (DataType.FullName != DataTypeNames.String && propertyDefine.Length > 0)
			{
				throw new SpiderException("Only string property can set length");
			}
			DefaultValue = Property.PropertyType.IsValueType ? Activator.CreateInstance(Property.PropertyType) : null;
			Option = propertyDefine.Option;
			SelectorAttribute = new SelectorAttribute
			{
				Expression = propertyDefine.Expression,
				Type = propertyDefine.Type,
				Arguments = propertyDefine.Arguments
			};
			NotNull = propertyDefine.NotNull;
			IgnoreStore = propertyDefine.IgnoreStore;
			Length = propertyDefine.Length;

			foreach (var formatter in property.GetCustomAttributes<Formatter.Formatter>(true))
			{
				Formatters.Add(formatter);
			}
		}

		/// <summary>
		/// ���ԵĶ���
		/// </summary>
		public PropertyDefine PropertyDefine { get; }

		/// <summary>
		/// ���Ե���Ϣ
		/// </summary>
		public PropertyInfo Property { get; }

		/// <summary>
		/// ���Ե�Ĭ��ֵ
		/// </summary>
		public object DefaultValue { get; }

		/// <summary>
		/// �е�����
		/// </summary>
		public string Name => Property.Name;

		/// <summary>
		/// ����ֵ��ѡ����
		/// </summary>
		public SelectorAttribute SelectorAttribute { get; set; }

		/// <summary>
		/// ����ֵ�Ƿ�Ϊ��
		/// </summary>
		public bool NotNull { get; set; }

		/// <summary>
		/// ����ѡ��
		/// </summary>
		public PropertyDefineOptions Option { get; set; }

		/// <summary>
		/// �еĳ���
		/// </summary>
		public int Length { get; set; }

		/// <summary>
		/// ���Ե�����
		/// </summary>
		public Type DataType => Property.PropertyType;

		/// <summary>
		/// �Ƿ񲻰Ѵ������ݱ��浽���ݿ�
		/// </summary>
		public bool IgnoreStore { get; set; }

		/// <summary>
		/// ����ֵ�ĸ�ʽ��
		/// </summary>
		public List<Formatter.Formatter> Formatters { get; set; } = new List<Formatter.Formatter>();

		/// <summary>
		/// ���� ToString
		/// </summary>
		/// <returns>String</returns>
		public override string ToString()
		{
			return $"{Name},{DataType.Name}";
		}
	}
}