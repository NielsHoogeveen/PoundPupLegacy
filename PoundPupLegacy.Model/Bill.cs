namespace PoundPupLegacy.Model;

public interface Bill : Nameable, Documentable
{
    public DateTime? IntroductionDate { get; }

}
