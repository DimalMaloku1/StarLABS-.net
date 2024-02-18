using System.IO;

namespace API
{
    public static class LogEvents
    {
      public static void LogToFile(string Title, string LogMessage, IWebHostEnvironment env)
        {
            bool exists = Directory.Exists(env.WebRootPath + "\\" + "LogFolder");
            if (!exists)
            {
                Directory.CreateDirectory(env.WebRootPath + "\\" + "LogFolder");
            }

            StreamWriter swlog;
            string logPath = "";

            string Filename = DateTime.Now.ToString("ddMMyyyy") + ".txt";
            logPath = Path.Combine(env.WebRootPath + "\\" + "LogFolder", Filename);

            if(!File.Exists(logPath))
            {
                swlog = new StreamWriter(logPath);
            }
            else
            {
                swlog = File.AppendText(logPath);
            }

            swlog.WriteLine("Log Entry");
            swlog.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongTimeString());
            swlog.Write(" :");
            swlog.WriteLine("Message Title: {0}", Title);
            swlog.WriteLine("Message: {0}", LogMessage);
            swlog.WriteLine("----------------------------------------------------");
            swlog.WriteLine("");

            swlog.Close();
        }  
    }
}
