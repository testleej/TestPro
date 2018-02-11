using System;
using System.Collections.Generic;
using System.Net;

namespace DotnetSpider.Core.Downloader
{
	/// <summary>
	/// Downloader interface
	/// </summary>
	/// <summary xml:lang="zh-CN">
	/// �������ӿ�
	/// </summary>
	public interface IDownloader : IDisposable
	{
		/// <summary>
		/// Download content from a web url
		/// </summary>
		/// <summary xml:lang="zh-CN">
		/// ������������
		/// </summary>
		/// <param name="request">�������� <see cref="Request"/></param>
		/// <param name="spider">����ӿ� <see cref="ISpider"/></param>
		/// <returns>�������ݷ�װ�õ�ҳ����� <see cref="Page"/></returns>
		Page Download(Request request, ISpider spider);

		/// <summary>
		/// Add handlers for post-processing.
		/// </summary>
		/// <summary xml:lang="zh-CN">
		/// ���������ɺ�ĺ����������
		/// </summary>
		/// <param name="handler"><see cref="IAfterDownloadCompleteHandler"/></param>
		void AddAfterDownloadCompleteHandler(IAfterDownloadCompleteHandler handler);

		/// <summary>
		/// Add handlers for pre-processing.
		/// </summary>
		/// <summary xml:lang="zh-CN">
		/// ������ز���ǰ�Ĵ������
		/// </summary>
		/// <param name="handler"><see cref="IBeforeDownloadHandler"/></param>
		void AddBeforeDownloadHandler(IBeforeDownloadHandler handler);

		/// <summary>
		/// Add cookies.
		/// </summary>
		/// <summary xml:lang="zh-CN">
		/// ���� Cookie
		/// </summary>
		/// <param name="cookie">Cookie <see cref="Cookie"/></param>
		void AddCookie(Cookie cookie);

		/// <summary>
		/// Add cookies.
		/// </summary>
		/// <summary xml:lang="zh-CN">
		/// ���� Cookie
		/// </summary>
		/// <param name="name">����(<see cref="Cookie.Name"/>)</param>
		/// <param name="value">ֵ(<see cref="Cookie.Value"/>)</param>
		/// <param name="domain">������(<see cref="Cookie.Domain"/>)</param>
		/// <param name="path">����·��(<see cref="Cookie.Path"/>)</param>
		void AddCookie(string name, string value, string domain, string path = "/");

		/// <summary>
		/// Add cookies to downloader
		/// </summary>
		/// <summary xml:lang="zh-CN">
		/// ���Cookies
		/// </summary>
		/// <param name="cookies">Cookies�ļ�ֵ�� (Cookie's key-value pairs)</param>
		/// <param name="domain">������(<see cref="Cookie.Domain"/>)</param>
		/// <param name="path">����·��(<see cref="Cookie.Path"/>)</param>
		void AddCookies(IDictionary<string, string> cookies, string domain, string path = "/");

		/// <summary>
		/// Add cookies to downloader
		/// </summary>
		/// <summary xml:lang="zh-CN">
		/// ���� Cookies
		/// </summary>
		/// <param name="cookiesStr">Cookies�ļ�ֵ���ַ���, ��: a1=b;a2=c;(Cookie's key-value pairs string, a1=b;a2=c; etc.)</param>
		/// <param name="domain">������(<see cref="Cookie.Domain"/>)</param>
		/// <param name="path">����·��(<see cref="Cookie.Path"/>)</param>
		void AddCookies(string cookiesStr, string domain, string path = "/");

		/// <summary>
		/// Cookie Injector.
		/// </summary>
		/// <summary xml:lang="zh-CN">
		/// Cookie ע����
		/// </summary>
		ICookieInjector CookieInjector { get; set; }

		/// <summary>
		/// Clone a Downloader throuth <see cref="object.MemberwiseClone"/>, override if you need a deep clone or others. 
		/// </summary>
		/// <summary xml:lang="zh-CN">
		/// ��¡һ��������, ���߳�ʱ, ÿ���߳�ʹ��һ������������, ������WebDriver������������Ҫ����WebDriver����ĸ�����, ÿ����������ֻ����һ��WebDriver
		/// </summary>
		/// <returns>������</returns>
		IDownloader Clone();

		/// <summary>
		/// Gets a <see cref="System.Net.CookieCollection"/> that contains the <see cref="System.Net.Cookie"/> instances that are associated with a specific <see cref="Uri"/>.
		/// </summary>
		/// <param name="uri">The URI of the System.Net.Cookie instances desired.</param>
		/// <returns>A System.Net.CookieCollection that contains the System.Net.Cookie instances that are associated with a specific URI.</returns>
		CookieCollection GetCookies(Uri uri);
	}
}
