using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using ScrapEra.Utils;

namespace ScrapEra.CleanReader
{
    public class CleanReaderCore
    {
        private readonly Dictionary<XElement, float> _elementsScores;
        private readonly SgmlDomFactory _sgmlDomFactory;

        public CleanReaderCore()
        {
            _sgmlDomFactory = new SgmlDomFactory();
            _elementsScores = new Dictionary<XElement, float>();
        }

        internal XDocument TranscodeToXml(string htmlContent, string url, out string extractedTitle)
        {
            if (string.IsNullOrEmpty(htmlContent))
            {
                throw new ArgumentNullException("htmlContent");
            }
            var document = _sgmlDomFactory.BuildDocument(htmlContent);
            PrepareDocument(document);
            if (!string.IsNullOrEmpty(url))
            {
                ResolveElementsUrls(document, "img", "src", url, ImageSourceTranformer);
                ResolveElementsUrls(document, "a", "href", url, AnchorHrefTranformer);
            }
            var articleTitleElement = ExtractArticleTitle(document);
            var articleContentElement = ExtractArticleContent(document, url);
            GlueDocument(document, articleTitleElement, articleContentElement);
            extractedTitle = ExtractTitle(document);
            return document;
        }

        internal void PrepareDocument(XDocument document)
        {
            document
                .DescendantNodes()
                .Where(node => node.NodeType == XmlNodeType.Comment || node.NodeType == XmlNodeType.CDATA)
                .Remove();
            var documentBody = GetOrCreateBody(document);
            var rootElement = document.Root;
            var elementsToRemove = new List<XElement>();
            rootElement.GetElementsByTagName("script")
                .ForEach(scriptElement => { elementsToRemove.Add(scriptElement); });
            RemoveElements(elementsToRemove);
            elementsToRemove.AddRange(rootElement.GetElementsByTagName("noscript"));
            RemoveElements(elementsToRemove);
            elementsToRemove.AddRange(
                rootElement.GetElementsByTagName("link")
                    .Where(element => element.GetAttributeValue("rel", "").Trim().ToLower() == "stylesheet"
                                      &&
                                      element.GetAttributeValue("href", "")
                                          .LastIndexOf("readability", StringComparison.Ordinal) == -1));
            RemoveElements(elementsToRemove);
            elementsToRemove.AddRange(rootElement.GetElementsByTagName("style"));
            RemoveElements(elementsToRemove);
            elementsToRemove.AddRange(rootElement.GetElementsByTagName("nav"));
            RemoveElements(elementsToRemove);
            var anchorElements =
                rootElement.GetElementsByTagName("a")
                    .Where(aElement => aElement.Attribute("name") != null && aElement.Attribute("href") == null);
            elementsToRemove.AddRange(anchorElements);
            RemoveElements(elementsToRemove);
            var bodyInnerHtml = documentBody.GetInnerHtml();
            bodyInnerHtml = _replaceDoubleBrsRegex.Replace(bodyInnerHtml, "</p><p>");
            documentBody.SetInnerHtml(bodyInnerHtml);
        }

        internal XElement ExtractArticleTitle(XDocument document)
        {
            var documentBody = GetOrCreateBody(document);
            var documentTitle = document.GetTitle() ?? "";
            var currentTitle = documentTitle;
            if (_articleTitleDashRegex1.IsMatch(currentTitle))
            {
                currentTitle = _articleTitleDashRegex2.Replace(documentTitle, "$1");
                if (currentTitle.Split(' ').Length < MinArticleTitleWordsCount1)
                {
                    currentTitle = _articleTitleDashRegex3.Replace(documentTitle, "$1");
                }
            }
            else if (currentTitle.IndexOf(": ", StringComparison.Ordinal) != -1)
            {
                currentTitle = _articleTitleColonRegex1.Replace(documentTitle, "$1");
                if (currentTitle.Split(' ').Length < MinArticleTitleWordsCount1)
                {
                    currentTitle = _articleTitleColonRegex2.Replace(documentTitle, "$1");
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
            var headElement = document.GetElementsByTagName("head").FirstOrDefault();
            if (headElement == null)
            {
                headElement = new XElement("head");
                documentBody.AddBeforeSelf(headElement);
            }
            var styleElement = new XElement("style");
            styleElement.SetAttributeValue("type", "text/css");
            var readabilityStylesheetStream =
                Assembly.GetExecutingAssembly().GetManifestResourceStream(StylesheetResourceName);
            if (readabilityStylesheetStream == null)
            {
                throw new Exception("Couldn't load the ScrapEra stylesheet embedded resource.");
            }
            using (var sr = new StreamReader(readabilityStylesheetStream))
            {
                styleElement.SetInnerHtml(sr.ReadToEnd());
            }
            headElement.Add(styleElement);
            /* Create inner div. */
            var innerDiv = new XElement("div");
            innerDiv.SetId(InnerDivId);
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
                        && _unlikelyCandidatesRegex.IsMatch(unlikelyMatchString)
                        && !_okMaybeItsACandidateRegex.IsMatch(unlikelyMatchString))
                    {
                        var parentElement = element.Parent;
                        if (parentElement != null)
                        {
                            element.Remove();
                        }
                        return;
                    }

                    if (!"div".Equals(elementName, StringComparison.OrdinalIgnoreCase)) return;
                    if (!_divToPElementsRegex.IsMatch(element.GetInnerHtml()))
                    {
                        SetElementName(element, "p");
                    }
                    else
                    {
                        new ChildNodesTraverser(
                            childNode =>
                            {
                                if (childNode.NodeType != XmlNodeType.Text
                                    || GetInnerText(childNode).Length == 0)
                                {
                                    return;
                                }
                                var paraElement = new XElement("p");
                                paraElement.SetInnerHtml(((XText) childNode).Value);
                                childNode.ReplaceWith(paraElement);
                            }
                            ).Traverse(element);
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
                    if (elementName.Equals("div", StringComparison.OrdinalIgnoreCase))
                    {
                        var childNode = element.Nodes().SingleOrNone();
                        var childElement = childNode as XElement;
                        if (childElement == null ||
                            !GetElementName(childElement).Equals("p", StringComparison.OrdinalIgnoreCase)) return;
                        var parentElement = element.Parent;
                        if (parentElement == null) return;
                        element.AddBeforeSelf(childElement);
                        element.Remove();
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
                if (grandParentElement == null ||
                    "html".Equals(grandParentElement.Name.LocalName, StringComparison.OrdinalIgnoreCase)) continue;
                candidateElements.Add(grandParentElement);
                AddPointsToElementScore(grandParentElement, score/2);
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
            if (topCandidateElement != null &&
                !"body".Equals(topCandidateElement.Name.LocalName, StringComparison.OrdinalIgnoreCase))
                return topCandidateElement;
            topCandidateElement = new XElement("div");
            var documentBody = GetOrCreateBody(document);
            topCandidateElement.Add(documentBody.Nodes());
            return topCandidateElement;
        }

        internal XElement CreateArticleContentElement(XDocument document, XElement topCandidateElement)
        {
            /* Now that we have the top candidate, look through its siblings for content that might also be related.
             * Things like preambles, content split by ads that we removed, etc. */
            var articleContentElement = new XElement("div");
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
                else if (GetElementScore(siblingElement) + contentBonus >= siblingScoreThreshold)
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
                                     && _endOfSentenceRegex.IsMatch(siblingElementInnerText);
                        }
                    }
                }
                if (!append) continue;
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
            var paraElements = articleContentElement.GetElementsByTagName("p");
            var elementsToRemove = (from paraElement in paraElements
                let innerText = GetInnerText(paraElement)
                where innerText.Length <= 0
                let imgsCount = paraElement.GetElementsByTagName("img").Count()
                where imgsCount <= 0
                let embedsCount = paraElement.GetElementsByTagName("embed").Count()
                where embedsCount <= 0
                let objectsCount = paraElement.GetElementsByTagName("object").Count()
                where objectsCount <= 0
                select paraElement).ToList();
            RemoveElements(elementsToRemove);
            /* Remove br's that are directly before paragraphs. */
            articleContentElement.SetInnerHtml(_breakBeforeParagraphRegex.Replace(articleContentElement.GetInnerHtml(),
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

        internal int GetClassWeight(XElement element)
        {
            var weight = 0;
            var elementClass = element.GetClass();
            if (elementClass.Length > 0)
            {
                if (_negativeWeightRegex.IsMatch(elementClass))
                {
                    weight -= 25;
                }
                if (_positiveWeightRegex.IsMatch(elementClass))
                {
                    weight += 25;
                }
            }
            var elementId = element.GetId();
            if (elementId.Length <= 0) return weight;
            if (_negativeWeightRegex.IsMatch(elementId))
            {
                weight -= 25;
            }
            if (_positiveWeightRegex.IsMatch(elementId))
            {
                weight += 25;
            }
            return weight;
        }

        internal string GetInnerText(XNode node)
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
            return _normalizeSpacesRegex.Replace(result, " ");
        }

        internal void KillBreaks(XElement element)
        {
            element.SetInnerHtml(_killBreaksRegex.Replace(element.GetInnerHtml(), "<br />"));
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
                if (GetSegmentsCount(elementInnerText, ',') >= MinCommaSegments) continue;
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
                              linksDensity > MaxDensityForElementsWithGreaterClassWeight) ||
                             embedsCount > MaxEmbedsCount || (embedsCount == MaxEmbedsCount &&
                                                              innerTextLength < MinInnerTextLengthInElementsWithEmbed);
                if (remove)
                {
                    elementsToRemove.Add(element);
                }
            }
            RemoveElements(elementsToRemove);
        }

        internal void CleanHeaders(XElement element)
        {
            var elementsToRemove = new List<XElement>();
            for (var headerLevel = 1; headerLevel < 7; headerLevel++)
            {
                var headerElements = element.GetElementsByTagName("h" + headerLevel);
                elementsToRemove.AddRange(headerElements.Where(headerElement => GetClassWeight(headerElement)
                                                                                < 0 ||
                                                                                GetLinksDensity(headerElement) >
                                                                                MaxHeaderLinksDensity));
            }
            RemoveElements(elementsToRemove);
        }

        /// <summary>
        ///     Removes the style attribute from the specified <paramref name="rootElement" /> and all elements underneath it.
        /// </summary>
        internal void CleanStyles(XElement rootElement)
        {
            new ElementTraveseHelper(
                element => { element.SetStyle(null); }).Traverse(rootElement);
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
            if (documentBody != null) return documentBody;
            var htmlElement = document.GetChildrenByTagName("html").FirstOrDefault();
            if (htmlElement == null)
            {
                htmlElement = new XElement("html");
                document.Add(htmlElement);
            }
            documentBody = new XElement("body");
            htmlElement.Add(documentBody);
            return documentBody;
        }

        private static void RemoveElements(List<XElement> elementsToRemove)
        {
            elementsToRemove.ForEach(elementToRemove => elementToRemove.Remove());
            elementsToRemove.Clear();
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
                if (string.IsNullOrEmpty(attributeValue)) continue;
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

        private static string ResolveElementUrl(string url, string articleUrl)
        {
            if (MailtoHrefRegex.IsMatch(url))
            {
                return url;
            }
            Uri baseUri;
            if (!Uri.TryCreate(articleUrl, UriKind.Absolute, out baseUri))
            {
                return url;
            }
            if (url.StartsWith("?"))
            {
                return baseUri.Scheme + "://" + baseUri.Host + baseUri.AbsolutePath + url;
            }
            Uri absoluteUri;
            return Uri.TryCreate(baseUri, url, out absoluteUri) ? absoluteUri.OriginalString : url;
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
            if (!LikelyParagraphDivRegex.IsMatch(element.GetClass()))
            {
                // we'll consider divs only with certain classes as potential paragraph divs
                return false;
            }
            var childNode = element.Nodes().SingleOrNone();
            var childElement = childNode as XElement;
            return childElement != null
                   && "p".Equals(GetElementName(childElement), StringComparison.OrdinalIgnoreCase);
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
                extractedTitle = TitleWhitespacesCleanUpRegex.Replace(extractedTitle, " ").Trim();
            }
            if (extractedTitle != null && extractedTitle.Length == 0)
            {
                extractedTitle = null;
            }
            return extractedTitle;
        }

        private static XElement TryFindArticleContentElement(XContainer document, string articleContentElementHint)
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

        #region "Class Fields"

        internal const string OverlayDivId = Constants.OverlayDivId;
        internal const string InnerDivId = Constants.InnerDivId;
        private const int MinParagraphLength = Constants.MinParagraphLength;
        private const int MinInnerTextLength = Constants.MinInnerTextLength;
        private const int ParagraphSegmentLength = Constants.ParagraphSegmentLength;
        private const int MaxPointsForSegmentsCount = Constants.MaxPointsForSegmentsCount;
        private const int MinSiblingParagraphLength = Constants.MinSiblingParagraphLength;
        private const int MinCommaSegments = Constants.MinCommaSegments;
        private const int LisCountTreshold = Constants.LisCountTreshold;
        private const int MaxImagesInShortSegmentsCount = Constants.MaxImagesInShortSegmentsCount;
        private const int MinInnerTextLengthInElementsWithEmbed = Constants.MinInnerTextLengthInElementsWithEmbed;
        private const int ClassWeightTreshold = Constants.ClassWeightTreshold;
        private const int MaxEmbedsCount = Constants.MaxEmbedsCount;
        private const int MaxArticleTitleLength = Constants.MaxArticleTitleLength;
        private const int MinArticleTitleLength = Constants.MinArticleTitleLength;
        private const int MinArticleTitleWordsCount1 = Constants.MinArticleTitleWordsCount1;
        private const int MinArticleTitleWordsCount2 = Constants.MinArticleTitleWordsCount2;
        private const float SiblingScoreTresholdCoefficient = Constants.SiblingScoreTresholdCoefficient;
        private const float MaxSiblingScoreTreshold = Constants.MaxSiblingScoreTreshold;
        private const float MaxSiblingParagraphLinksDensity = Constants.MaxSiblingParagraphLinksDensity;
        private const float MaxHeaderLinksDensity = Constants.MaxHeaderLinksDensity;

        private const float MaxDensityForElementsWithSmallerClassWeight =
            Constants.MaxDensityForElementsWithSmallerClassWeight;

        private const float MaxDensityForElementsWithGreaterClassWeight =
            Constants.MaxDensityForElementsWithGreaterClassWeight;

        private static readonly string StylesheetResourceName = Constants.StylesheetResourceName;
        public Func<AttributeTransformationInput, AttributeTransformationResult> ImageSourceTranformer { get; set; }
        public Func<AttributeTransformationInput, AttributeTransformationResult> AnchorHrefTranformer { get; set; }

        #endregion

        #region "Compiled Regex"

        private readonly Regex _unlikelyCandidatesRegex =
            Constants.UnlikelyCandidatesRegex;

        private readonly Regex _okMaybeItsACandidateRegex = Constants.OkMaybeItsACandidateRegex;

        private readonly Regex _positiveWeightRegex = Constants.PositiveWeightRegex;

        private readonly Regex _negativeWeightRegex = Constants.NegativeWeightRegex;

        private readonly Regex _divToPElementsRegex = Constants.DivToPElementsRegex;

        private readonly Regex _endOfSentenceRegex = Constants.EndOfSentenceRegex;

        private readonly Regex _breakBeforeParagraphRegex = Constants.BreakBeforeParagraphRegex;
        private readonly Regex _normalizeSpacesRegex = Constants.NormalizeSpacesRegex;

        private readonly Regex _killBreaksRegex = Constants.KillBreaksRegex;

        private readonly Regex _replaceDoubleBrsRegex = Constants.ReplaceDoubleBrsRegex;

        private readonly Regex _articleTitleDashRegex1 = Constants.ArticleTitleDashRegex1;
        private readonly Regex _articleTitleDashRegex2 = Constants.ArticleTitleDashRegex2;

        private readonly Regex _articleTitleDashRegex3 = Constants.ArticleTitleDashRegex3;

        private readonly Regex _articleTitleColonRegex1 = Constants.ArticleTitleColonRegex1;
        private readonly Regex _articleTitleColonRegex2 = Constants.ArticleTitleColonRegex2;

        private static readonly Regex LikelyParagraphDivRegex = Constants.LikelyParagraphDivRegex;

        private static readonly Regex MailtoHrefRegex = Constants.MailtoHrefRegex;

        private static readonly Regex TitleWhitespacesCleanUpRegex = Constants.TitleWhitespacesCleanUpRegex;

        #endregion
    }
}