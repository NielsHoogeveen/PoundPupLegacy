namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Organization))]
public partial class OrganizationJsonContext : JsonSerializerContext { }

public sealed record Organization : LocatableBase
{

    public string? WebsiteUrl { get; init; }
    public string? EmailAddress { get; init; }
    public FuzzyDate? Establishment { get; init; }
    public FuzzyDate? Termination { get; init; }

    private BasicLink[] organizationTypes = Array.Empty<BasicLink>();
    public BasicLink[] OrganizationTypes {
        get => organizationTypes;
        init {
            if (value is not null) {
                organizationTypes = value;
            }
        }
    }

    private InterOrganizationalRelation[] interOrganizationalRelations = Array.Empty<InterOrganizationalRelation>();
    public InterOrganizationalRelation[] InterOrganizationalRelations {
        get => interOrganizationalRelations;
        init {
            if (value is not null) {
                interOrganizationalRelations = value;
            }
        }
    }
    private PartyCaseType[] partyCaseTypes = Array.Empty<PartyCaseType>();
    public PartyCaseType[] PartyCaseTypes {
        get => partyCaseTypes;
        init {
            if (value is not null) {
                partyCaseTypes = value;
            }
        }
    }
    private PersonOrganizationRelation[] personOrganizationRelations = Array.Empty<PersonOrganizationRelation>();
    public PersonOrganizationRelation[] PersonOrganizationRelations {
        get => personOrganizationRelations;
        init {
            if (value is not null) {
                personOrganizationRelations = value;
            }
        }
    }
    private PartyPoliticalEntityRelation[] partyPoliticalEntityRelations = Array.Empty<PartyPoliticalEntityRelation>();
    public PartyPoliticalEntityRelation[] PartyPoliticalEntityRelations {
        get => partyPoliticalEntityRelations;
        init {
            if (value is not null) {
                partyPoliticalEntityRelations = value;
            }
        }
    }
}
