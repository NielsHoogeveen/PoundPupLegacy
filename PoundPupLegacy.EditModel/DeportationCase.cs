namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingDeportationCase))]
public partial class ExistingDeportationCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewDeportationCase))]
public partial class NewDeportationCaseJsonContext : JsonSerializerContext { }

public interface DeportationCase : Case
{
    int? SubdivisionIdFrom { get; set; }
    int? CountryIdTo { get; set; }

}
public record ExistingDeportationCase : DeportationCaseBase, ExistingNode
{
    public int NodeId { get; set; }

    public int UrlId { get; set; }
}
public record NewDeportationCase : DeportationCaseBase, NewNode
{
}
public record DeportationCaseBase : CaseBase, DeportationCase
{
    public int? SubdivisionIdFrom { get; set; }
    public int? CountryIdTo { get; set; }

}



