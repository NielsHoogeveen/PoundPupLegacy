﻿namespace PoundPupLegacy.ViewModel;

public record Person : Nameable, Documentable, Locatable
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

    public required DateTime? DateOfBirth { get; init; }
    public required DateTime? DateOfDeath { get; init; }
    public required string? PortraitFilePath { get; init; }
    public required string? FirstName { get; init; }
    public required string? MiddleName { get; init; }
    public required string? LastName { get; init; }
    public required string? FullName { get; init; }
    public required string? Suffix { get; init; }
    public required string? NickName { get; init; }
    private Link[] professions = Array.Empty<Link>();
    public required Link[] Professions
    {
        get => professions;
        init
        {
            if (value is not null)
            {
                professions = value;
            }
        }
    }

    private Location[] _locations = Array.Empty<Location>();
    public required Location[] Locations { 
        get => _locations; 
        init
        {
            if(value is not null)
            {
                _locations = value;
            }
        }
    }

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
    private InterPersonalRelation[] interPersonalRelations = Array.Empty<InterPersonalRelation>();
    public required InterPersonalRelation[] InterPersonalRelations
    {
        get => interPersonalRelations;
        init
        {
            if (value is not null)
            {
                interPersonalRelations = value;
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
    private OrganizationPersonRelation[] organizationPersonRelations = Array.Empty<OrganizationPersonRelation>();
    public required OrganizationPersonRelation[] OrganizationPersonRelations
    {
        get => organizationPersonRelations;
        init
        {
            if (value is not null)
            {
                organizationPersonRelations = value;
            }
        }
    }
    private PartyPoliticalEntityRelation[] partyPoliticalEntityRelations = Array.Empty<PartyPoliticalEntityRelation>();
    public required PartyPoliticalEntityRelation[] PartyPoliticalEntityRelations
    {
        get => partyPoliticalEntityRelations;
        init
        {
            if (value is not null)
            {
                partyPoliticalEntityRelations = value;
            }
        }
    }
    private File[] _files = Array.Empty<File>();
    public required File[] Files
    {
        get => _files;
        init
        {
            if (value is not null)
            {
                _files = value;
            }
        }
    }

}