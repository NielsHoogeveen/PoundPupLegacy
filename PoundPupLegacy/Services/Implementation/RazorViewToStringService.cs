using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace PoundPupLegacy.Services.Implementation;

internal class RazorViewToStringService : IRazorViewToStringService
{
    private readonly ITempDataProvider _tempDataProvider;
    private readonly IRazorViewEngine _viewEngine;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RazorViewToStringService(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IHttpContextAccessor httpContextAccessor)
    {
        _tempDataProvider = tempDataProvider;
        _viewEngine = viewEngine;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<string> GetFromView<T>(string viewName, T model)
    {

        var context = _httpContextAccessor.HttpContext!;
        var viewResult = _viewEngine.GetView("", viewName, false);
        if (viewResult.Success == false) {
            throw new Exception($"view {viewName} could not be found");
        }
        using var writer = new StringWriter();
        var c = new ActionContext {
            HttpContext = context,
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor {

            },
            RouteData = new RouteData()
        };
        var fact = new TempDataDictionaryFactory(_tempDataProvider);
        var viewContext = new ViewContext(
            c,
            viewResult.View,
            new ViewDataDictionary<T>(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary()) {
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
