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
public interface ResolvedChildTraffickingCase : ChildTraffickingCase, ResolvedNode
{
    new CountryListItem CountryFrom { get; set; }
}
public sealed record ExistingChildTraffickingCase : ExistingCaseBase, ResolvedChildTraffickingCase, ExistingLocatable
{
    public required int? NumberOfChildrenInvolved { get; set; }

    public required CountryListItem CountryFrom { get; set; }
}
public sealed record ResolvedNewChildTraffickingCase : NewCaseBase, ResolvedChildTraffickingCase, ResolvedNewNode, NewLocatable
{
    public required int? NumberOfChildrenInvolved { get; set; }
    public required CountryListItem CountryFrom { get; set; }
}
public sealed record NewChildTraffickingCase : NewCaseBase, NewNode, ChildTraffickingCase
{
    public required int? NumberOfChildrenInvolved { get; set; }
    public required CountryListItem CountryFrom { get; set; }
    public ResolvedNewChildTraffickingCase Resolve(CountryListItem CountryFrom)
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
            Title = Title,
            VocabularyIdTagging = VocabularyIdTagging,
        };
    }
}
