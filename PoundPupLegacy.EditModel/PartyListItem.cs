namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(PersonItem.PersonListItem))]
public partial class PartyItemPersonListItemJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(PersonItem.PersonName))]
public partial class PartyItemPersonNameJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(OrganizationItem.OrganizationListItem))]
public partial class PartyItemOrganizationListItemJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(OrganizationItem.OrganizationName))]
public partial class PartyItemOrganizationNameJsonContext : JsonSerializerContext { }

public abstract record OrganizationItem 
{
    private OrganizationItem()
    {
    }
    public abstract string Name { get; set; }

    public abstract T Match<T>(
        Func<OrganizationListItem, T> personItem,
        Func<OrganizationName, T> personName);

    public sealed record OrganizationListItem : OrganizationItem, EditListItem
    {
        public required int Id { get; init; }
        public required override string Name { get; set; }
        public override T Match<T>(
            Func<OrganizationListItem, T> personItem,
            Func<OrganizationName, T> personName)
        {
            return personItem(this);
        }
    }
    public sealed record OrganizationName : OrganizationItem, NamedOnly
    {
        public required override string Name { get; set; }
        public override T Match<T>(
            Func<OrganizationListItem, T> personItem,
            Func<OrganizationName, T> personName)
        {
            return personName(this);
        }
    }
}

public abstract record PersonItem
{
    private PersonItem()
    {
    }
    public abstract string Name { get; set; }

    public abstract T Match<T>(
        Func<PersonListItem, T> personItem,
        Func<PersonName, T> personName);

    public sealed record PersonListItem : PersonItem, EditListItem
    {
        public required int Id { get; init; }
        public required override string Name { get; set; }
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
            Func<PersonName, T> personName)
        {
            return personName(this);
        }
    }
}



