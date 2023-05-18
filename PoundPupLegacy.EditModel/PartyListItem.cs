namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(PersonListItem))]
public partial class PersonListItemJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(PersonName))]
public partial class PersonNameJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(OrganizationListItem))]
public partial class OrganizationListItemJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(OrganizationName))]
public partial class OrganizationNameJsonContext : JsonSerializerContext { }

public abstract record OrganizationItem
{
    private OrganizationItem()
    {
    }
    public abstract string Name { get; set; }

    [RequireNamedArgs]
    public abstract T Match<T>(
        Func<OrganizationListItem, T> organizationListItem,
        Func<OrganizationName, T> organizationName);

    public sealed record OrganizationListItem : OrganizationItem, EditListItem
    {
        public required int Id { get; init; }
        public required override string Name { get; set; }
        public override T Match<T>(
            Func<OrganizationListItem, T> organizationListItem,
            Func<OrganizationName, T> organizationName)
        {
            return organizationListItem(this);
        }
    }
    public sealed record OrganizationName : OrganizationItem, NamedOnly
    {
        public required override string Name { get; set; }
        public override T Match<T>(
            Func<OrganizationListItem, T> organizationListItem,
            Func<OrganizationName, T> organizationName)
        {
            return organizationName(this);
        }
    }
}

public abstract record PersonItem
{
    private PersonItem()
    {
    }

    public abstract string Name { get; set; }

    [RequireNamedArgs]
    public abstract T Match<T>(
        Func<PersonListItem, T> personListItem,
        Func<PersonName, T> personName);

    public sealed record PersonListItem : PersonItem, EditListItem
    {
        public required int Id { get; init; }
        public required override string Name { get; set; }
        public override T Match<T>(
            Func<PersonListItem, T> personListItem,
            Func<PersonName, T> personName)
        {
            return personListItem(this);
        }
    }
    public sealed record PersonName : PersonItem, NamedOnly
    {
        public required override string Name { get; set; }
        public override T Match<T>(
            Func<PersonListItem, T> personListItem,
            Func<PersonName, T> personName)
        {
            return personName(this);
        }
    }
}



