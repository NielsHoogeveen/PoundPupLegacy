using Bunit;
using PoundPupLegacy.ViewModel.UI.Components;
using PoundPupLegacy.ViewModel.Models;
using Xunit;
namespace PoundPupLegacy.Test;

public class AuthoringViewerTest
{

    private static DateTime now = DateTime.Now;
    private static Authoring authoring = new Authoring {
        Id = 1,
        Name = "Test Author",
        ChangedDateTime = now,
        CreatedDateTime = now,
    };

    [Fact]
    public void AuthoringViewRendersDateTimeInddMMMMyyyy()
    {
        var ctx = new TestContext();
        var now = DateTime.Now;
        var param = ComponentParameter.CreateParameter("Model", authoring);
        var aut = ctx.RenderComponent<AuthoringViewer>(param);
        var elem = aut.Find("span.date");
        Assert.Equal(elem.InnerHtml, now.ToString("dddd, dd MMMM yyyy"));
    }
    [Fact]
    public void AuthoringViewRendersUserAsALink()
    {
        var ctx = new TestContext();
        var now = DateTime.Now;
        var param = ComponentParameter.CreateParameter("Model", authoring);
        var aut = ctx.RenderComponent<AuthoringViewer>(param);

        var elem = aut.Find("span.name a");
        Assert.NotEmpty(elem.Attributes);
        var attr = elem.Attributes[0];
        Assert.Equal("href", attr!.Name);
        Assert.Equal("/user/1", attr.Value);
        Assert.Equal("Test Author", elem.InnerHtml);
    }
    [Fact]
    public void AuthoringViewRendersDateTimeInAuthorDiv()
    {
        var ctx = new TestContext();
        var now = DateTime.Now;
        var param = ComponentParameter.CreateParameter("Model", authoring);
        var aut = ctx.RenderComponent<AuthoringViewer>(param);
        var elem = aut.Find("span.date");
        var parent = elem.Parent;
        Assert.NotNull(parent);
        Assert.IsAssignableFrom<AngleSharp.Html.Dom.IHtmlDivElement>(parent);
        AngleSharp.Html.Dom.IHtmlDivElement parentDiv = (parent as AngleSharp.Html.Dom.IHtmlDivElement)!;
        Assert.Equal("author", parentDiv.ClassName);
    }


}


