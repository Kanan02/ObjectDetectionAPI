using System.Diagnostics;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using static Community.CsharpSqlite.Sqlite3;

namespace ObjectDetectionAPI.Services
{
    public class PythonRunService
    {
        public PythonRunService()
        {
            
        }
        public void Run()
        {
            string cmd = "main.py";
            string args = "C:\\Users\\Kenan\\source\\repos\\Aspnet\\ObjectDetectionAPI\\ObjectDetectionAPI\\bin\\Debug\\net6.0\\Uploads\\img.png C:\\Users\\Kenan\\source\\repos\\Aspnet\\ObjectDetectionAPI\\ObjectDetectionAPI\\bin\\Debug\\net6.0\\Uploads";

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Users\Kenan\AppData\Local\Microsoft\WindowsApps\python.exe";
            start.Arguments = string.Format("{0} {1}", cmd, args);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
            }
            
        }
    }
}
