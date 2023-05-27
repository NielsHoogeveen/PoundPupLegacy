namespace PoundPupLegacy.EditModel.UI.Mappers;

internal interface IMapper<TIn, TOut>
{
    TOut Map(TIn source);
}
internal interface IEnumerableMapper<TIn, TOut>
{
    IEnumerable<TOut> Map(IEnumerable<TIn> source);
}
