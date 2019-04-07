using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.ManagerObjects
{
    public class FileManager
    {
        private static string main_folder_path = @"E:\UwAmp\www\Cluster2";
        private static FileSystemWatcher watcher = null;

        private static string logs_file_path = @"E:\UwAmp\www\Cluster2\logs";
        private static List<string> remote_folders_paths = null;


        /*
         * Public functions zone
         */
        // Don't be lazy, put the paths in a file next time
        public static void Initialize()
        {
            remote_folders_paths = new List<string>();
            //remote_folders_paths.Add(@"E:\UwAmp\www\Cluster2"); @"\\ipaddress\sharename\filename"
            remote_folders_paths.Add(@"\\127.0.0.1\E$\UwAmp\www\Cluster1"); 
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
                    watcher.IncludeSubdirectories = true;
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
                // Retrieve all lines in logs
                List<List<string>> changes = GetChangesList();
                foreach(List<string> change in changes)
                {
                    string action = change[0];
                    string filename = change[1];
                    string file_path = change[1];
                    switch (action)
                    {
                        case "created":
                            // Copy file to all remotes
                            CopyFile(file_path);
                            break;
                        case "deleted":
                            // Delete file from all remotes
                            DeleteFile(file_path);
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
            WriteChange("created", e.FullPath, e.Name);
        }

        private static void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            // Rename = create + delete
            WriteChange("created", e.FullPath, e.Name);
            WriteChange("deleted", e.OldFullPath, e.OldName);
        }

        private static void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            WriteChange("deleted", e.FullPath, e.Name);
        }

        // Writes all changes in directory in logs file
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
                // Changing absolute file path to relative
                file_path = file_path.Substring(main_folder_path.Length + 1);
                // Writes changes into logs file
                StreamWriter writer = new StreamWriter(logs_file_path, true);
                writer.WriteLine(DateTime.Now + "---" + state + "---" + filename + "---" + file_path);
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
            List<List<string>> changes = new List<List<string>>();
            using (StreamReader reader = new StreamReader(logs_file_path))
            {
                try
                {
                    string line = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                        List<string> change = new List<string>();
                        Console.WriteLine(line);
                        string[] list = line.Split(new string[] { "---" }, StringSplitOptions.None);
                        change.Add(list[1]);
                        change.Add(list[2]);
                        change.Add(list[3]);
                        changes.Add(change);
                    }
                    return changes;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message + " " + exception.StackTrace);
                    throw exception;
                }
            }
        }

        private static void CopyFile(string file_path)
        {
            string source = Path.Combine(main_folder_path, file_path);
            try
            {
                // Copy files using FTP
                using (FtpClient client = new FtpClient("127.0.0.1", 21, "Yagami", "test"))
                {
                    /*
                        foreach (string remote_folder_path in remote_folders_paths)
                        {
                            string dest = Path.Combine(remote_folder_path, filename);
                            File.Delete(dest);
                        }
                    */
                    client.UploadFile(source, "/" + file_path, FtpExists.Overwrite, true);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message + " " + exception.StackTrace);
                Console.WriteLine("Inner:  ");
                Console.WriteLine(exception.InnerException);
                throw exception;
            }
        }

        private static void DeleteFile(string file_path)
        {
            string dest = "/" + file_path;
            try
            {
                // Delete files using FTP
                using (FtpClient client = new FtpClient("127.0.0.1", 21, "Yagami", "test"))
                {
                    // If it is a directory
                    if (client.DirectoryExists(dest))
                    {
                        Console.WriteLine("Directory");
                        client.DeleteDirectory("/" + file_path, FtpListOption.AllFiles);
                        return;
                    }
                    // If it is a file
                    if (client.FileExists(dest))
                    {
                        Console.WriteLine("File");
                        client.DeleteFile("/" + file_path);
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message + " " + exception.StackTrace);
                Console.WriteLine("Inner:  ");
                Console.WriteLine(exception.InnerException);
                throw exception;
            }
        }
    }
}
