using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace PoundPupLegacy.Services.Implementation;

internal class RazorViewToStringService : IRazorViewToStringService
{
    private ITempDataProvider _tempDataProvider;
    private IRazorViewEngine _viewEngine;
    public RazorViewToStringService(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider)
    {
        _tempDataProvider = tempDataProvider;
        _viewEngine = viewEngine;
    }
    public async Task<string> GetFromView<T>(string viewName, T model, HttpContext context)
    {

        var viewResult = _viewEngine.GetView("", viewName, false);
        if (viewResult.Success == false)
        {
            throw new Exception($"view {viewName} could not be found");
        }
        using var writer = new StringWriter();
        var c = new ActionContext
        {
            HttpContext = context,
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor
            {

            },
            RouteData = new RouteData()
        };
        var fact = new TempDataDictionaryFactory(_tempDataProvider);
        var viewContext = new ViewContext(
            c,
            viewResult.View,
            new ViewDataDictionary<T>(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
            {
                Model = model
            },
            fact.GetTempData(context),
            writer,
            new HtmlHelperOptions()
        );
        await viewResult.View.RenderAsync(viewContext);
        return writer.GetStringBuilder().ToString();
    }
}
