namespace PoundPupLegacy.ViewModel.Models;

public abstract record FormalSubdivisionBase : SubdivisionBase
{
    
    public required string ISO3166_2_Code { get; init; }

}
