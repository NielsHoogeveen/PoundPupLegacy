namespace PoundPupLegacy.CreateModel;

public interface Bill : Nameable, Documentable
{
    public DateTime? IntroductionDate { get; }

}
