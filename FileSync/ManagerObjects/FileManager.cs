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
        private static string main_folder_path = @"E:\UwAmp\www\Cluster1";
        private static FileSystemWatcher watcher = null;

        private static string logs_file_path = @"E:\UwAmp\www\Cluster1\logs";
        private static List<string> remote_folders_paths = null;


        /*
         * Public functions zone
         */
        // Don't be lazy, put the paths in a file next time
        public static void Initialize()
        {
            remote_folders_paths = new List<string>();
            //remote_folders_paths.Add(@"E:\UwAmp\www\Cluster2"); @"\\ipaddress\sharename\filename"
            remote_folders_paths.Add(@"\\127.0.0.1\E$\UwAmp\www\Cluster2"); 
        }

        // Starts the watcher
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

        // Stops the watcher
        public static void StopWatcher()
        {
            if (watcher != null)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
                watcher = null;
            }
        }

        public static void SyncFiles()
        {
            // Exits if logs file does not exist
            if (!File.Exists(logs_file_path))
                return;
            // List of changes/actions
            try
            {
                List<List<string>> changes = GetChangesList();
                foreach(List<string> change in changes)
                {
                    string action = change[0];
                    string filename = change[1];
                    switch (action)
                    {
                        case "created":
                            // Copy file to all remotes
                            CopyFile(filename);
                            break;
                        case "renamed":
                            break;
                        case "deleted":
                            break;
                    }
                }
                File.Delete(logs_file_path);
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message + " " + exception.StackTrace);
                throw exception;
            }
        }

        /*
         * Private functions zone
         */
        private static void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File created: {0}", e.Name);
            WriteChange("created", e.FullPath, e.Name);
        }

        private static void FileSystemWatcher_Renamed(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File renamed: {0}", e.Name);
            WriteChange("renamed", e.FullPath, e.Name);
        }

        private static void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File deleted: {0}", e.Name);
            WriteChange("deleted", e.FullPath, e.Name);
        }

        private static void WriteChange(string state, string file_path, string filename)
        {
            try
            {
                // Creates logs file if not existing
                if (!File.Exists(logs_file_path))
                    File.Create(logs_file_path).Close();
                // Exits if file log is edited
                if (file_path.Equals(logs_file_path))
                    return;
                // Writes changes into logs file
                StreamWriter writer = new StreamWriter(logs_file_path);
                writer.WriteLine(DateTime.Now + "---" + state + "---" + filename);
                writer.Close();
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message + " " + exception.StackTrace);
                throw exception;
            }
        }

        private static List<List<string>> GetChangesList()
        {
            StreamReader reader = null;
            List<List<string>> changes = new List<List<string>>();
            try
            {
                reader = new StreamReader(logs_file_path);
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    List<string> change = new List<string>();
                    Console.WriteLine(line);
                    string[] list = line.Split(new string[] { "---" }, StringSplitOptions.None);
                    change.Add(list[1]);
                    change.Add(list[2]);
                    changes.Add(change);
                }
                return changes;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message + " " + exception.StackTrace);
                throw exception;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        private static void CopyFile(string filename)
        {
            string source = Path.Combine(main_folder_path, filename);
            Console.WriteLine("Filename " + filename);
            Console.WriteLine("source " + source);
            // If it is a folder
            if (Directory.Exists(source))
            {
                Console.WriteLine("Directory");
                return;
            }
            else
            {
                try
                {
                    foreach (string remote_folder_path in remote_folders_paths)
                    {
                        string dest = Path.Combine(remote_folder_path, filename);
                        Console.WriteLine("dest " + dest);
                        File.Copy(source, dest, true);
                    }
                }
                catch(Exception exception)
                {
                    Console.WriteLine(exception.Message + " " + exception.StackTrace);
                    throw exception;
                }
            }
        }
    }
}
