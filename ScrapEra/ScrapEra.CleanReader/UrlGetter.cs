using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace ScrapEra.CleanReader
{
    public class UrlGetter
    {
        private const int BufferSize = 8192;
        private readonly WebClientWithCookieContainer _webClient;

        public UrlGetter()
        {
            _webClient = new WebClientWithCookieContainer();
            _webClient.Headers.Add("User-Agent",
                Constants.UserAgentString);
            _webClient.Headers.Add("Accept", Constants.WebContentMimeInfo);
            _webClient.Headers.Add("Accept-Encoding", Constants.WebCompressionAlgorithms);
        }

        public string Fetch(string url)
        {
            return DownloadString(url);
        }

        public string DownloadString(string url)
        {
            return MakeRequest(url, () => _webClient.DownloadData(url));
        }

        private string MakeRequest(string url, Func<byte[]> makeRequestFunc)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            var responseBytes = makeRequestFunc();
            var contentEncoding = _webClient.ResponseHeaders["Content-Encoding"];
            if (!string.IsNullOrEmpty(contentEncoding))
            {
                contentEncoding = contentEncoding.ToLower();
                if (contentEncoding.Contains("gzip"))
                {
                    responseBytes = Decompress(responseBytes, CompressionAlgorithm.GZip);
                }
                else if (contentEncoding.Contains("deflate"))
                {
                    responseBytes = Decompress(responseBytes, CompressionAlgorithm.Deflate);
                }
                else
                {
                    throw new Exception("Unsupported content encoding: " + contentEncoding + ".");
                }
            }
            return Encoding.UTF8.GetString(responseBytes);
        }

        private static byte[] Decompress(byte[] responseBytes, CompressionAlgorithm compressionAlgorithm)
        {
            Stream decompressingStream;
            switch (compressionAlgorithm)
            {
                case CompressionAlgorithm.GZip:
                    decompressingStream = new GZipStream(new MemoryStream(responseBytes), CompressionMode.Decompress);
                    break;
                case CompressionAlgorithm.Deflate:
                    decompressingStream = new DeflateStream(new MemoryStream(responseBytes), CompressionMode.Decompress);
                    break;
                default:
                    throw new NotSupportedException("Unsupported compression algorithm: " + compressionAlgorithm + ".");
            }
            try
            {
                var buffer = new byte[BufferSize];
                using (var outputMemoryStream = new MemoryStream())
                {
                    while (true)
                    {
                        var bytesRead = decompressingStream.Read(buffer, 0, buffer.Length);
                        if (bytesRead <= 0)
                        {
                            break;
                        }
                        outputMemoryStream.Write(buffer, 0, bytesRead);
                    }
                    return outputMemoryStream.ToArray();
                }
            }
            finally
            {
                decompressingStream.Close();
            }
        }

        private enum CompressionAlgorithm
        {
            GZip,
            Deflate
        }
    }
}