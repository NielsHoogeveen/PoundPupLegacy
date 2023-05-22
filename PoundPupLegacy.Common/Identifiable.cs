namespace PoundPupLegacy.Common;

public interface Identifiable: IRequest
{

}
public interface ImmediatelyIdentifiable : Identifiable
{
    public int Id { get; init; }
}

public interface EventuallyIdentifiable : Identifiable
{
    public int? Id { get; set; }
}
