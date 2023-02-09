namespace PoundPupLegacy.Services;

public interface IRazorViewToStringService
{
    public Task<string> GetFromView<T>(string viewName, T model, HttpContext context);
}

