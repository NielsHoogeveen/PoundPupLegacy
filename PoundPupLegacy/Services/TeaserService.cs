namespace PoundPupLegacy.Services;

public class TeaserService
{
    StringToDocumentService _stringToDocumentService;
    public TeaserService(StringToDocumentService stringToDocumentService)
    {
        _stringToDocumentService = stringToDocumentService;
    }

    public string MakeTeaser(string text)
    {
        var doc = _stringToDocumentService.Convert(text);
        var res = doc.DocumentNode.ChildNodes.Take(5).Aggregate("", (a, b) => a + b.OuterHtml.Replace(@"href=""http://poundpuplegacy.org", @"href="""));
        return res;
    }
}
