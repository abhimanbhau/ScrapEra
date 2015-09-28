using System;
using System.Collections.Generic;
using System.IO;
using ScrapEra.ScrapLogger;

namespace ScrapEra.OutputProcessor
{
    public class FileOutput
    {
        public static void GenerateFileOutput(string filePath, List<string> content)
        {
            try
            {
                using (var stream = new StreamWriter(filePath) {AutoFlush = true, NewLine = Environment.NewLine})
                {
                    foreach (var line in content)
                    {
                        stream.WriteLine(line);
                    }
                    stream.Flush();
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.LogE("Fatal Error -> " + ex.Message +
                            Environment.NewLine + ex.StackTrace);
            }
        }
    }
}