using DotnetSpider.Core;

namespace DotnetSpider.Extension
{
	/// <summary>
	/// ���ڲ���
	/// </summary>
	internal class DefaultEntitySpider : EntitySpider
	{
		internal DefaultEntitySpider() : this(new Site()) { }

		internal DefaultEntitySpider(Site site) : base(site)
		{
		}

		protected override void MyInit(params string[] arguments)
		{
		}
	}
}