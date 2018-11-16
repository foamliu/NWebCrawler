using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace NWebCrawlerLib
{
    static class MemCache
    {
        public static Collection<string> AllowedFileTypes;
        public static int ConnectionTimeoutMs;
        public static string SQLiteDBFolder;
        public static string FileSystemFolder;
        public static bool AllowAllMimeTypes;
        public static int ThreadCount;
        public static int ThreadSleepTimeWhenQueueIsEmptyMs;

        static MemCache()
        {
            AllowedFileTypes = new Collection<string>(Settings.FileMatches.Split(','));
            ConnectionTimeoutMs = Settings.ConnectionTimeout * 1000;
            SQLiteDBFolder = Settings.SQLiteDBFolder;
            FileSystemFolder = Settings.FileSystemFolder;
            AllowAllMimeTypes = Settings.AllowAllMimeTypes;
            ThreadCount = Settings.ThreadCount;
            ThreadSleepTimeWhenQueueIsEmptyMs = Settings.ThreadSleepTimeWhenQueueIsEmpty * 1000;
        }
    }
}
