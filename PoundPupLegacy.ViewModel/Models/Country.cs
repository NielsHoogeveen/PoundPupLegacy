namespace PoundPupLegacy.ViewModel.Models;

public abstract record CountryBase: NameableBase, Country{
    public required AdoptionImports AdoptionImports { get; init; }

    private OrganizationTypeWithOrganizations[] organizationTypes = Array.Empty<OrganizationTypeWithOrganizations>();
    public required OrganizationTypeWithOrganizations[] OrganizationTypes {
        get => organizationTypes;
        init {
            if (value is not null) {
                organizationTypes = value;
            }
        }
    }
    private CountrySubdivisionType[] subdivisionTypes = Array.Empty<CountrySubdivisionType>();
    public required CountrySubdivisionType[] SubdivisionTypes {
        get => subdivisionTypes;
        init {
            if (value is not null) {
                subdivisionTypes = value;
            }
        }
    }
    public required Image FlagImage { get; init; }
}

public interface Country : PoliticalEntity
{
    AdoptionImports AdoptionImports { get; }

    OrganizationTypeWithOrganizations[] OrganizationTypes { get; }

    CountrySubdivisionType[] SubdivisionTypes { get; }

    Image FlagImage { get; }

}
