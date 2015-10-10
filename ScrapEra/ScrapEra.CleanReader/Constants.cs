namespace ScrapEra.CleanReader
{
    public static class Constants
    {
        public const string ScriptTag = "<script>";
        public const string ScriptEndTag = "</script>";
        public const int ScriptEndTagLength = 9;

        public const string HtmlDocumentHeader =
            "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\"\r\n\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n";

        public const string UserAgentString =
            "    Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";

        public const string WebCompressionAlgorithms = "gzip,deflate";
        public const string WebContentMimeInfo = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
    }
}