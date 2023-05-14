﻿namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingChildTraffickingCase))]
public partial class ExistingChildTraffickingCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewChildTraffickingCase))]
public partial class NewChildTraffickingCaseJsonContext : JsonSerializerContext { }

public interface ChildTraffickingCase: Case
{
    int? NumberOfChildrenInvolved { get; set; }
    int CountryIdFrom { get; set; }

}
public record ExistingChildTraffickingCase : ChildTraffickingCaseBase, ExistingNode
{
    public int NodeId { get; set; }

    public int UrlId { get; set; }
}
public record NewChildTraffickingCase : ChildTraffickingCaseBase, NewNode
{
}
 public record ChildTraffickingCaseBase : CaseBase, ChildTraffickingCase
{
    public int? NumberOfChildrenInvolved { get; set; }
    public required int CountryIdFrom { get; set; }

}
