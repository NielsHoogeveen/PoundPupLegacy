namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingDeportationCase))]
public partial class ExistingDeportationCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewDeportationCase))]
public partial class NewDeportationCaseJsonContext : JsonSerializerContext { }

public interface DeportationCase : Case, ResolvedNode
{
    SubdivisionListItem? SubdivisionFrom { get;  }
    CountryListItem? CountryTo { get;  }

}

public sealed record ExistingDeportationCase : DeportationCaseBase, ExistingNode
{
    public int NodeId { get; set; }

    public int UrlId { get; set; }
}

public sealed record NewDeportationCase : DeportationCaseBase, ResolvedNewNode
{
}
public abstract record DeportationCaseBase : CaseBase, DeportationCase
{
    public SubdivisionListItem? SubdivisionFrom { get; set; }
    public CountryListItem? CountryTo { get; set; }

}



