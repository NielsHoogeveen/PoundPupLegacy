namespace PoundPupLegacy.Common;

public interface Identifiable: IRequest
{
    Identification Identification { get; }
}
public interface CertainlyIdentifiable : Identifiable
{
    Identification.Certain IdentificationCertain { get; }
}

public interface PossiblyIdentifiable : Identifiable
{
    Identification.Possible IdentificationForCreate { get; }
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