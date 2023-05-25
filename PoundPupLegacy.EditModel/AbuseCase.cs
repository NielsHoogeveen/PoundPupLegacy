namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingAbuseCase))]
public partial class ExistingAbuseCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewAbuseCase))]
public partial class NewAbuseCaseJsonContext : JsonSerializerContext { }

public interface AbuseCase : Case, ResolvedNode
{
    int ChildPlacementTypeId { get; set; }
    int? FamilySizeId { get; set; }
    bool? HomeschoolingInvolved { get; set; }
    bool? FundamentalFaithInvolved { get; set; }
    bool? DisabilitiesInvolved { get; set; }
}

public sealed record NewAbuseCase : AbuseCaseBase, ResolvedNewNode
{
}
public sealed record ExistingAbuseCase : AbuseCaseBase, ExistingNode
{
    public int NodeId { get; set; }

    public int UrlId { get; set; }

}
public abstract record AbuseCaseBase : CaseBase, AbuseCase
{

    public required int ChildPlacementTypeId { get; set; }
    public int? FamilySizeId { get; set; }
    public bool? HomeschoolingInvolved { get; set; }
    public bool? FundamentalFaithInvolved { get; set; }
    public bool? DisabilitiesInvolved { get; set; }

    private List<int> typeOfAbuseIds = new List<int>();
    public List<int> TypeOfAbuseIds { 
        get => typeOfAbuseIds; 
        set => typeOfAbuseIds = value ?? new List<int>(); 
    }
    private List<int> typeOfAbuserIds = new List<int>();
    public List<int> TypeOfAbuserIds {
        get => typeOfAbuserIds;
        set => typeOfAbuserIds = value ?? new List<int>();
    }
    public required ChildPlacementType[] ChildPlacementTypes { get; init; }
    public required FamilySize[] FamilySizes { get; init; }
    public required TypeOfAbuse[] TypesOfAbuse { get; init; }
    public required TypeOfAbuser[] TypesOfAbuser { get; init; }


}
