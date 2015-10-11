namespace ScrapEra.CleanReader
{
    public class CleanReaderWeb
    {
        private readonly SgmlDomSerializerFactory _sgmlDomSerializer;
        private readonly CleanReaderCore _transcoder;
        private readonly UrlGetter _urlFetcher;

        public CleanReaderWeb(CleanReaderCore transcoder, UrlGetter urlFetcher)
        {
            _transcoder = transcoder;
            _urlFetcher = urlFetcher;
            _sgmlDomSerializer = new SgmlDomSerializerFactory();
        }

        public CleanReaderWeb()
            : this(new CleanReaderCore(), new UrlGetter())
        {
        }

        public string Transcode(string url, out bool mainContentExtracted)
        {
            string extractedTitle;
            return DoTranscode(url, DomSerializationParams.CreateDefault(), out mainContentExtracted, out extractedTitle);
        }


        private string DoTranscode(string url, DomSerializationParams domSerializationParams,
            out bool mainContentExtracted, out string extractedTitle)
        {
            var htmlContent = _urlFetcher.Fetch(url);
            if (string.IsNullOrEmpty(htmlContent))
            {
                mainContentExtracted = false;
                extractedTitle = null;

                return null;
            }
            var document = _transcoder.TranscodeToXml(htmlContent, url, out mainContentExtracted, out extractedTitle);
            return _sgmlDomSerializer.SerializeDocument(document, domSerializationParams);
        }
    }
}