
namespace NWebCrawlerLib.Common
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Threading;

    public enum LogLevel
    {
        None = 0,
        Fatal = 1,
        Error = 2,
        Warn = 4,
        Info = 8,
        Debug = 16,
        Trace = 32,
    }

    public delegate void NewLogEventHandler(object sender, NewLogEventArgs e);
    public class NewLogEventArgs : EventArgs
    {
        public string LogMessage { get; set; }
    }

    public delegate void NewErrorEventHandler(object sender, NewErrorEventArgs e);
    public class NewErrorEventArgs : EventArgs
    {
        public string ErrorMessage { get; set; }
    }

    public class Logger
    {
        const string BaseFileName = "NWebCrawler";
        private string DeclaringType;

        public static event NewLogEventHandler NewLogEvent;
        public static event NewErrorEventHandler NewErrorEvent;

        public Logger(Type type)
        {
            if (type != null)
                DeclaringType = type.FullName;
        }

        public static string LogPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + "\\" + BaseFileName + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + ".log";

            }
        }

        public static void LogMessage(string message, LogLevel logLevel)
        {
            StreamWriter writer = null;

            try
            {
                FormatMessage(ref message);

                System.Console.WriteLine(message);

                writer = File.AppendText(LogPath);
                writer.WriteLine(message);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (null != writer)
                {
                    writer.Close();
                }
            }

        }

        private static void FormatMessage(ref string message)
        {
            int CurrentProcessId = Process.GetCurrentProcess().Id;
            int CurrentThreadId = Thread.CurrentThread.ManagedThreadId;
            string temp = message;
            string format = "yyyy-MM-dd HH:mm:ss.fff";
            message = String.Format(CultureInfo.InvariantCulture, "{0} [pid:{1}] [tid:{2}] - {3}",
                DateTime.Now.ToString(format, CultureInfo.InvariantCulture),
                CurrentProcessId,
                CurrentThreadId,
                temp
                );
        }

        public static void Info(string format, params object[] args)
        {
            Logger.Info(string.Format(format, args));
        }

        public static void Info(string message)
        {
            //Logger.LogMessage(message, LogLevel.Info);
            FormatMessage(ref message);
            NewLogEvent(null, new NewLogEventArgs() { LogMessage = message });
        }

        public static void Error(string format, params object[] args)
        {
            Logger.Error(string.Format(format, args));
        }

        public static void Error(string message)
        {
            //Logger.LogMessage(message, LogLevel.Error);
            FormatMessage(ref message);
            if (NewErrorEvent != null)
            {
                NewErrorEvent(null, new NewErrorEventArgs() { ErrorMessage = message });
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}
