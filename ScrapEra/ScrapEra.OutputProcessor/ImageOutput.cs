using System;
using System.IO;

namespace ScrapEra.OutputProcessor
{
    public static class ImageOutput
    {
        public static void WriteImageToFile(string base64String, string filePath)
        {
            var bytes = Convert.FromBase64String(base64String);
            using (var imageFile = new FileStream(filePath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }
        }
    }
}