using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.ManagerObjects
{
    public class FileManager
    {
        private static string main_folder_path = "E:\\UwAmp\\www\\Cluster1";
        private static FileSystemWatcher watcher = null;

        private static string logs_file_path = "E:\\UwAmp\\www\\Cluster1\\logs";

        public static void StartWatcher()
        {
            try
            {
                if (watcher == null)
                {
                    watcher = new FileSystemWatcher();
                    watcher.Path = main_folder_path;
                    watcher.Created += FileSystemWatcher_Created;
                    watcher.Renamed += FileSystemWatcher_Renamed;
                    watcher.Deleted += FileSystemWatcher_Deleted;
                }
                watcher.EnableRaisingEvents = true;
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message + " " + exception.StackTrace);
                throw exception;
            }
        }

        public static void StopWatcher()
        {
            if (watcher != null)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
                watcher = null;
            }
        }

        private static void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File created: {0}", e.Name);
            WriteChange("created", e.FullPath);
        }

        private static void FileSystemWatcher_Renamed(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File renamed: {0}", e.Name);
            WriteChange("renamed", e.FullPath);
        }

        private static void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File deleted: {0}", e.Name);
            WriteChange("deleted", e.FullPath);
        }

        private static void WriteChange(string state, string path)
        {
            // Creates logs file if not existing
            if (!File.Exists(logs_file_path))
                File.Create(logs_file_path).Close();
            // Exits if file log is edited
            if (path.Equals(logs_file_path))
                return;
            // Writes changes into logs file
            StreamWriter writer = new StreamWriter(logs_file_path);
            writer.WriteLine(DateTime.Now + " - " + state + " - " + path);
            writer.Close();
        }
    }
}
