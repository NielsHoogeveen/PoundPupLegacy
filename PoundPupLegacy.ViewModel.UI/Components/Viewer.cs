using PoundPupLegacy.Common.UI.Components;

namespace PoundPupLegacy.ViewModel.UI.Components;

public abstract class Viewer: PageViewerBase
{
}
public abstract class NodeViewerBase<TModel>: Viewer
    where TModel: class, Node
{
    public abstract TModel Model { get; set; }

    protected abstract string ItemDescription { get; }
    protected override string? Description => $"{ItemDescription} {topicsDecription}";
    protected sealed override string Title => Model.Title;

    private List<string> Keywords => Model.Tags.Select(x => x.Title).ToList();
    private string topicsDecription => Keywords.Any()
        ? Keywords.Count > 1
            ? $"about {string.Join(',', Keywords.Take(Keywords.Count - 1)).Replace(",", ", ")} and {Keywords.TakeLast(1).ToList()[0]}"
            : $"about {Keywords[0]}"
        : "";

}
public abstract class SimpleTextNodeViewerBase<TModel> : NodeViewerBase<TModel>
    where TModel: class, SimpleTextNode
{

}
public abstract class CountryViewerBase<TModel> : NodeViewerBase<TModel>
    where TModel : class, Country
{
    protected override string? Description => $"Detailed information about the country {Model.Title}";
    protected override string ItemDescription => "";

}
public abstract class SubdivisionViewerBase<TModel> : NodeViewerBase<TModel>
    where TModel : class, Subdivision
{

    protected override string? Description => $"Detailed information about the subdivision {Model?.Title}";

    protected override string ItemDescription => "";
}

public abstract class SimpleNameableViewerBase<TModel> : NodeViewerBase<TModel>
    where TModel : class, Nameable
{

    protected override string? Description => $"Detailed information about and documents related to the topic {Model?.Title}";

    protected override string ItemDescription => "";
}