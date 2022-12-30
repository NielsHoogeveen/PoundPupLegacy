namespace PoundPupLegacy.Model;

public interface Party : Documentable, Locatable
{
    public bool IsTopic { get; }
}
