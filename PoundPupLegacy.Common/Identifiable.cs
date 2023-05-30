namespace PoundPupLegacy.Common;

public interface Identifiable: IRequest
{
}
public interface CertainlyIdentifiable : Identifiable
{
    Identification.Certain Identification { get; }
}

public interface PossiblyIdentifiable : Identifiable
{
    Identification.Possible Identification { get; }
}
public abstract record Identification
{
    private Identification() { }
    public sealed record Possible : Identification
    {
        public required int? Id { get; set; }
    }
    public sealed record Certain : Identification
    {
        public required int Id { get; init; }
    }
}