using System;
namespace Karma.Service.Helpers
{
	public class Helper
	{
		public static void RmoveFile(string webRootPath,string folder,string filename)
		{
			File.Delete(Path.Combine(webRootPath, folder, filename));
		}
	}
}

