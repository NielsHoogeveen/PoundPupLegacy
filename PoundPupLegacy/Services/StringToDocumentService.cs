﻿using HtmlAgilityPack;
using System;
using System.Text;

namespace PoundPupLegacy.Services;

public class StringToDocumentService
{
    public StringToDocumentService() { }

    public HtmlDocument Convert(string text)
    {

        if (!text.Contains("</") && !text.Contains("/>"))
        {
            text = FormatText(text);
        }
        var doc = new HtmlDocument();
        doc.LoadHtml(text);
        var elements = MakeParagraphs(doc).ToList();
        doc.DocumentNode.RemoveAllChildren();
        foreach (var element in elements)
        {
            doc.DocumentNode.ChildNodes.Add(element);
        }
        return doc;
    }

    private string FormatText(string text)
    {
        var stringBuilder = new StringBuilder();
        var i = 0;
        while (i < text.Length)
        {
            var c = text[i];
            if(i == 0)
            {
                stringBuilder.Append("<p>");
            }
            if (i == text.Length - 1)
            {
                stringBuilder.Append(text[i]);
                stringBuilder.Append("</p>");
            }

            if (c == 'h' && (text[i..].StartsWith("http://") || text[i..].StartsWith("https://")))
            {
                var endPos = text[i..].IndexOfAny(new char[] { ' ', '\t', '\n', '\r' });
                var url = endPos == -1 ? 
                    text[i..text.Length] : 
                    text[i..(endPos + i)];
                stringBuilder.Append($@"<a href=""{url}"">{url}</a>");
                i += endPos == -1 ? text.Length: endPos;
                continue;
            }
            else if (text[i]  == '\n')
            {
                stringBuilder.Append("</p><p>");
            }
            else if(text[i] == '\r')
            {
                stringBuilder.Append("</p><p>");
                i++;
            }
            else
            {
                stringBuilder.Append(c);
            }
            i++;
        }
        return stringBuilder.ToString();
    }

    private IEnumerable<HtmlNode> MakeParagraphs(HtmlDocument doc)
    {
        var element = doc.CreateElement("p");
        foreach (var elem in doc.DocumentNode.ChildNodes)
        {
            if (elem is HtmlTextNode textNode)
            {
                element.ChildNodes.Add(textNode.Clone());
            }
            else if (elem.Name == "a")
            {
                element.ChildNodes.Add(elem.Clone());
            }
            else if (elem.Name == "br")
            {
                if (element.ChildNodes.Count > 0)
                {
                    yield return element.Clone();
                    element = doc.CreateElement("p");
                }
            }
            else
            {
                if (element.ChildNodes.Count > 0)
                {
                    yield return element.Clone();
                    element = doc.CreateElement("p");
                }
                yield return elem.Clone();
            }
        }
        if (element.ChildNodes.Count > 0)
        {
            yield return element;
        }
    }
}
