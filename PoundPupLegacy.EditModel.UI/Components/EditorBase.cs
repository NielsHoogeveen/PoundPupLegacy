using Microsoft.AspNetCore.Components;
using PoundPupLegacy.Common.UI.Components;

namespace PoundPupLegacy.EditModel.UI.Components;

public abstract class EditorBase : ViewerBase
{
    [CascadingParameter(Name = "NodeId")]
    public int? NodeId { get; set; }
}

public abstract class EditorDetailBase: EditorBase
{
    public abstract Task<List<System.ComponentModel.DataAnnotations.ValidationResult>> Validate();

    public abstract void OnTitleChange(string title);
}

public abstract class EntityEditorBase<TUpdateModel, TCreateModel, TResolveData> : EditorBase
    where TUpdateModel : class, ExistingNode
    where TCreateModel : class, ResolvedNewNode
{
    protected abstract Resolver<TUpdateModel, TCreateModel, TResolveData> Model { get; }

    protected abstract List<EditorDetailBase?> DetailsEditors { get; }

    protected abstract TResolveData ResolveData { get; }

    protected virtual void OnTitleChanged(string title)
    {
        foreach(var editor in DetailsEditors) {
            editor?.OnTitleChange(title);
        }
    }

    protected async Task<ValidationResult<TUpdateModel, TCreateModel>> Validate()
    {
        var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        foreach (var editor in DetailsEditors) {
            if (editor is null)
                throw new NullReferenceException("editor should not be null");
            errors.AddRange(await editor.Validate());
        }
        if (errors.Any()) {
            return new ValidationResult<TUpdateModel, TCreateModel>.Error {
                Errors = errors
            };
        }
        return new ValidationResult<TUpdateModel, TCreateModel>.Success {
            Node = Model.Resolve(ResolveData)
        };
    }
}
