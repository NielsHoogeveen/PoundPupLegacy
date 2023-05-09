namespace PoundPupLegacy.EditModel.UI.Services;

public interface ISearchService<T>
    where T : EditListItem
{
    Task<List<T>> GetItems(int tenantId, string searchString);

}
