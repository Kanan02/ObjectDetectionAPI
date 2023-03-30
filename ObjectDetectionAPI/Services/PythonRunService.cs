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
            string args = "/mnt/c/Users/aliev/PycharmProjects/yolo_object_detection/img.png /mnt/c/Users/aliev/PycharmProjects/yolo_object_detection/storage";

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "/usr/bin/python";
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