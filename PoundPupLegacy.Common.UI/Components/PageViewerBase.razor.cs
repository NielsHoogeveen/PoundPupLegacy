namespace PoundPupLegacy.Common.UI.Components;

public abstract partial class PageViewerBase: ViewerBase
{
    protected abstract string Title { get; }
    protected abstract string? Description { get; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
    }

}
