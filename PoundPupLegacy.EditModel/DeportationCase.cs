namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingDeportationCase))]
public partial class ExistingDeportationCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewDeportationCase))]
public partial class NewDeportationCaseJsonContext : JsonSerializerContext { }

public interface DeportationCase : Case
{
    SubdivisionListItem? SubdivisionFrom { get; set; }
    CountryListItem? CountryTo { get; set; }

}
public sealed record ExistingDeportationCase : DeportationCaseBase, ExistingNode
{
    public int NodeId { get; set; }

    public int UrlId { get; set; }
}
public sealed record NewDeportationCase : DeportationCaseBase, NewNode
{
}
public abstract record DeportationCaseBase : CaseBase, DeportationCase
{
    public SubdivisionListItem? SubdivisionFrom { get; set; }
    public CountryListItem? CountryTo { get; set; }

}



