namespace PoundPupLegacy.Common;

public interface Identifiable: IRequest
{
    Identification Identification { get; }
}
public interface ImmediatelyIdentifiable : Identifiable
{
    Identification.IdentificationForUpdate IdentificationForUpdate { get; }
}

public interface EventuallyIdentifiable : Identifiable
{
    Identification.IdentificationForCreate IdentificationForCreate { get; }
}
public abstract record Identification
{
    private Identification() { }
    public sealed record IdentificationForCreate : Identification
    {
        public required int? Id { get; set; }
    }
    public sealed record IdentificationForUpdate : Identification
    {
        public required int Id { get; init; }
    }
}