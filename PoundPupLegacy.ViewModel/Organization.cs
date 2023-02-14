namespace PoundPupLegacy.ViewModel;

public record Organization : Nameable, Documentable, Locatable
{
    public required string Description { get; init; }
    public required int Id { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Title { get; init; }
    public required Authoring Authoring { get; init; }
    public required bool HasBeenPublished { get; init; }
    private Link[] tags = Array.Empty<Link>();
    public required Link[] Tags
    {
        get => tags;
        init
        {
            if (value is not null)
            {
                tags = value;
            }

        }
    }
    private Comment[] comments = Array.Empty<Comment>();
    public required Comment[] Comments
    {
        get => comments;
        init
        {
            if (value is not null)
            {
                comments = value;
            }
        }
    }

    public required Link[] BreadCrumElements { get; init; }

    private DocumentListItem[] documents = Array.Empty<DocumentListItem>();
    public required DocumentListItem[] Documents
    {
        get => documents;
        init
        {
            if (value is not null)
            {
                documents = value;
            }
        }
    }

    public required string? WebsiteUrl { get; init; }
    public required string? EmailAddress { get; init; }
    public required DateTime? Established { get; init; }
    public required DateTime? Terminated { get; init; }
    private Link[] organizationTypes = Array.Empty<Link>();
    public required Link[] OrganizationTypes
    {
        get => organizationTypes;
        init
        {
            if (value is not null)
            {
                organizationTypes = value;
            }
        }
    }

    public required Location[] Locations { get; init; }

    private Link[] subTopics = Array.Empty<Link>();
    public required Link[] SubTopics
    {
        get => subTopics;
        init
        {
            if (value is not null)
            {
                subTopics= value;
            }
        }
    }

    private Link[] superTopics = Array.Empty<Link>();
    public required Link[] SuperTopics
    {
        get => superTopics;
        init
        {
            if (value is not null)
            {
                superTopics = value;
            }
        }
    }
    private InterOrganizationalRelation[] interOrganizationalRelations = Array.Empty<InterOrganizationalRelation>();
    public required InterOrganizationalRelation[] InterOrganizationalRelations
    {
        get => interOrganizationalRelations;
        init
        {
            if (value is not null)
            {
                interOrganizationalRelations = value;
            }
        }
    }
    private PartyCaseType[] partyCaseTypes = Array.Empty<PartyCaseType>();
    public required PartyCaseType[] PartyCaseTypes
    {
        get => partyCaseTypes;
        init
        {
            if (value is not null)
            {
                partyCaseTypes = value;
            }
        }
    }
    private PersonOrganizationRelation[] personOrganizationRelations = Array.Empty<PersonOrganizationRelation>();
    public required PersonOrganizationRelation[] PersonOrganizationRelations
    {
        get => personOrganizationRelations;
        init
        {
            if (value is not null)
            {
                personOrganizationRelations = value;
            }
        }
    }

}
