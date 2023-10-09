using PoundPupLegacy.ViewModel.Readers;

namespace PoundPupLegacy.ViewModel.Models;

public abstract record CaseBase: LocatableBase, Case
{
    private CaseParties[] _caseParties = Array.Empty<CaseParties>();
    public required CaseParties[] CaseParties {
        get => _caseParties;
        init {
            if (value is not null) {
                _caseParties = value;
            }
        }
    }
    public FuzzyDate? Date { get; set; }

}
public interface Case : Nameable, Documentable, Locatable
{
    CaseParties[] CaseParties { get; }
    FuzzyDate? Date { get; }
}
