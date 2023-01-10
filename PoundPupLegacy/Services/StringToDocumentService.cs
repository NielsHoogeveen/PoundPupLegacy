using HtmlAgilityPack;

namespace PoundPupLegacy.Services;

public class StringToDocumentService
{
    public StringToDocumentService() { }

    public HtmlDocument Convert(string text)
    {
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
