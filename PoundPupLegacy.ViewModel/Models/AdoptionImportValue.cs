namespace PoundPupLegacy.ViewModel.Models;



public record AdoptionImportValue
{
    public required int Year { get; init; }
    public required int NumberOfChildren { get; init; }
}
