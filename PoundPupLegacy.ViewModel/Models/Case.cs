using PoundPupLegacy.Common;

namespace PoundPupLegacy.ViewModel.Models;

public interface Case : Nameable, Documentable, Locatable
{
    CaseParties[] CaseParties { get; }
    FuzzyDate? FuzzyDate { get; }
}
