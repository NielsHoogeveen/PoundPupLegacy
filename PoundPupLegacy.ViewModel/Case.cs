namespace PoundPupLegacy.ViewModel;

public interface Case : Nameable, Documentable, Locatable
{
    CaseParties[] CaseParties { get; }
}
