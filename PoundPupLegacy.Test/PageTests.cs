using Microsoft.AspNetCore.Components;
using PoundPupLegacy.Pages;
using Xunit;

namespace PoundPupLegacy.Test;


public class PageTests
{
    [Fact]
    public void AllBlazorPagesExceptIndexDeriveFromPage()
    {
        typeof(Program).Assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ComponentBase)))
            .Where(t => t.GetCustomAttributes(true).OfType<RouteAttribute>().Any())
            .Where(t => t != typeof(PoundPupLegacy.Pages.Index))
            .ToList()
            .ForEach(t => Assert.True(t.IsSubclassOf(typeof(Page))));
    }
}
