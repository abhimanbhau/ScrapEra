using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace ScrapEra.CleanReader
{
    public class CleanReaderCore
    {
        public const ReadingStyle DefaultReadingStyle = ReadingStyle.Ebook;
        public const ReadingMargin DefaultReadingMargin = ReadingMargin.XNarrow;
        public const ReadingSize DefaultReadingSize = ReadingSize.XLarge;

        internal const string OverlayDivId = "readOverlay";
        internal const string InnerDivId = "mainContent";
        internal const string ContentDivId = "readability-content";
        internal const string ReadabilityStyledCssClass = "readability-styled";

        private const int MinParagraphLength = 25;
        private const int MinInnerTextLength = 25;
        private const int ParagraphSegmentLength = 100;
        private const int MaxPointsForSegmentsCount = 3;
        private const int MinSiblingParagraphLength = 80;
        private const int MinCommaSegments = 10;
        private const int LisCountTreshold = 100;
        private const int MaxImagesInShortSegmentsCount = 2;
        private const int MinInnerTextLengthInElementsWithEmbed = 75;
        private const int ClassWeightTreshold = 25;
        private const int MaxEmbedsCount = 1;
        private const int MaxArticleTitleLength = 150;
        private const int MinArticleTitleLength = 15;
        private const int MinArticleTitleWordsCount1 = 3;
        private const int MinArticleTitleWordsCount2 = 4;

        private const float SiblingScoreTresholdCoefficient = 0.2f;
        private const float MaxSiblingScoreTreshold = 10.0f;
        private const float MaxSiblingParagraphLinksDensity = 0.25f;
        private const float MaxHeaderLinksDensity = 0.33f;
        private const float MaxDensityForElementsWithSmallerClassWeight = 0.2f;
        private const float MaxDensityForElementsWithGreaterClassWeight = 0.5f;

        private static readonly string ReadabilityStylesheetResourceName = "ScrapEra.CleanReader" +
                                                                           ".Resources.ScrapEra.css";


        private static readonly Regex _UnlikelyCandidatesRegex =
            new Regex(
                "combx|comment|community|disqus|extra|foot|header|menu|remark|rss|shoutbox|sidebar|side|sponsor|ad-break|agegate|pagination|pager|popup|tweet|twitter",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _OkMaybeItsACandidateRegex = new Regex("and|article|body|column|main|shadow",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _PositiveWeightRegex =
            new Regex("article|body|content|entry|hentry|main|page|pagination|post|text|blog|story",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _NegativeWeightRegex =
            new Regex(
                "combx|comment|com-|contact|foot|footer|footnote|masthead|media|meta|outbrain|promo|related|scroll|shoutbox|sidebar|side|sponsor|shopping|tags|tool|widget|References",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _DivToPElementsRegex = new Regex("<(a|blockquote|dl|div|img|ol|p|pre|table|ul)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _EndOfSentenceRegex = new Regex("\\.( |$)",
            RegexOptions.Compiled | RegexOptions.Multiline);

        private static readonly Regex _BreakBeforeParagraphRegex = new Regex("<br[^>]*>\\s*<p", RegexOptions.Compiled);
        private static readonly Regex _NormalizeSpacesRegex = new Regex("\\s{2,}", RegexOptions.Compiled);

        private static readonly Regex _KillBreaksRegex = new Regex("(<br\\s*\\/?>(\\s|&nbsp;?)*){1,}",
            RegexOptions.Compiled);

        private static readonly Regex _ReplaceDoubleBrsRegex = new Regex("(<br[^>]*>[ \\n\\r\\t]*){2,}",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _ArticleTitleDashRegex1 = new Regex(" [\\|\\-] ", RegexOptions.Compiled);
        private static readonly Regex _ArticleTitleDashRegex2 = new Regex("(.*)[\\|\\-] .*", RegexOptions.Compiled);

        private static readonly Regex _ArticleTitleDashRegex3 = new Regex("[^\\|\\-]*[\\|\\-](.*)",
            RegexOptions.Compiled);

        private static readonly Regex _ArticleTitleColonRegex1 = new Regex(".*:(.*)", RegexOptions.Compiled);
        private static readonly Regex _ArticleTitleColonRegex2 = new Regex("[^:]*[:](.*)", RegexOptions.Compiled);

        private static readonly Regex _LikelyParagraphDivRegex = new Regex("text|para",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _MailtoHrefRegex = new Regex("^\\s*mailto\\s*:",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _TitleWhitespacesCleanUpRegex = new Regex("\\s+", RegexOptions.Compiled);
        private readonly bool _dontNormalizeSpacesInTextContent;
        private readonly bool _dontWeightClasses;
        private readonly Dictionary<XElement, float> _elementsScores;
        private readonly ReadingMargin _readingMargin;
        private readonly ReadingSize _readingSize;
        private readonly ReadingStyle _readingStyle;
        private readonly SgmlDomFactory _SgmlDomFactory;
        private readonly SgmlDomSerializerFactory _sgmlDomSerializer;
        private bool _dontStripUnlikelys;

        private CleanReaderCore(
            bool dontStripUnlikelys,
            bool dontNormalizeSpacesInTextContent,
            bool dontWeightClasses,
            ReadingStyle readingStyle,
            ReadingMargin readingMargin,
            ReadingSize readingSize)
        {
            _dontStripUnlikelys = dontStripUnlikelys;
            _dontNormalizeSpacesInTextContent = dontNormalizeSpacesInTextContent;
            _dontWeightClasses = dontWeightClasses;
            _readingStyle = readingStyle;
            _readingMargin = readingMargin;
            _readingSize = readingSize;

            _SgmlDomFactory = new SgmlDomFactory();
            _sgmlDomSerializer = new SgmlDomSerializerFactory();
            _elementsScores = new Dictionary<XElement, float>();
        }

        public CleanReaderCore(ReadingStyle readingStyle, ReadingMargin readingMargin, ReadingSize readingSize)
            : this(false, false, false, readingStyle, readingMargin, readingSize)
        {
        }

        public CleanReaderCore()
            : this(DefaultReadingStyle, DefaultReadingMargin, DefaultReadingSize)
        {
        }

        public Func<AttributeTransformationInput, AttributeTransformationResult> ImageSourceTranformer { get; set; }
        public Func<AttributeTransformationInput, AttributeTransformationResult> AnchorHrefTranformer { get; set; }

        internal XDocument TranscodeToXml(string htmlContent, string url, out bool mainContentExtracted,
            out string extractedTitle)
        {
            if (string.IsNullOrEmpty(htmlContent))
            {
                throw new ArgumentNullException("htmlContent");
            }

            var document = _SgmlDomFactory.BuildDocument(htmlContent);

            PrepareDocument(document);

            if (!string.IsNullOrEmpty(url))
            {
                ResolveElementsUrls(document, "img", "src", url, ImageSourceTranformer);
                ResolveElementsUrls(document, "a", "href", url, AnchorHrefTranformer);
            }

            var articleTitleElement = ExtractArticleTitle(document);
            var articleContentElement = ExtractArticleContent(document, url);

            GlueDocument(document, articleTitleElement, articleContentElement);

            mainContentExtracted = !articleContentElement.IsEmpty;
            extractedTitle = ExtractTitle(document);

            return document;
        }

        internal void PrepareDocument(XDocument document)
        {
            document
                .DescendantNodes()
                .Where(node => node.NodeType == XmlNodeType.Comment)
                .Remove();

            /* In some cases a body element can't be found (if the HTML is totally hosed for example),
             * so we create a new body element and append it to the document. */
            var documentBody = GetOrCreateBody(document);

            var rootElement = document.Root;

            var elementsToRemove = new List<XElement>();

            rootElement.GetElementsByTagName("script")
                .ForEach(scriptElement => { elementsToRemove.Add(scriptElement); });

            RemoveElements(elementsToRemove);

            elementsToRemove.Clear();
            elementsToRemove.AddRange(rootElement.GetElementsByTagName("noscript"));
            RemoveElements(elementsToRemove);

            elementsToRemove.Clear();
            elementsToRemove.AddRange(
                rootElement.GetElementsByTagName("link")
                    .Where(element => element.GetAttributeValue("rel", "").Trim().ToLower() == "stylesheet"
                                      &&
                                      element.GetAttributeValue("href", "")
                                          .LastIndexOf("readability", StringComparison.Ordinal) == -1));
            RemoveElements(elementsToRemove);

            /* Remove all style tags. */
            elementsToRemove.Clear();
            elementsToRemove.AddRange(rootElement.GetElementsByTagName("style"));
            RemoveElements(elementsToRemove);

            /* Remove all nav tags. */
            elementsToRemove.Clear();
            elementsToRemove.AddRange(rootElement.GetElementsByTagName("nav"));
            RemoveElements(elementsToRemove);

            /* Remove all anchors. */
            elementsToRemove.Clear();

            var anchorElements =
                rootElement.GetElementsByTagName("a")
                    .Where(aElement => aElement.Attribute("name") != null && aElement.Attribute("href") == null);

            elementsToRemove.AddRange(anchorElements);
            RemoveElements(elementsToRemove);

            var bodyInnerHtml = documentBody.GetInnerHtml();

            bodyInnerHtml = _ReplaceDoubleBrsRegex.Replace(bodyInnerHtml, "</p><p>");

            documentBody.SetInnerHtml(bodyInnerHtml);
        }

        internal XElement ExtractArticleTitle(XDocument document)
        {
            var documentBody = GetOrCreateBody(document);
            var documentTitle = document.GetTitle() ?? "";
            var currentTitle = documentTitle;

            if (_ArticleTitleDashRegex1.IsMatch(currentTitle))
            {
                currentTitle = _ArticleTitleDashRegex2.Replace(documentTitle, "$1");

                if (currentTitle.Split(' ').Length < MinArticleTitleWordsCount1)
                {
                    currentTitle = _ArticleTitleDashRegex3.Replace(documentTitle, "$1");
                }
            }
            else if (currentTitle.IndexOf(": ", StringComparison.Ordinal) != -1)
            {
                currentTitle = _ArticleTitleColonRegex1.Replace(documentTitle, "$1");

                if (currentTitle.Split(' ').Length < MinArticleTitleWordsCount1)
                {
                    currentTitle = _ArticleTitleColonRegex2.Replace(documentTitle, "$1");
                }
            }
            else if (currentTitle.Length > MaxArticleTitleLength || currentTitle.Length < MinArticleTitleLength)
            {
                var titleHeaders = documentBody.GetElementsByTagName("h1").ToList();

                if (titleHeaders.Count == 0)
                {
                    // if we don't have any level one headers let's give level two header a chance
                    titleHeaders = documentBody.GetElementsByTagName("h2").ToList();
                }

                if (titleHeaders.Count == 1)
                {
                    currentTitle = GetInnerText(titleHeaders[0]);
                }
            }

            currentTitle = (currentTitle ?? "").Trim();

            if (!string.IsNullOrEmpty(documentTitle)
                && currentTitle.Split(' ').Length <= MinArticleTitleWordsCount2)
            {
                currentTitle = documentTitle;
            }

            if (string.IsNullOrEmpty(currentTitle))
            {
                return null;
            }

            var articleTitleElement = new XElement("h1");

            articleTitleElement.SetInnerHtml(currentTitle);

            return articleTitleElement;
        }

        internal XElement ExtractArticleContent(XDocument document, string url = null)
        {
            StripUnlikelyCandidates(document);
            CollapseRedundantParagraphDivs(document);
            var candidatesForArticleContent =
                FindCandidatesForArticleContent(
                    document, "");

            var topCandidateElement = DetermineTopCandidateElement(document, candidatesForArticleContent);
            var articleContentElement = CreateArticleContentElement(document, topCandidateElement);

            PrepareArticleContentElement(articleContentElement);

            return articleContentElement;
        }

        internal void GlueDocument(XDocument document, XElement articleTitleElement, XElement articleContentElement)
        {
            var documentBody = GetOrCreateBody(document);

            /* Include readability.css stylesheet. */
            var headElement = document.GetElementsByTagName("head").FirstOrDefault();

            if (headElement == null)
            {
                headElement = new XElement("head");
                documentBody.AddBeforeSelf(headElement);
            }

            var styleElement = new XElement("style");

            styleElement.SetAttributeValue("type", "text/css");

            var readabilityStylesheetStream =
                Assembly.GetExecutingAssembly().GetManifestResourceStream(ReadabilityStylesheetResourceName);

            //Stream readabilityStylesheetStream = new MemoryStream(ScrapEra.CleanReader.Properties.Resources.readability.ToArray());

            if (readabilityStylesheetStream == null)
            {
                throw new Exception("Couldn't load the ScrapEra stylesheet embedded resource.");
            }

            using (var sr = new StreamReader(readabilityStylesheetStream))
            {
                styleElement.SetInnerHtml(sr.ReadToEnd());
            }

            headElement.Add(styleElement);

            /* Apply reading style to body. */
            var readingStyleClass = GetReadingStyleClass(_readingStyle);

            documentBody.SetClass(readingStyleClass);
            documentBody.SetStyle("display: block;");

            /* Create inner div. */
            var innerDiv = new XElement("div");

            innerDiv.SetId(InnerDivId);
            innerDiv.SetClass(GetReadingMarginClass(_readingMargin) + " " + GetReadingSizeClass(_readingSize));

            if (articleTitleElement != null)
            {
                innerDiv.Add(articleTitleElement);
            }

            if (articleContentElement != null)
            {
                innerDiv.Add(articleContentElement);
            }

            /* Create overlay div. */
            var overlayDiv = new XElement("div");

            overlayDiv.SetId(OverlayDivId);
            overlayDiv.SetClass(readingStyleClass);
            overlayDiv.Add(innerDiv);

            /* Clear the old HTML, insert the new content. */
            documentBody.RemoveAll();
            documentBody.Add(overlayDiv);
        }

        internal void StripUnlikelyCandidates(XDocument document)
        {
            var rootElement = document.Root;

            new ElementTraveseHelper(
                element =>
                {
                    var elementName = GetElementName(element);

                    var unlikelyMatchString = element.GetClass() + " " + element.GetId();

                    if (unlikelyMatchString.Length > 0
                        && !"body".Equals(elementName, StringComparison.OrdinalIgnoreCase)
                        && !"a".Equals(elementName, StringComparison.OrdinalIgnoreCase)
                        && _UnlikelyCandidatesRegex.IsMatch(unlikelyMatchString)
                        && !_OkMaybeItsACandidateRegex.IsMatch(unlikelyMatchString))
                    {
                        var parentElement = element.Parent;

                        if (parentElement != null)
                        {
                            element.Remove();
                        }
                        return;
                    }

                    /* Turn all divs that don't have children block level elements into p's or replace text nodes within the div with p's. */
                    if ("div".Equals(elementName, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!_DivToPElementsRegex.IsMatch(element.GetInnerHtml()))
                        {
                            // no block elements inside - change to p
                            SetElementName(element, "p");
                        }
                        else
                        {
                            // replace text nodes with p's (experimental)
                            new ChildNodesTraverser(
                                childNode =>
                                {
                                    if (childNode.NodeType != XmlNodeType.Text
                                        || GetInnerText(childNode).Length == 0)
                                    {
                                        return;
                                    }

                                    var paraElement = new XElement("p");

                                    // note that we're not using GetInnerText() here; instead we're getting raw InnerText to preserve whitespaces
                                    paraElement.SetInnerHtml(((XText) childNode).Value);

                                    paraElement.SetClass(ReadabilityStyledCssClass);
                                    paraElement.SetStyle("display: inline;");

                                    childNode.ReplaceWith(paraElement);
                                }
                                ).Traverse(element);
                        }
                    }
                }).Traverse(rootElement);
        }

        internal void CollapseRedundantParagraphDivs(XDocument document)
        {
            var rootElement = document.Root;

            new ElementTraveseHelper(
                element =>
                {
                    var elementName = GetElementName(element);

                    if ("div".Equals(elementName, StringComparison.OrdinalIgnoreCase))
                    {
                        var childNode = element.Nodes().SingleOrNone();

                        var childElement = childNode as XElement;

                        if (childElement != null &&
                            "p".Equals(GetElementName(childElement), StringComparison.OrdinalIgnoreCase))
                        {
                            // we have a div with a single child element that is a paragraph - let's remove the div and attach the paragraph to the div's parent
                            var parentElement = element.Parent;

                            if (parentElement != null)
                            {
                                element.AddBeforeSelf(childElement);
                                element.Remove();
                            }
                        }
                    }
                }).Traverse(rootElement);
        }

        internal IEnumerable<XElement> FindCandidatesForArticleContent(XDocument document,
            string articleContentElementHint = null)
        {
            if (!string.IsNullOrEmpty(articleContentElementHint))
            {
                var articleContentElement =
                    TryFindArticleContentElement(document, articleContentElementHint);

                if (articleContentElement != null)
                {
                    return new[] {articleContentElement};
                }
            }

            var paraElements = document.GetElementsByTagName("p");
            var candidateElements = new HashSet<XElement>();

            _elementsScores.Clear();

            foreach (var paraElement in paraElements)
            {
                var innerText = GetInnerText(paraElement);

                if (innerText.Length < MinParagraphLength)
                {
                    continue;
                }

                var parentElement = paraElement.Parent;
                var grandParentElement = parentElement != null ? parentElement.Parent : null;
                var score = 1; // 1 point for having a paragraph

                // Add points for any comma-segments within this paragraph.
                score += GetSegmentsCount(innerText, ',');

                // For every PARAGRAPH_SEGMENT_LENGTH characters in this paragraph, add another point. Up to MAX_POINTS_FOR_SEGMENTS_COUNT points.
                score += Math.Min(innerText.Length/ParagraphSegmentLength, MaxPointsForSegmentsCount);

                // Add the score to the parent.
                if (parentElement != null &&
                    !"html".Equals(parentElement.Name.LocalName, StringComparison.OrdinalIgnoreCase))
                {
                    candidateElements.Add(parentElement);
                    AddPointsToElementScore(parentElement, score);
                }

                // Add half the score to the grandparent.
                if (grandParentElement != null &&
                    !"html".Equals(grandParentElement.Name.LocalName, StringComparison.OrdinalIgnoreCase))
                {
                    candidateElements.Add(grandParentElement);
                    AddPointsToElementScore(grandParentElement, score/2);
                }
            }

            return candidateElements;
        }

        internal XElement DetermineTopCandidateElement(XDocument document,
            IEnumerable<XElement> candidatesForArticleContent)
        {
            XElement topCandidateElement = null;

            foreach (var candidateElement in candidatesForArticleContent)
            {
                var candidateScore = GetElementScore(candidateElement);

                // Scale the final candidates score based on link density. Good content should have a
                // relatively small link density (5% or less) and be mostly unaffected by this operation.
                var newCandidateScore = (1.0f - GetLinksDensity(candidateElement))*candidateScore;

                SetElementScore(candidateElement, newCandidateScore);

                if (topCandidateElement == null
                    || newCandidateScore > GetElementScore(topCandidateElement))
                {
                    topCandidateElement = candidateElement;
                }
            }

            if (topCandidateElement == null
                || "body".Equals(topCandidateElement.Name.LocalName, StringComparison.OrdinalIgnoreCase))
            {
                topCandidateElement = new XElement("div");

                var documentBody = GetOrCreateBody(document);

                topCandidateElement.Add(documentBody.Nodes());
            }

            return topCandidateElement;
        }

        internal XElement CreateArticleContentElement(XDocument document, XElement topCandidateElement)
        {
            /* Now that we have the top candidate, look through its siblings for content that might also be related.
             * Things like preambles, content split by ads that we removed, etc. */

            var articleContentElement = new XElement("div");

            articleContentElement.SetId(ContentDivId);

            var parentElement = topCandidateElement.Parent;

            if (parentElement == null)
            {
                // if the top candidate element has no parent, it means that it's an element created by us and detached from the document,
                // so we don't analyze its siblings and just attach it to the article content
                articleContentElement.Add(topCandidateElement);

                return articleContentElement;
            }

            var siblingElements = parentElement.Elements();

            var topCandidateElementScore = GetElementScore(topCandidateElement);
            var siblingScoreThreshold =
                Math.Max(
                    MaxSiblingScoreTreshold,
                    SiblingScoreTresholdCoefficient*topCandidateElementScore);

            var topCandidateClass = topCandidateElement.GetClass();

            // iterate through the sibling elements and decide whether append them
            foreach (var siblingElement in siblingElements)
            {
                var append = false;
                var siblingElementName = GetElementName(siblingElement);
                float contentBonus = 0;

                // Give a bonus if sibling nodes and top canidates have the same class name
                if (!string.IsNullOrEmpty(topCandidateClass) && siblingElement.GetClass() == topCandidateClass)
                {
                    contentBonus += topCandidateElementScore*SiblingScoreTresholdCoefficient;
                }

                if (siblingElement == topCandidateElement)
                {
                    // we'll append the article content element (created from the top candidate element during an earlier step)
                    append = true;
                }
                else if ((GetElementScore(siblingElement) + contentBonus) >= siblingScoreThreshold)
                {
                    // we'll append this element if the calculated score is higher than a treshold (derived from the score of the top candidate element)
                    append = true;
                }
                else if ("p".Equals(siblingElementName, StringComparison.OrdinalIgnoreCase))
                {
                    // we have to somehow decide whether we should append this paragraph

                    var siblingElementInnerText = GetInnerText(siblingElement);

                    // we won't append an empty paragraph
                    if (siblingElementInnerText.Length > 0)
                    {
                        var siblingElementInnerTextLength = siblingElementInnerText.Length;

                        if (siblingElementInnerTextLength >= MinSiblingParagraphLength)
                        {
                            // we'll append this paragraph if the links density is not higher than a treshold
                            append = GetLinksDensity(siblingElement) < MaxSiblingParagraphLinksDensity;
                        }
                        else
                        {
                            // we'll append this paragraph if there are no links inside and if it contains a probable end of sentence indicator
                            append = GetLinksDensity(siblingElement).IsCloseToZero()
                                     && _EndOfSentenceRegex.IsMatch(siblingElementInnerText);
                        }
                    }
                }

                if (append)
                {
                    XElement elementToAppend;

                    if ("div".Equals(siblingElementName, StringComparison.OrdinalIgnoreCase)
                        || "p".Equals(siblingElementName, StringComparison.OrdinalIgnoreCase))
                    {
                        elementToAppend = siblingElement;
                    }
                    else
                    {
                        /* We have an element that isn't a common block level element, like a form or td tag.
                         * Turn it into a div so it doesn't get filtered out later by accident. */

                        elementToAppend = new XElement("div");
                        elementToAppend.SetId(siblingElement.GetId());
                        elementToAppend.SetClass(siblingElement.GetClass());
                        elementToAppend.Add(siblingElement.Nodes());
                    }

                    articleContentElement.Add(elementToAppend);
                }
            }

            return articleContentElement;
        }

        internal void PrepareArticleContentElement(XElement articleContentElement)
        {
            CleanStyles(articleContentElement);
            KillBreaks(articleContentElement);

            /* Clean out junk from the article content. */
            Clean(articleContentElement, "form");
            Clean(articleContentElement, "object");

            if (articleContentElement.GetElementsByTagName("h1").Count() == 1)
            {
                Clean(articleContentElement, "h1");
            }

            /* If there is only one h2, they are probably using it as a header and not a subheader,
             * so remove it since we already have a header. */
            if (articleContentElement.GetElementsByTagName("h2").Count() == 1)
            {
                Clean(articleContentElement, "h2");
            }

            Clean(articleContentElement, "iframe");
            CleanHeaders(articleContentElement);

            /* Do these last as the previous stuff may have removed junk that will affect these. */
            CleanConditionally(articleContentElement, "table");
            CleanConditionally(articleContentElement, "ul");
            CleanConditionally(articleContentElement, "div");

            /* Remove extra paragraphs. */
            var paraElements = articleContentElement.GetElementsByTagName("p");
            var elementsToRemove = (from paraElement in paraElements
                                    let innerText = GetInnerText(paraElement, false)
                                    where innerText.Length <= 0 
                                    let imgsCount = paraElement.GetElementsByTagName("img").Count()
                                    where imgsCount <= 0
                                    let embedsCount = paraElement.GetElementsByTagName("embed").Count() 
                                    where embedsCount <= 0 
                                    let objectsCount = paraElement.GetElementsByTagName("object").Count()
                                    where objectsCount <= 0 select paraElement).ToList();

            RemoveElements(elementsToRemove);

            /* Remove br's that are directly before paragraphs. */
            articleContentElement.SetInnerHtml(_BreakBeforeParagraphRegex.Replace(articleContentElement.GetInnerHtml(),
                "<p"));
        }

        internal float GetLinksDensity(XElement element)
        {
            var elementInnerText = GetInnerText(element);
            var elementInnerTextLength = elementInnerText.Length;

            if (elementInnerTextLength == 0)
            {
                // we won't divide by zero
                return 0.0f;
            }

            var linksLength =
                element.GetElementsByTagName("a")
                    .Sum(anchorElement => GetInnerText(anchorElement).Length);

            return (float) linksLength/elementInnerTextLength;
        }

        internal int GetSegmentsCount(string s, char ch)
        {
            return s.Count(c => c == ch) + 1;
        }

        /// <summary>
        ///     Get "class/id weight" of the given <paramref name="element" />. Uses regular expressions to tell if this element
        ///     looks good or bad.
        /// </summary>
        internal int GetClassWeight(XElement element)
        {
            if (_dontWeightClasses)
            {
                return 0;
            }

            var weight = 0;

            /* Look for a special classname. */
            var elementClass = element.GetClass();

            if (elementClass.Length > 0)
            {
                if (_NegativeWeightRegex.IsMatch(elementClass))
                {
                    weight -= 25;
                }

                if (_PositiveWeightRegex.IsMatch(elementClass))
                {
                    weight += 25;
                }
            }

            /* Look for a special ID */
            var elementId = element.GetId();

            if (elementId.Length > 0)
            {
                if (_NegativeWeightRegex.IsMatch(elementId))
                {
                    weight -= 25;
                }

                if (_PositiveWeightRegex.IsMatch(elementId))
                {
                    weight += 25;
                }
            }

            return weight;
        }

        internal string GetInnerText(XNode node, bool dontNormalizeSpaces)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            string result;

            var element = node as XElement;
            if (element != null)
            {
                result = element.Value;
            }
            else if (node is XText)
            {
                result = ((XText) node).Value;
            }
            else
            {
                throw new NotSupportedException(string.Format("Nodes of type '{0}' are not supported.", node.GetType()));
            }

            result = (result ?? "").Trim();

            if (!dontNormalizeSpaces)
            {
                return _NormalizeSpacesRegex.Replace(result, " ");
            }

            return result;
        }

        internal string GetInnerText(XNode node)
        {
            return GetInnerText(node, _dontNormalizeSpacesInTextContent);
        }

        /// <summary>
        ///     Removes extraneous break tags from a <paramref name="element" />.
        /// </summary>
        internal void KillBreaks(XElement element)
        {
            element.SetInnerHtml(_KillBreaksRegex.Replace(element.GetInnerHtml(), "<br />"));
        }

        /// <summary>
        ///     Cleans an element of all elements with name <paramref name="elementName" />.
        /// </summary>
        internal void Clean(XElement rootElement, string elementName)
        {
            var elements = rootElement.GetElementsByTagName(elementName);

            var elementsToRemove = elements.ToList();

            RemoveElements(elementsToRemove);
        }

        /// <summary>
        ///     Cleans a <paramref name="rootElement" /> of all elements with name <paramref name="elementName" /> if they look
        ///     fishy.
        ///     "Fishy" is an algorithm based on content length, classnames, link density, number of images and embeds, etc.
        /// </summary>
        internal void CleanConditionally(XElement rootElement, string elementName)
        {
            if (elementName == null)
            {
                throw new ArgumentNullException("elementName");
            }

            var elements = rootElement.GetElementsByTagName(elementName);
            var elementsToRemove = new List<XElement>();

            foreach (var element in elements)
            {
                var weight = GetClassWeight(element);
                var score = GetElementScore(element);

                if (weight + score < 0.0f)
                {
                    elementsToRemove.Add(element);
                    continue;
                }

                if (ElementLooksLikeParagraphDiv(element))
                {
                    // leave the element - it's probably just a div pretending to be a paragraph
                    continue;
                }

                /* If there are not very many commas and the number of non-paragraph elements
                 * is more than paragraphs or other ominous signs, remove the element. */
                var elementInnerText = GetInnerText(element);

                if (GetSegmentsCount(elementInnerText, ',') < MinCommaSegments)
                {
                    var psCount = element.GetElementsByTagName("p").Count();
                    var imgsCount = element.GetElementsByTagName("img").Count();
                    var lisCount = element.GetElementsByTagName("li").Count();
                    var inputsCount = element.GetElementsByTagName("input").Count();

                    // while counting embeds we omit video-embeds
                    var embedsCount =
                        element.GetElementsByTagName("embed")
                            .Count();

                    var linksDensity = GetLinksDensity(element);
                    var innerTextLength = elementInnerText.Length;
                    var elementNameLower = elementName.Trim().ToLower();
                    var remove = (imgsCount > psCount)
                                 ||
                                 (lisCount - LisCountTreshold > psCount && elementNameLower != "ul" &&
                                  elementNameLower != "ol")
                                 || (inputsCount > psCount/3)
                                 ||
                                 (innerTextLength < MinInnerTextLength &&
                                  (imgsCount == 0 || imgsCount > MaxImagesInShortSegmentsCount))
                                 ||
                                 (weight < ClassWeightTreshold &&
                                  linksDensity > MaxDensityForElementsWithSmallerClassWeight)
                                 ||
                                 (weight >= ClassWeightTreshold &&
                                  linksDensity > MaxDensityForElementsWithGreaterClassWeight)
                                 ||
                                 (embedsCount > MaxEmbedsCount ||
                                  (embedsCount == MaxEmbedsCount &&
                                   innerTextLength < MinInnerTextLengthInElementsWithEmbed));

                    if (remove)
                    {
                        elementsToRemove.Add(element);
                    }
                }
            } /* end foreach */

            RemoveElements(elementsToRemove);
        }

        internal void CleanHeaders(XElement element)
        {
            var elementsToRemove = new List<XElement>();

            for (var headerLevel = 1; headerLevel < 7; headerLevel++)
            {
                var headerElements = element.GetElementsByTagName("h" + headerLevel);

                elementsToRemove.AddRange(headerElements.Where(headerElement => GetClassWeight(headerElement)
                    < 0 || GetLinksDensity(headerElement) > MaxHeaderLinksDensity));
            }

            RemoveElements(elementsToRemove);
        }

        /// <summary>
        ///     Removes the style attribute from the specified <paramref name="rootElement" /> and all elements underneath it.
        /// </summary>
        internal void CleanStyles(XElement rootElement)
        {
            new ElementTraveseHelper(
                element =>
                {
                    var elementClass = element.GetClass();

                    if (elementClass.Contains(ReadabilityStyledCssClass))
                    {
                        // don't remove the style if that's we who have styled this element
                        return;
                    }

                    element.SetStyle(null);
                }).Traverse(rootElement);
        }

        internal string GetUserStyleClass(string prefix, string enumStr)
        {
            var suffixSb = new StringBuilder();
            var wasUpperCaseCharacterSeen = false;

            enumStr.Aggregate(
                suffixSb,
                (sb, ch) =>
                {
                    if (char.IsUpper(ch))
                    {
                        if (wasUpperCaseCharacterSeen)
                        {
                            sb.Append('-');
                        }

                        wasUpperCaseCharacterSeen = true;

                        sb.Append(char.ToLower(ch));
                    }
                    else
                    {
                        sb.Append(ch);
                    }

                    return sb;
                });

            return string.Format("{0}-{1}", prefix, suffixSb).TrimEnd('-');
        }


        private static XElement GetOrCreateBody(XDocument document)
        {
            var documentBody = document.GetBody();

            if (documentBody == null)
            {
                var htmlElement = document.GetChildrenByTagName("html").FirstOrDefault();

                if (htmlElement == null)
                {
                    htmlElement = new XElement("html");
                    document.Add(htmlElement);
                }

                documentBody = new XElement("body");
                htmlElement.Add(documentBody);
            }

            return documentBody;
        }

        private static void RemoveElements(IEnumerable<XElement> elementsToRemove)
        {
            elementsToRemove.ForEach(elementToRemove => elementToRemove.Remove());
        }

        private static void ResolveElementsUrls(XDocument document, string tagName, string attributeName, string url,
            Func<AttributeTransformationInput, AttributeTransformationResult> attributeValueTransformer)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            var elements = document.GetElementsByTagName(tagName);

            foreach (var element in elements)
            {
                var attributeValue = element.GetAttributeValue(attributeName, null);

                if (attributeValue == null)
                {
                    continue;
                }

                attributeValue = ResolveElementUrl(attributeValue, url);

                if (!string.IsNullOrEmpty(attributeValue))
                {
                    AttributeTransformationResult attributeTransformationResult;

                    if (attributeValueTransformer != null)
                    {
                        attributeTransformationResult =
                            attributeValueTransformer.Invoke(new AttributeTransformationInput
                            {
                                AttributeValue = attributeValue,
                                Element = element
                            });
                    }
                    else
                    {
                        attributeTransformationResult = new AttributeTransformationResult
                        {
                            TransformedValue = attributeValue
                        };
                    }

                    element.SetAttributeValue(attributeName, attributeTransformationResult.TransformedValue);

                    if (!string.IsNullOrEmpty(attributeTransformationResult.OriginalValueAttributeName))
                    {
                        element.SetAttributeValue(attributeTransformationResult.OriginalValueAttributeName,
                            attributeValue);
                    }
                }
            }
        }

        private static string ResolveElementUrl(string url, string articleUrl)
        {
            if (_MailtoHrefRegex.IsMatch(url))
            {
                return url;
            }

            Uri baseUri;

            if (!Uri.TryCreate(articleUrl, UriKind.Absolute, out baseUri))
            {
                return url;
            }

            /* If the link is simply a query string, then simply attach it to the original URL */
            if (url.StartsWith("?"))
            {
                return baseUri.Scheme + "://" + baseUri.Host + baseUri.AbsolutePath + url;
            }

            Uri absoluteUri;

            if (Uri.TryCreate(baseUri, url, out absoluteUri))
            {
                return absoluteUri.OriginalString;
            }

            return url;
        }

        private static string GetElementName(XElement element)
        {
            return element.Name.LocalName ?? "";
        }

        private static void SetElementName(XElement element, string newLocalName)
        {
            element.Name = XName.Get(newLocalName, element.Name.NamespaceName);
        }

        private static bool ElementLooksLikeParagraphDiv(XElement element)
        {
            var elementName = GetElementName(element);

            if (!"div".Equals(elementName, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!_LikelyParagraphDivRegex.IsMatch(element.GetClass()))
            {
                // we'll consider divs only with certain classes as potential paragraph divs
                return false;
            }

            var childNode = element.Nodes().SingleOrNone();

            var childElement = childNode as XElement;

            if (childElement != null
                && "p".Equals(GetElementName(childElement), StringComparison.OrdinalIgnoreCase))
            {
                // we have a div with a single child element that is a paragraph
                return true;
            }

            return false;
        }

        private static string ExtractTitle(XDocument transcodedXmlDocument)
        {
            var firstH1Element =
                transcodedXmlDocument.Root
                    .GetElementsByTagName("h1").FirstOrDefault();

            var extractedTitle =
                firstH1Element != null
                    ? firstH1Element.Value
                    : null;

            if (!string.IsNullOrEmpty(extractedTitle))
            {
                extractedTitle = _TitleWhitespacesCleanUpRegex.Replace(extractedTitle, " ");
                extractedTitle = extractedTitle.Trim();
            }

            if (extractedTitle != null && extractedTitle.Length == 0)
            {
                extractedTitle = null;
            }

            return extractedTitle;
        }

        private static XElement TryFindArticleContentElement(XDocument document, string articleContentElementHint)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            if (string.IsNullOrEmpty(articleContentElementHint))
            {
                throw new ArgumentException("Argument can't be null nor empty.", "articleContentElementHint");
            }

            return document
                .GetElementsByTagName(articleContentElementHint)
                .FirstOrDefault();
        }

        private string GetReadingStyleClass(ReadingStyle readingStyle)
        {
            return GetUserStyleClass("style", readingStyle.ToString());
        }

        private string GetReadingMarginClass(ReadingMargin readingMargin)
        {
            return GetUserStyleClass("margin", readingMargin.ToString());
        }

        private string GetReadingSizeClass(ReadingSize readingSize)
        {
            return GetUserStyleClass("size", readingSize.ToString());
        }

        private void AddPointsToElementScore(XElement element, int pointsToAdd)
        {
            var currentScore = _elementsScores.ContainsKey(element) ? _elementsScores[element] : 0.0f;

            _elementsScores[element] = currentScore + pointsToAdd;
        }

        private float GetElementScore(XElement element)
        {
            return _elementsScores.ContainsKey(element) ? _elementsScores[element] : 0.0f;
        }

        private void SetElementScore(XElement element, float score)
        {
            _elementsScores[element] = score;
        }
    }
}