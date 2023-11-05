namespace PoundPupLegacy.Pages;

public abstract partial class NodeViewerPage<TModel>
    where TModel: class, ViewModel.Models.Node
{
    public abstract int Id { get; set; }
}
