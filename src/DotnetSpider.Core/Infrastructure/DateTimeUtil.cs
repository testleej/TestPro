using System;

namespace DotnetSpider.Core.Infrastructure
{
	/// <summary>
	/// ʱ��İ�����
	/// </summary>
	public static class DateTimeUtil
	{
		private static readonly DateTimeOffset Epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
		private const long TicksPerMicrosecond = 10;

		/// <summary>
		/// �����RunId: 2017-12-20
		/// </summary>
		public static string RunIdOfToday { get; }

		/// <summary>
		/// ���µ�RunId: 2017-12-01
		/// </summary>
		public static string RunIdOfMonthly { get; }

		/// <summary>
		/// ���ܵ�RunId: 2018-01-01 (it's monday)
		/// </summary>
		public static string RunIdOfMonday { get; }

		/// <summary>
		/// ��ǰ�·ݵĵ�һ��
		/// </summary>
		public static DateTime FirstDayOfTheMonth { get; }
		/// <summary>
		/// ��ǰ�·ݵĵ�һ��
		/// </summary>
		public static DateTime LastDayOfTheMonth { get; }
		/// <summary>
		/// ��ǰ�·ݵ����һ��
		/// </summary>
		public static DateTime FirstDayOfLastMonth { get; }
		/// <summary>
		/// ǰһ�·ݵ����һ��
		/// </summary>
		public static DateTime LastDayOfLastMonth { get; }
		/// <summary>
		/// ����һ
		/// </summary>
		public static DateTime Monday { get; }
		/// <summary>
		/// ���ڶ�
		/// </summary>
		public static DateTime Tuesday { get; }
		/// <summary>
		/// ������
		/// </summary>
		public static DateTime Wednesday { get; }
		/// <summary>
		/// ������
		/// </summary>
		public static DateTime Thursday { get; }
		/// <summary>
		/// ������
		/// </summary>
		public static DateTime Friday { get; }
		/// <summary>
		/// ������
		/// </summary>
		public static DateTime Saturday { get; }
		/// <summary>
		/// ������
		/// </summary>
		public static DateTime Sunday { get; }
		/// <summary>
		/// ���ܵ�����һ
		/// </summary>
		public static DateTime LastMonday { get; }
		/// <summary>
		/// ���ܵ����ڶ�
		/// </summary>
		public static DateTime LastTuesday { get; }
		/// <summary>
		/// ���ܵ�������
		/// </summary>
		public static DateTime LastWednesday { get; }
		/// <summary>
		/// ���ܵ�������
		/// </summary>
		public static DateTime LastThursday { get; }
		/// <summary>
		/// ���ܵ�������
		/// </summary>
		public static DateTime LastFriday { get; }
		/// <summary>
		/// ���ܵ�������
		/// </summary>
		public static DateTime LastSaturday { get; }
		/// <summary>
		/// ���ܵ�������
		/// </summary>
		public static DateTime LastSunday { get; }
		/// <summary>
		/// ���ܵ�����һ
		/// </summary>
		public static DateTime NextMonday { get; }
		/// <summary>
		/// ���ܵ����ڶ�
		/// </summary>
		public static DateTime NextTuesday { get; }
		/// <summary>
		/// ���ܵ�������
		/// </summary>
		public static DateTime NextWednesday { get; }
		/// <summary>
		/// ���ܵ�������
		/// </summary>
		public static DateTime NextThursday { get; }
		/// <summary>
		/// ���ܵ�������
		/// </summary>
		public static DateTime NextFriday { get; }
		/// <summary>
		/// ���ܵ�������
		/// </summary>
		public static DateTime NextSaturday { get; }
		/// <summary>
		/// ���ܵ�������
		/// </summary>
		public static DateTime NextSunday { get; }

		/// <summary>
		/// ��ʱ��ת����Unixʱ��: 1515133023012
		/// </summary>
		/// <param name="time">ʱ��</param>
		/// <returns>Unixʱ��</returns>
		public static string ConvertDateTimeToUnix(DateTime time)
		{
			return time.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds.ToString("f0");
		}

		/// <summary>
		/// ��Unixʱ��ת����DateTime
		/// </summary>
		/// <param name="unixTime">Unixʱ��</param>
		/// <returns>DateTime</returns>
		public static DateTime ToDateTimeOffset(long unixTime)
		{
			return Epoch.AddTicks(unixTime * TicksPerMicrosecond).DateTime;
		}

		/// <summary>
		/// ��ȡ��ǰUnixʱ��
		/// </summary>
		/// <returns>Unixʱ��</returns>
		public static string GetCurrentUnixTimeString()
		{
			return ConvertDateTimeToUnix(DateTime.UtcNow);
		}

		/// <summary>
		/// ��ȡ��ǰUnixʱ��
		/// </summary>
		/// <returns>Unixʱ��</returns>
		public static double GetCurrentUnixTimeNumber()
		{
			return DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
		}

		static DateTimeUtil()
		{
			var now = DateTime.Now.Date;

			FirstDayOfTheMonth = now.AddDays(now.Day * -1 + 1);
			LastDayOfTheMonth = FirstDayOfTheMonth.AddMonths(1).AddDays(-1);
			FirstDayOfLastMonth = FirstDayOfTheMonth.AddMonths(-1);
			LastDayOfLastMonth = FirstDayOfTheMonth.AddDays(-1);

			int i = now.DayOfWeek - DayOfWeek.Monday == -1 ? 6 : -1;
			TimeSpan ts = new TimeSpan(i, 0, 0, 0);

			Monday = now.Subtract(ts).Date;
			Tuesday = Monday.AddDays(1);
			Wednesday = Monday.AddDays(2);
			Thursday = Monday.AddDays(3);
			Friday = Monday.AddDays(4);
			Saturday = Monday.AddDays(5);
			Sunday = Monday.AddDays(6);

			LastMonday = Monday.AddDays(-7);
			LastTuesday = LastMonday.AddDays(1);
			LastWednesday = LastMonday.AddDays(2);
			LastThursday = LastMonday.AddDays(3);
			LastFriday = LastMonday.AddDays(4);
			LastSaturday = LastMonday.AddDays(5);
			LastSunday = LastMonday.AddDays(6);

			NextMonday = Sunday.AddDays(1);
			NextTuesday = Monday.AddDays(1);
			NextWednesday = Monday.AddDays(2);
			NextThursday = Monday.AddDays(3);
			NextFriday = Monday.AddDays(4);
			NextSaturday = Monday.AddDays(5);
			NextSunday = Monday.AddDays(6);

			RunIdOfToday = now.ToString("yyyy-MM-dd");
			RunIdOfMonthly = FirstDayOfTheMonth.ToString("yyyy-MM-dd");
			RunIdOfMonday = Monday.ToString("yyyy-MM-dd");
		}
	}
}