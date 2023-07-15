namespace PoundPupLegacy.DomainModel;

public interface MenuItem : PossiblyIdentifiable
{
    public double Weight { get; }
}
