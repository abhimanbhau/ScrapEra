using System;
using System.Collections.Generic;
using System.IO;

namespace ScrapEra.ScrapLogger
{
    public class Logger
    {
        static Logger()
        {
            Instance = new Logger();
            var fileName = Path.GetTempFileName().Replace(".tmp", ".scrapLog");
            Stream = new StreamWriter(fileName) {AutoFlush = true, NewLine = Environment.NewLine};
        }

        public static Logger Instance { get; private set; }

        public static void LogI(string message)
        {
            message = string.Format("{0} {1}-{2} {3}", _infoSymbol,
                DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), message);
            Stream.WriteLine(message);
        }

        public static void LogE(string message)
        {
            message = string.Format("{0} {1}-{2} {3}", _errorSymbol,
                DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), message);
            Stream.WriteLine(message);
        }

        public static void LogD(string message)
        {
            message = string.Format("{0} {1}-{2} {3}", _debugSymbol,
                DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), message);
            Stream.WriteLine(message);
        }

        #region class fields

        private static readonly StreamWriter Stream;
        public static List<string> LogList = new List<string>();
        private static readonly string _debugSymbol = "[DEBUG]";
        private static readonly string _infoSymbol = "[INFO]";
        private static readonly string _errorSymbol = "[ERROR]";

        #endregion
    }
}