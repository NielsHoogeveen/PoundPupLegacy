namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(BillActionType))]
public partial class BillActionTypeJsonContext : JsonSerializerContext { }

public sealed record BillActionType : NameableBase
{
}
