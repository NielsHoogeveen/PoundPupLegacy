namespace PoundPupLegacy.CreateModel;

public sealed record NewChildTraffickingCase : NewCaseBase, EventuallyIdentifiableCase
{
    public required int? NumberOfChildrenInvolved { get; init; }
    public required int CountryIdFrom { get; init; }
}
public sealed record ExistingChildTraffickingCase : ExistingCaseBase, ImmediatelyIdentifiableCase
{
    public required int? NumberOfChildrenInvolved { get; init; }
    public required int CountryIdFrom { get; init; }
}
