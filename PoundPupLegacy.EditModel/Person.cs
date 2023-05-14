namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingPerson))]
public partial class ExistingPersonJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewPerson))]
public partial class NewPersonJsonContext : JsonSerializerContext { }

public interface Person : Party
{
    List<InterPersonalRelation> InterPersonalRelations { get; }
    List<InterPersonalRelationTypeListItem> InterPersonalRelationTypes { get; }
}
public record NewPerson : PersonBase, NewNode 
{ 
}
public record ExistingPerson : PersonBase, ExistingNode
{
    public required int NodeId { get; init; }
    public required int UrlId { get; set; }
}
public record PersonBase : PartyBase, Person
{
    private List<InterPersonalRelation> interPersonalRelations = new();

    public List<InterPersonalRelation> InterPersonalRelations {
        get => interPersonalRelations;
        init {
            if (value is not null) {
                interPersonalRelations = value;
            }
        }
    }

    private List<InterPersonalRelationTypeListItem> interPersonalRelationTypes = new();

    public List<InterPersonalRelationTypeListItem> InterPersonalRelationTypes {
        get => interPersonalRelationTypes;
        init {
            if (value is not null) {
                interPersonalRelationTypes = value;
            }
        }
    }
}
