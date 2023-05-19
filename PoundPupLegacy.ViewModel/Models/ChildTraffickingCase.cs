namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(ChildTraffickingCase))]
public partial class ChildTraffickingCaseJsonContext : JsonSerializerContext { }

public sealed record ChildTraffickingCase : CaseBase
{
    public required BasicLink? CountryFrom { get; init; }
    public required int? NumberOfChildrenInvolved { get; init; }
}
