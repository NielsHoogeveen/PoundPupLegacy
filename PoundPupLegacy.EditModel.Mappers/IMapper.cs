namespace PoundPupLegacy.EditModel.Mappers;

public interface IMapper<TIn, TOut>
{
    TOut Map(TIn source);
}
public interface IEnumerableMapper<TIn, TOut>
{
    IEnumerable<TOut> Map(IEnumerable<TIn> source);
}
