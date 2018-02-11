using System.Collections.Generic;
using System.IO;

namespace DotnetSpider.Core.Infrastructure
{
	/// <summary>
	/// �ļ�����չ
	/// </summary>
	public static class DirectoryExtension
	{
		/// <summary>
		/// ����ļ���û�д���, ���������
		/// </summary>
		/// <param name="fullName">�ļ�·��</param>
		/// <returns>�ļ���·��</returns>
		public static string CheckAndMakeParentDirecotry(string fullName)
		{
			string path = Path.GetDirectoryName(fullName);

			if (path != null)
			{
				DirectoryInfo directory = new DirectoryInfo(path);
				if (!directory.Exists)
				{
					directory.Create();
				}
			}
			return path;
		}

		/// <summary>
		/// �����ļ���
		/// </summary>
		/// <param name="source">�����Ƶ��ļ���</param>
		/// <param name="destination">Ŀ���ļ���</param>
		/// <returns></returns>
		public static void CopyTo(this DirectoryInfo source, string destination)
		{
			string sourcepath = source.FullName;
			Queue<FileSystemInfo> copyfolders = new Queue<FileSystemInfo>(new DirectoryInfo(sourcepath).GetFileSystemInfos());
			string copytopath = destination;
			copytopath = copytopath.LastIndexOf(Path.DirectorySeparatorChar) == copytopath.Length - 1 ? copytopath : (copytopath + Path.DirectorySeparatorChar) + Path.GetFileName(sourcepath);
			Directory.CreateDirectory(copytopath);
			while (copyfolders.Count > 0)
			{
				FileSystemInfo filsSystemInfo = copyfolders.Dequeue();
				if (!(filsSystemInfo is FileInfo file))
				{
					DirectoryInfo directory = (DirectoryInfo) filsSystemInfo;
					Directory.CreateDirectory(directory.FullName.Replace(sourcepath, copytopath));
					foreach (FileSystemInfo fi in directory.GetFileSystemInfos())
					{
						copyfolders.Enqueue(fi);
					}
				}
				else
				{
					file.CopyTo(file.FullName.Replace(sourcepath, copytopath));
				}
			}
		}
	}
}
