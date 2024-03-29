﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using System.Diagnostics;
using System.Configuration;

namespace FolderWatcherService
{
    public class FolderWatcher
    {       
        static string path = @"C:\Folder1";
        static readonly EventLog eventLog = new EventLog(); 
        
        public FolderWatcher()
        {          
            MonitorDirectory(path);
        }       
        private static void MonitorDirectory(string path)
        {
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();           
            fileSystemWatcher.Path = path;
            fileSystemWatcher.NotifyFilter = NotifyFilters.FileName| NotifyFilters.DirectoryName | NotifyFilters.Attributes;
            fileSystemWatcher.Created += FileSystemWatcher_Created;          
            fileSystemWatcher.Deleted += FileSystemWatcher_Created;         
            fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;           
            fileSystemWatcher.EnableRaisingEvents = true;          
        }
      
        private static void WriteLine(string line)
        {
          File.AppendAllText(@"C:\Folder1\FolderWatch.txt", Environment.NewLine + line);         
        }
        private static void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            try
            {                
                WriteLine(e.ChangeType + " | " +  e.FullPath + " | " + DateTime.Now.ToString());
                var message = e.ChangeType + " " + e.FullPath;
                eventLog.Source = "FolderWatcherService";
                eventLog.WriteEntry(message, EventLogEntryType.Information);
            }
            catch (Exception x)
            {               
                eventLog.Source = "FolderWatcherService";
                eventLog.WriteEntry(x.Message, EventLogEntryType.Warning);
            }
        }
        private static void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            try
            {               
                WriteLine("File renamed: \n" + "From " + e.OldName + " to " + e.Name + " | " + DateTime.Now.ToString());
                var message = "File renamed: " + e.ChangeType + " " + e.FullPath;
                eventLog.Source = "FolderWatcherService";
                eventLog.WriteEntry(message, EventLogEntryType.Information);
            }
            catch (Exception x)
            {               
                eventLog.Source = "FolderWatcherService";
                eventLog.WriteEntry(x.Message, EventLogEntryType.Warning);
            }           
        }               
        public void Start()
        {
            eventLog.Source = "FolderWatcherService";
            eventLog.WriteEntry("This is a test message for start demo.", EventLogEntryType.Information);                       
        }

        public void Stop()
        {
            eventLog.Source = "FolderWatcherService";
            eventLog.WriteEntry("This is a test message for stopped demo.", EventLogEntryType.Information);               
        }      
    }
}
