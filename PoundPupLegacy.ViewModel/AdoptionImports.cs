using System.ComponentModel.DataAnnotations;

namespace PoundPupLegacy.ViewModel;

public record AdoptionImports
{
    public required int StartYear { get; init; }

    public required int EndYear { get; init; }

    private AdoptionImport[] imports = Array.Empty<AdoptionImport>();
    public required AdoptionImport[] Imports 
    { 
        get => imports; 
        init 
        {
            if(value is not null) 
            {
                imports = value;
            }
        }
    }
}
