﻿namespace PoundPupLegacy.EditModel;

public record Person : Party
{
    public int? NodeId { get; init; }
    public int? UrlId { get; set; }
    public required string NodeTypeName { get; set; }
    public required string Title { get; set; }
    public required int PublisherId { get; set; }
    public required int OwnerId { get; set; }
    public required string Description { get; set; }

    private List<Tags> tags = new();

    public List<Tags> Tags {
        get => tags;
        init {
            if (value is not null) {
                tags = value;
            }
        }
    }
    private List<TenantNode> tenantNodes = new();

    public List<TenantNode> TenantNodes {
        get => tenantNodes;
        init {
            if (value is not null) {
                tenantNodes = value;
            }
        }
    }
    private List<Tenant> tenants = new();

    public List<Tenant> Tenants {
        get => tenants;
        init {
            if (value is not null) {
                tenants = value;
            }
        }
    }
    private List<File> files = new();

    public List<File> Files {
        get => files;
        init {
            if (value is not null) {
                files = value;
            }
        }
    }
    private List<Location> locations = new();

    public List<Location> Locations {
        get => locations;
        init {
            if (value is not null) {
                locations = value;
            }
        }
    }
    private List<Term> terms = new();

    public List<Term> Terms {
        get => terms;
        init {
            if (value is not null) {
                terms = value;
            }
        }
    }
    private List<OrganizationOrganizationType> organizationOrganizationTypes = new();

    public List<OrganizationOrganizationType> OrganizationOrganizationTypes {
        get => organizationOrganizationTypes;
        init {
            if (value is not null) {
                organizationOrganizationTypes = value;
            }
        }
    }
    private List<OrganizationType> organizationTypes = new();

    public List<OrganizationType> OrganizationTypes {
        get => organizationTypes;
        init {
            if (value is not null) {
                organizationTypes = value;
            }
        }
    }
    private List<CountryListItem> countries = new();

    public List<CountryListItem> Countries {
        get => countries;
        init {
            if (value is not null) {
                countries = value;
            }
        }
    }


}