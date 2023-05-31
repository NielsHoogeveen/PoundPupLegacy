namespace PoundPupLegacy.EditModel.UI.Services;

internal interface IEntitySaveService<TUpdateModel, TCreateModel>
    where TUpdateModel : class, ExistingNode
    where TCreateModel : class, ResolvedNewNode
{
    Task<int> SaveAsync(TCreateModel node);
    Task<int> SaveAsync(TUpdateModel node);
}
