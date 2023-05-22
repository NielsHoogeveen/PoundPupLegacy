namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingChildTraffickingCase))]
public partial class ExistingChildTraffickingCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewChildTraffickingCase))]
public partial class NewChildTraffickingCaseJsonContext : JsonSerializerContext { }

public interface ChildTraffickingCase : Case
{
    int? NumberOfChildrenInvolved { get; }
    CountryListItem? CountryFrom { get; }

}
public interface ResolvedChildTraffickingCase : ChildTraffickingCase
{
    new CountryListItem CountryFrom { get; set; }
}
public sealed record ExistingChildTraffickingCase : ChildTraffickingCaseBase, ResolvedChildTraffickingCase, ExistingNode
{
    public int NodeId { get; set; }

    public int UrlId { get; set; }
    
    public new required CountryListItem CountryFrom { get; set; }
}
public sealed record ResolvedNewChildTraffickingCase : ChildTraffickingCaseBase, ResolvedChildTraffickingCase, NewNode
{
    public new required CountryListItem CountryFrom { get; set; }
}
public sealed record NewChildTraffickingCase : ChildTraffickingCaseBase, NewNode
{
    ResolvedNewChildTraffickingCase Resolve(CountryListItem CountryFrom)
    {
        return new ResolvedNewChildTraffickingCase {
            CountryFrom = CountryFrom,
            Date = Date,
            CasePartyTypesCaseParties = CasePartyTypesCaseParties,
            Description = Description,
            Files = Files,
            NodeTypeName = NodeTypeName,
            NumberOfChildrenInvolved = NumberOfChildrenInvolved,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            Tags = Tags,
            TenantNodes = TenantNodes,
            Tenants = Tenants,
            Terms = Terms,
            Title = Title
        };
    }

}
public abstract record ChildTraffickingCaseBase : CaseBase, ChildTraffickingCase
{
    public int? NumberOfChildrenInvolved { get; set; }

    public CountryListItem? CountryFrom { get; set; }
}
