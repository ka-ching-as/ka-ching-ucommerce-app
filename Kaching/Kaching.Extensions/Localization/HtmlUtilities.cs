//using HtmlAgilityPack;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Kaching.Extensions.Localization
{

    public class HtmlUtilities
    {
        /// <summary>
        /// Converts HTML to plain text / strips tags.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static string ConvertToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            const string paragraph = @"<(p)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var paragraphRegex = new Regex(paragraph, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, "\n");
            text = paragraphRegex.Replace(text, "\n\n");
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }


        /// <summary>
        /// Count the words.
        /// The content has to be converted to plain text before (using ConvertToPlainText).
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns></returns>
        public static int CountWords(string plainText)
        {
            return !String.IsNullOrEmpty(plainText) ? plainText.Split(' ', '\n').Length : 0;
        }


        public static string Cut(string text, int length)
        {
            if (!String.IsNullOrEmpty(text) && text.Length > length)
            {
                text = text.Substring(0, length - 4) + " ...";
            }
            return text;
        }


        //private static void ConvertContentTo(HtmlNode node, TextWriter outText)
        //{
        //    foreach (HtmlNode subnode in node.ChildNodes)
        //    {
        //        ConvertTo(subnode, outText);
        //    }
        //}


        //private static void ConvertTo(HtmlNode node, TextWriter outText)
        //{
        //    string html;
        //    switch (node.NodeType)
        //    {
        //        case HtmlNodeType.Comment:
        //            // don't output comments
        //            break;

        //        case HtmlNodeType.Document:
        //            ConvertContentTo(node, outText);
        //            break;

        //        case HtmlNodeType.Text:
        //            // script and style must not be output
        //            string parentName = node.ParentNode.Name;
        //            if ((parentName == "script") || (parentName == "style"))
        //                break;

        //            // get text
        //            html = ((HtmlTextNode)node).Text;

        //            // is it in fact a special closing node output as text?
        //            if (HtmlNode.IsOverlappedClosingElement(html))
        //                break;

        //            // check the text is meaningful and not a bunch of whitespaces
        //            if (html.Trim().Length > 0)
        //            {
        //                outText.Write(HtmlEntity.DeEntitize(html));
        //            }
        //            break;

        //        case HtmlNodeType.Element:
        //            switch (node.Name)
        //            {
        //                case "p":
        //                    // treat paragraphs as crlf
        //                    outText.Write("\r\n");
        //                    break;
        //                case "br":
        //                    outText.Write("\r\n");
        //                    break;
        //            }

        //            if (node.HasChildNodes)
        //            {
        //                ConvertContentTo(node, outText);
        //            }
        //            break;
        //    }
        //}
    }
}
