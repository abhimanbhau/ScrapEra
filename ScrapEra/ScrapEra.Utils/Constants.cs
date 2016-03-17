using System.Text.RegularExpressions;

namespace ScrapEra.Utils
{
    public class Constants
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
        // CleanReader Core Constants
        public const string OverlayDivId = "readOverlay";
        public const string InnerDivId = "mainContent";
        public const int MinParagraphLength = 25;
        public const int MinInnerTextLength = 25;
        public const int ParagraphSegmentLength = 100;
        public const int MaxPointsForSegmentsCount = 3;
        public const int MinSiblingParagraphLength = 80;
        public const int MinCommaSegments = 10;
        public const int LisCountTreshold = 100;
        public const int MaxImagesInShortSegmentsCount = 2;
        public const int MinInnerTextLengthInElementsWithEmbed = 75;
        public const int ClassWeightTreshold = 25;
        public const int MaxEmbedsCount = 1;
        public const int MaxArticleTitleLength = 150;
        public const int MinArticleTitleLength = 15;
        public const int MinArticleTitleWordsCount1 = 3;
        public const int MinArticleTitleWordsCount2 = 4;
        public const float SiblingScoreTresholdCoefficient = 0.2f;
        public const float MaxSiblingScoreTreshold = 10.0f;
        public const float MaxSiblingParagraphLinksDensity = 0.25f;
        public const float MaxHeaderLinksDensity = 0.33f;
        public const float MaxDensityForElementsWithSmallerClassWeight = 0.2f;
        public const float MaxDensityForElementsWithGreaterClassWeight = 0.5f;
        public const string StylesheetResourceName = "ScrapEra.CleanReader.Resources.ScrapEra.css";
        // CleanReader Core Compiled Regexes
        public static readonly Regex ArticleTitleColonRegex1 = new Regex(".*:(.*)", RegexOptions.Compiled);
        public static readonly Regex ArticleTitleColonRegex2 = new Regex("[^:]*[:](.*)", RegexOptions.Compiled);
        public static readonly Regex ArticleTitleDashRegex1 = new Regex(" [\\|\\-] ", RegexOptions.Compiled);
        public static readonly Regex ArticleTitleDashRegex2 = new Regex("(.*)[\\|\\-] .*", RegexOptions.Compiled);

        public static readonly Regex ArticleTitleDashRegex3 = new Regex("[^\\|\\-]*[\\|\\-](.*)",
            RegexOptions.Compiled);

        public static readonly Regex BreakBeforeParagraphRegex = new Regex("<br[^>]*>\\s*<p", RegexOptions.Compiled);

        public static readonly Regex DivToPElementsRegex = new Regex("<(a|blockquote|dl|div|img|ol|p|pre|table|ul)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static readonly Regex EndOfSentenceRegex = new Regex("\\.( |$)",
            RegexOptions.Compiled | RegexOptions.Multiline);

        public static readonly Regex KillBreaksRegex = new Regex("(<br\\s*\\/?>(\\s|&nbsp;?)*){1,}",
            RegexOptions.Compiled);

        public static readonly Regex LikelyParagraphDivRegex = new Regex("text|para",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static readonly Regex MailtoHrefRegex = new Regex("^\\s*mailto\\s*:",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static readonly Regex NegativeWeightRegex =
            new Regex(
                "combx|comment|com-|contact|foot|footer|footnote|masthead|media|meta|outbrain|promo|related|scroll|shoutbox|sidebar|side|sponsor|shopping|tags|tool|widget|References",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static readonly Regex NormalizeSpacesRegex = new Regex("\\s{2,}", RegexOptions.Compiled);

        public static readonly Regex OkMaybeItsACandidateRegex = new Regex("and|article|body|column|main|shadow",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static readonly Regex PositiveWeightRegex =
            new Regex("article|body|content|entry|hentry|main|page|pagination|post|text|blog|story",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static readonly Regex ReplaceDoubleBrsRegex = new Regex("(<br[^>]*>[ \\n\\r\\t]*){2,}",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static readonly Regex TitleWhitespacesCleanUpRegex = new Regex("\\s+", RegexOptions.Compiled);

        public static readonly Regex UnlikelyCandidatesRegex =
            new Regex(
                "combx|comment|community|disqus|extra|foot|header|menu|remark|rss|shoutbox|sidebar|side|sponsor|ad-break|agegate|pagination|pager|popup|tweet|twitter",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}