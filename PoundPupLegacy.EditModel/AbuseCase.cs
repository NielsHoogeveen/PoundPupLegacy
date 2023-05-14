namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingAbuseCase))]
public partial class ExistingAbuseCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewAbuseCase))]
public partial class NewAbuseCaseJsonContext : JsonSerializerContext { }

public interface AbuseCase: Case
{
    int ChildPlacementTypeId { get; set; }
    int? FamilySizeId { get; set; }
    bool? HomeschoolingInvolved { get; set; }
    bool? FundamentalFaithInvolved { get; set; }
    bool? DisabilitiesInvolved { get; set; }
}

public record NewAbuseCase : AbuseCaseBase, NewNode
{
}
public record ExistingAbuseCase : AbuseCaseBase, ExistingNode
{
    public int NodeId { get; set; }

    public int UrlId { get; set; }

}
public record AbuseCaseBase : CaseBase, AbuseCase
{

    public required int ChildPlacementTypeId { get; set; }
    public int? FamilySizeId { get; set; }
    public bool? HomeschoolingInvolved { get; set; }
    public bool? FundamentalFaithInvolved { get; set; }
    public bool? DisabilitiesInvolved { get; set; }

}
