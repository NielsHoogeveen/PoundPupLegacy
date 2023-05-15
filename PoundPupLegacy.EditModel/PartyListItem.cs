namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(PartyItem.PersonListItem))]
public partial class PartyItemPersonListItemJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(PartyItem.PersonName))]
public partial class PartyItemPersonNameJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(PartyItem.OrganizationListItem))]
public partial class PartyItemOrganizationListItemJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(PartyItem.OrganizationName))]
public partial class PartyItemOrganizationNameJsonContext : JsonSerializerContext { }

public abstract record PartyItem 
{
    private PartyItem()
    {
    }
    public abstract string Name { get; set; }

    public abstract T Match<T>(
        Func<PersonListItem, T> personItem, 
        Func<PersonName, T> personName,
        Func<OrganizationListItem, T> organizationItem,
        Func<OrganizationName, T> organizationName);

    public abstract record PersonItem: PartyItem
    {
        public abstract T Match<T>(
            Func<PersonListItem, T> personItem,
            Func<PersonName, T> personName);

    }
    public sealed record PersonListItem : PersonItem, EditListItem
    {
        public required int Id { get; init; }
        public required override string Name { get; set; }
        public override T Match<T>(
            Func<PersonListItem, T> personItem,
            Func<PersonName, T> personName,
            Func<OrganizationListItem, T> organizationItem,
            Func<OrganizationName, T> organizationName)
        {
            return personItem(this);
        }
        public override T Match<T>(
            Func<PersonListItem, T> personItem,
            Func<PersonName, T> personName)
        {
            return personItem(this);
        }
    }
    public sealed record PersonName : PersonItem, NamedOnly
    {
        public required override string Name { get; set; }
        public override T Match<T>(
            Func<PersonListItem, T> personItem,
            Func<PersonName, T> personName,
            Func<OrganizationListItem, T> organizationItem,
            Func<OrganizationName, T> organizationName)
        {
            return personName(this);
        }
        public override T Match<T>(
            Func<PersonListItem, T> personItem,
            Func<PersonName, T> personName)
        {
            return personName(this);
        }

    }
    public abstract record OrganizationItem : PartyItem
    {
        public abstract T Match<T>(
            Func<OrganizationListItem, T> organizationItem,
            Func<OrganizationName, T> organizationName);
    }
    public sealed record OrganizationListItem : OrganizationItem, EditListItem
    {
        public required int Id { get; init; }
        public required override string Name { get; set; }
        public override T Match<T>(
            Func<PersonListItem, T> personItem,
            Func<PersonName, T> personName,
            Func<OrganizationListItem, T> organizationItem,
            Func<OrganizationName, T> organizationName)
        {
            return organizationItem(this);
        }
        public override T Match<T>(
            Func<OrganizationListItem, T> organizationItem,
            Func<OrganizationName, T> organizationName)
        {
            return organizationItem(this);
        }
    }
    public sealed record OrganizationName : OrganizationItem, NamedOnly
    {
        public required override string Name { get; set; }
        public override T Match<T>(
            Func<PersonListItem, T> personItem,
            Func<PersonName, T> personName,
            Func<OrganizationListItem, T> organizationItem,
            Func<OrganizationName, T> organizationName)
        {
            return organizationName(this);
        }
        public override T Match<T>(
            Func<OrganizationListItem, T> organizationItem,
            Func<OrganizationName, T> organizationName)
        {
            return organizationName(this);
        }
    }
}


