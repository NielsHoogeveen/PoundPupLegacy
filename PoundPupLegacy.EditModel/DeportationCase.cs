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

public sealed record ExistingDeportationCase : ExistingCaseBase, DeportationCase
{
    public SubdivisionListItem? SubdivisionFrom { get; set; }
    public CountryListItem? CountryTo { get; set; }
}

public sealed record NewDeportationCase : NewCaseBase, ResolvedNewNode, DeportationCase
{
    public SubdivisionListItem? SubdivisionFrom { get; set; }
    public CountryListItem? CountryTo { get; set; }

}
