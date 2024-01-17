using System;
using System.Net;

namespace Karma.Service.Helpers
{
	public class Helper
	{
		public static void RmoveFile(string webRootPath,string folder,string filename)
		{
			File.Delete(Path.Combine(webRootPath, folder, filename));
		}

        public static void SendMessageToTelegram(string Message)
        {
            string urlString = $"https://api.telegram.org/bot6496003360:AAEHiiWVN8LpwuujnDrPYHAl1foJZN8pENY/sendMessage?chat_id=1650815455&text={Message}";
            WebClient webclient = new WebClient();
            string res = webclient.DownloadString(urlString);
            Console.WriteLine(res);
        }
    }
  


}

