using Microsoft.AspNetCore.Components;
using PoundPupLegacy.Pages;
using Xunit;

namespace PoundPupLegacy.Test;


public class PageTests
{
    [Fact]
    public void AllBlazorPagesDeriveFromPage()
    {
        typeof(Program).Assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ComponentBase)))
            .Where(t => t.GetCustomAttributes(true).OfType<RouteAttribute>().Any())
            .ToList()
            .ForEach(t => Assert.True(t.IsSubclassOf(typeof(Page))));
    }
}
