namespace PoundPupLegacy.CreateModel;

public sealed record NewChildTraffickingCase : NewCaseBase, EventuallyIdentifiableChildTraffickingCase
{
    public required int? NumberOfChildrenInvolved { get; init; }
    public required int CountryIdFrom { get; init; }
}
public sealed record ExistingChildTraffickingCase : ExistingCaseBase, ImmediatelyIdentifiableChildTraffickingCase
{
    public required int? NumberOfChildrenInvolved { get; init; }
    public required int CountryIdFrom { get; init; }
}
public interface ImmediatelyIdentifiableChildTraffickingCase : ChildTraffickingCase, ImmediatelyIdentifiableCase
{
}
public interface EventuallyIdentifiableChildTraffickingCase : ChildTraffickingCase, EventuallyIdentifiableCase
{
}
public interface ChildTraffickingCase: Case
{
    int? NumberOfChildrenInvolved { get; }
    int CountryIdFrom { get;  }
}