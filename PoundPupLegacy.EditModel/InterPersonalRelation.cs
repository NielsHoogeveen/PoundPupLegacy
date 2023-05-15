namespace PoundPupLegacy.EditModel;

public interface InterPersonalRelation : Relation
{
    InterPersonalRelationTypeListItem InterPersonalRelationType { get; set; }
    string PersonFromName { get; }
    string PersonToName { get; }
    PersonItem? PersonItemFrom { get; }
    PersonItem? PersonItemTo { get; }
    PersonItem.PersonListItem? PersonListItemFrom { get; set; }
    PersonItem.PersonListItem? PersonListItemTo { get; set; }
    InterPersonalRelation SwapFromAndTo();
    RelationSide RelationSideThisPerson { get; }
}

public interface CompletedInterPersonalRelation : InterPersonalRelation
{
}

public interface NewInterPersonalRelation : InterPersonalRelation, NewNode
{
}
public interface IncompleteNewInterPersonalRelation : NewInterPersonalRelation
{
    public CompletedInterPersonalRelation GetCompletedRelation(PersonItem.PersonListItem personListItem);
}

public interface CompletedNewInterPersonalRelation : NewInterPersonalRelation, CompletedInterPersonalRelation
{
}
public interface ResolvedInterPersonalRelation : CompletedInterPersonalRelation
{
}

public abstract record InterPersonalRelationBase : RelationBase, InterPersonalRelation
{
    public required InterPersonalRelationTypeListItem InterPersonalRelationType { get; set; }
    public abstract string PersonFromName { get; }
    public abstract string PersonToName { get; }
    public abstract PersonItem? PersonItemFrom { get; }
    public abstract PersonItem? PersonItemTo { get; }

    public abstract InterPersonalRelation SwapFromAndTo();
    public abstract RelationSide RelationSideThisPerson { get; }
    public PersonItem.PersonListItem? PersonListItemFrom { get; set; }
    public PersonItem.PersonListItem? PersonListItemTo { get; set; }

}

[JsonSerializable(typeof(ExistingInterPersonalRelation))]
public partial class ExistingInterPersonalRelationJsonContext : JsonSerializerContext { }

public record ExistingInterPersonalRelation : InterPersonalRelationBase, ExistingNode, ResolvedInterPersonalRelation
{
    public required PersonItem.PersonListItem PersonFrom { get; set; }
    public required PersonItem.PersonListItem PersonTo { get; set; }
    public int NodeId { get; init; }
    public int UrlId { get; set; }
    [JsonIgnore]
    public override string PersonFromName => PersonFrom.Name;
    [JsonIgnore]
    public override string PersonToName => PersonTo.Name;
    [JsonIgnore]
    public override PersonItem? PersonItemFrom => PersonFrom;
    [JsonIgnore]
    public override PersonItem? PersonItemTo => PersonTo;
    public override ExistingInterPersonalRelation SwapFromAndTo()
    {
        (PersonTo, PersonFrom) = (PersonFrom, PersonTo);
        (PersonListItemTo, PersonListItemFrom) = (PersonListItemFrom, PersonListItemTo);

        if (SettableRelationSideThisPerson == RelationSide.To) {
            SettableRelationSideThisPerson = RelationSide.From;
        }
        else {
            SettableRelationSideThisPerson = RelationSide.To;
        }
        return this;
    }

    public override RelationSide RelationSideThisPerson => SettableRelationSideThisPerson;

    public required RelationSide SettableRelationSideThisPerson { get; set; }
}

public record NewInterPersonalExistingRelation : InterPersonalRelationBase, ResolvedInterPersonalRelation
{
    public required PersonItem.PersonListItem PersonFrom { get; set; }
    public required PersonItem.PersonListItem PersonTo { get; set; }
    public override string PersonFromName => PersonFrom.Name;
    public override string PersonToName => PersonTo.Name;
    public override PersonItem? PersonItemFrom => PersonFrom;
    public override PersonItem? PersonItemTo => PersonTo;

    public override NewInterPersonalExistingRelation SwapFromAndTo()
    {
        (PersonTo, PersonFrom) = (PersonFrom, PersonTo);
        (PersonListItemTo, PersonListItemFrom) = (PersonListItemFrom, PersonListItemTo);
        if (SettableRelationSideThisPerson == RelationSide.To) {
            SettableRelationSideThisPerson = RelationSide.From;
        }
        else {
            SettableRelationSideThisPerson = RelationSide.To;
        }
        return this;
    }
    public override RelationSide RelationSideThisPerson => SettableRelationSideThisPerson;

    public required RelationSide SettableRelationSideThisPerson { get; set; }
}

public record NewInterPersonalExistingFromRelation : InterPersonalRelationBase, InterPersonalRelation, IncompleteNewInterPersonalRelation
{
    public required PersonItem.PersonListItem PersonFrom { get; set; }
    public required PersonItem.PersonListItem? PersonTo { get; set; }
    public override string PersonFromName => PersonFrom.Name;
    public override string PersonToName => PersonTo is null ? "" : PersonTo.Name;
    public override PersonItem? PersonItemFrom => PersonFrom;
    public override PersonItem? PersonItemTo => PersonTo;
    public override NewInterPersonalExistingToRelation SwapFromAndTo()
    {
        return new NewInterPersonalExistingToRelation {
            PersonFrom = PersonTo,
            PersonTo = PersonFrom,
            DateFrom = DateFrom,
            DateTo = DateTo,
            Description = Description,
            InterPersonalRelationType = InterPersonalRelationType,
            ProofDocument = ProofDocument,
            NodeTypeName = NodeTypeName,
            Files = Files,
            HasBeenDeleted = HasBeenDeleted,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            Tags = Tags,
            TenantNodes = TenantNodes,
            Tenants = Tenants,
            Title = Title,
            PersonListItemFrom = PersonListItemTo,
            PersonListItemTo = PersonListItemFrom,
        };
    }
    public CompletedInterPersonalRelation GetCompletedRelation(PersonItem.PersonListItem personTo)
    {
        return new NewInterPersonalExistingRelation {
            PersonFrom = PersonFrom,
            PersonTo = personTo,
            InterPersonalRelationType = InterPersonalRelationType,
            DateFrom = DateFrom,
            DateTo = DateTo,
            Description = Description,
            ProofDocument = ProofDocument,
            PersonListItemFrom = PersonListItemFrom,
            PersonListItemTo = PersonListItemTo,
            HasBeenDeleted = HasBeenDeleted,
            Files = Files,
            Tags = Tags,
            TenantNodes = TenantNodes,
            Tenants = Tenants,
            NodeTypeName = NodeTypeName,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            Title = Title,
            SettableRelationSideThisPerson = RelationSide.From
        };
    }

    public override RelationSide RelationSideThisPerson => RelationSide.From;

}
public record NewInterPersonalExistingToRelation : InterPersonalRelationBase, InterPersonalRelation, IncompleteNewInterPersonalRelation
{
    public required PersonItem.PersonListItem? PersonFrom { get; set; }
    public required PersonItem.PersonListItem PersonTo { get; set; }
    public override string PersonFromName => PersonFrom is null ? "" : PersonFrom.Name;
    public override string PersonToName => PersonTo.Name;
    public override PersonItem? PersonItemFrom => PersonFrom;
    public override PersonItem? PersonItemTo => PersonTo;
    public override NewInterPersonalExistingFromRelation SwapFromAndTo()
    {
        return new NewInterPersonalExistingFromRelation {
            PersonFrom = PersonTo,
            PersonTo = PersonFrom,
            DateFrom = DateFrom,
            DateTo = DateTo,
            Description = Description,
            InterPersonalRelationType = InterPersonalRelationType,
            ProofDocument = ProofDocument,
            NodeTypeName = NodeTypeName,
            Files = Files,
            HasBeenDeleted = HasBeenDeleted,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            Tags = Tags,
            TenantNodes = TenantNodes,
            Tenants = Tenants,
            Title = Title,
            PersonListItemFrom = PersonListItemTo,
            PersonListItemTo = PersonListItemFrom,
        };
    }
    public CompletedInterPersonalRelation GetCompletedRelation(PersonItem.PersonListItem personFrom)
    {
        return new NewInterPersonalExistingRelation {
            PersonFrom = personFrom,
            PersonTo = PersonTo,
            InterPersonalRelationType = InterPersonalRelationType,
            DateFrom = DateFrom,
            DateTo = DateTo,
            Description = Description,
            ProofDocument = ProofDocument,
            PersonListItemFrom = PersonListItemFrom,
            PersonListItemTo = PersonListItemTo,
            HasBeenDeleted = HasBeenDeleted,
            Files = Files,
            Tags = Tags,
            TenantNodes = TenantNodes,
            Tenants = Tenants,
            NodeTypeName = NodeTypeName,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            Title = Title,
            SettableRelationSideThisPerson = RelationSide.To
        };
    }
    public override RelationSide RelationSideThisPerson => RelationSide.To;
}

public record CompletedNewInterPersonalNewFromRelation : InterPersonalRelationBase, CompletedInterPersonalRelation, CompletedNewInterPersonalRelation
{
    public required PersonItem.PersonName PersonFrom { get; set; }
    public required PersonItem.PersonListItem PersonTo { get; set; }
    public override string PersonFromName => PersonFrom.Name;
    public override string PersonToName => PersonTo.Name;
    public override PersonItem? PersonItemFrom => PersonFrom;
    public override PersonItem? PersonItemTo => PersonTo;

    public override CompletedNewInterPersonalNewToRelation SwapFromAndTo()
    {
        return new CompletedNewInterPersonalNewToRelation {
            PersonFrom = PersonTo,
            PersonTo = PersonFrom,
            DateFrom = DateFrom,
            DateTo = DateTo,
            Description = Description,
            InterPersonalRelationType = InterPersonalRelationType,
            ProofDocument = ProofDocument,
            NodeTypeName = NodeTypeName,
            Files = Files,
            HasBeenDeleted = HasBeenDeleted,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            Tags = Tags,
            TenantNodes = TenantNodes,
            Tenants = Tenants,
            Title = Title,
            PersonListItemFrom = PersonListItemTo,
            PersonListItemTo = PersonListItemFrom,
        };
    }
    public override RelationSide RelationSideThisPerson => RelationSide.From;
}
public record NewInterPersonalNewFromRelation : InterPersonalRelationBase, InterPersonalRelation, IncompleteNewInterPersonalRelation
{
    public required PersonItem.PersonName PersonFrom { get; set; }
    public required PersonItem.PersonListItem? PersonTo { get; set; }
    public override string PersonFromName => PersonFrom.Name;
    public override string PersonToName => PersonTo is null ? "" : PersonTo.Name;
    public override PersonItem? PersonItemFrom => PersonFrom;
    public override PersonItem? PersonItemTo => PersonTo;
    public override NewInterPersonalNewToRelation SwapFromAndTo()
    {
        return new NewInterPersonalNewToRelation {
            PersonFrom = PersonTo,
            PersonTo = PersonFrom,
            DateFrom = DateFrom,
            DateTo = DateTo,
            Description = Description,
            InterPersonalRelationType = InterPersonalRelationType,
            ProofDocument = ProofDocument,
            NodeTypeName = NodeTypeName,
            Files = Files,
            HasBeenDeleted = HasBeenDeleted,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            Tags = Tags,
            TenantNodes = TenantNodes,
            Tenants = Tenants,
            Title = Title,
            PersonListItemFrom = PersonListItemTo,
            PersonListItemTo = PersonListItemFrom,
        };
    }
    public CompletedInterPersonalRelation GetCompletedRelation(PersonItem.PersonListItem personTo)
    {
        return new CompletedNewInterPersonalNewFromRelation {
            PersonFrom = PersonFrom,
            PersonTo = personTo,
            InterPersonalRelationType = InterPersonalRelationType,
            DateFrom = DateFrom,
            DateTo = DateTo,
            Description = Description,
            ProofDocument = ProofDocument,
            PersonListItemFrom = PersonListItemFrom,
            PersonListItemTo = PersonListItemTo,
            HasBeenDeleted = HasBeenDeleted,
            Files = Files,
            Tags = Tags,
            TenantNodes = TenantNodes,
            Tenants = Tenants,
            NodeTypeName = NodeTypeName,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            Title = Title,
        };
    }

    public override RelationSide RelationSideThisPerson => RelationSide.From;
}
public record CompletedNewInterPersonalNewToRelation : InterPersonalRelationBase, CompletedInterPersonalRelation, CompletedNewInterPersonalRelation
{
    public required PersonItem.PersonListItem PersonFrom { get; set; }
    public required PersonItem.PersonName PersonTo { get; set; }
    public override string PersonFromName => PersonFrom.Name;
    public override string PersonToName => PersonTo.Name;
    public override PersonItem? PersonItemFrom => PersonFrom;
    public override PersonItem? PersonItemTo => PersonTo;
    public override CompletedNewInterPersonalNewFromRelation SwapFromAndTo()
    {
        return new CompletedNewInterPersonalNewFromRelation {
            PersonFrom = PersonTo,
            PersonTo = PersonFrom,
            DateFrom = DateFrom,
            DateTo = DateTo,
            Description = Description,
            InterPersonalRelationType = InterPersonalRelationType,
            ProofDocument = ProofDocument,
            NodeTypeName = NodeTypeName,
            Files = Files,
            HasBeenDeleted = HasBeenDeleted,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            Tags = Tags,
            TenantNodes = TenantNodes,
            Tenants = Tenants,
            Title = Title,
            PersonListItemFrom = PersonListItemTo,
            PersonListItemTo = PersonListItemFrom,
        };
    }

    public override RelationSide RelationSideThisPerson => RelationSide.To;
}
public record NewInterPersonalNewToRelation : InterPersonalRelationBase, InterPersonalRelation, IncompleteNewInterPersonalRelation
{
    public required PersonItem.PersonListItem? PersonFrom { get; set; }
    public required PersonItem.PersonName PersonTo { get; set; }
    public override string PersonFromName => PersonFrom is null ? "" : PersonFrom.Name;
    public override string PersonToName => PersonTo.Name;
    public override PersonItem? PersonItemFrom => PersonFrom;
    public override PersonItem? PersonItemTo => PersonTo;
    public override NewInterPersonalNewFromRelation SwapFromAndTo()
    {
        return new NewInterPersonalNewFromRelation {
            PersonFrom = PersonTo,
            PersonTo = PersonFrom,
            DateFrom = DateFrom,
            DateTo = DateTo,
            Description = Description,
            InterPersonalRelationType = InterPersonalRelationType,
            ProofDocument = ProofDocument,
            NodeTypeName = NodeTypeName,
            Files = Files,
            HasBeenDeleted = HasBeenDeleted,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            Tags = Tags,
            TenantNodes = TenantNodes,
            Tenants = Tenants,
            Title = Title,
            PersonListItemFrom = PersonListItemTo,
            PersonListItemTo = PersonListItemFrom,
        };
    }

    public CompletedInterPersonalRelation GetCompletedRelation(PersonItem.PersonListItem personFrom)
    {
        return new CompletedNewInterPersonalNewToRelation {
            PersonFrom = personFrom,
            PersonTo = PersonTo,
            InterPersonalRelationType = InterPersonalRelationType,
            DateFrom = DateFrom,
            DateTo = DateTo,
            Description = Description,
            ProofDocument = ProofDocument,
            PersonListItemFrom = PersonListItemFrom,
            PersonListItemTo = PersonListItemTo,
            HasBeenDeleted = HasBeenDeleted,
            Files = Files,
            Tags = Tags,
            TenantNodes = TenantNodes,
            Tenants = Tenants,
            NodeTypeName = NodeTypeName,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            Title = Title,
        };
    }

    public override RelationSide RelationSideThisPerson => RelationSide.To;
}
