namespace PoundPupLegacy.ViewModel.Models;

public interface Case : Nameable, Documentable, Locatable
{
    CaseParties[] CaseParties { get; }
    FuzzyDate? Date { get; }
}
