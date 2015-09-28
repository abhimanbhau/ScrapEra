using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEra.ScrapLogger
{
    public class Logger
    {
        #region class fields
        private static StreamWriter _stream;
        private static string _fileName;
        public static List<String> _LogList = new List<string>();
        private static string _debugSymbol = "[DEBUG]";
        private static string _infoSymbol = "[INFO]";
        private static string _errorSymbol = "[ERROR]";
        #endregion


        static Logger()
        {
            Instance = new Logger();
            _fileName = Path.GetTempFileName().Replace(".tmp", ".scrapLog");
            _stream = new StreamWriter(_fileName) { AutoFlush = true, NewLine = Environment.NewLine };
        }

        public static Logger Instance
        {
            get;
            private set;
        }

        public static void LogI(string message)
        {
            message = String.Format("{0} {1}-{2} {3}", _infoSymbol,
                DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), message);
            _stream.WriteLine();
        }

        public static void LogE(string message)
        {
            message = String.Format("{0} {1}-{2} {3}", _errorSymbol,
                DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), message);
            _stream.WriteLine();
        }

        public static void LogD(string message)
        {
            message = String.Format("{0} {1}-{2} {3}", _debugSymbol,
                DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), message);
            _stream.WriteLine();
        }
    }
}
