namespace PoundPupLegacy.EditModel.UI;

public abstract record ValidationResult<TUpdateModel, TCreateModel>
    where TCreateModel : class, ResolvedNewNode
    where TUpdateModel : class, ExistingNode
{
    public abstract T Match<T>(Func<Success, T> success, Func<Error, T> error);

    public sealed record Success : ValidationResult<TUpdateModel, TCreateModel>
    {
        public required Node<TUpdateModel, TCreateModel> Node { get; init; }
        public override T Match<T>(Func<Success, T> success, Func<Error, T> error) { 
            return success(this);
        }
    }

    public sealed record Error : ValidationResult<TUpdateModel, TCreateModel>
    {
        public required List<ErrorDetail> Errors { get; init; }
        public override T Match<T>(Func<Success, T> success, Func<Error, T> error)
        {
            return error(this);
        }
    }
}

public sealed record ErrorDetail
{
    public required string Id { get; init; }

    public required string Message { get; init; }
}