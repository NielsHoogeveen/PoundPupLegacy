using System.Text.Json.Serialization;

namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(BillAction))]
public partial class BillActionJsonContext : JsonSerializerContext { }

public record BillAction
{
    public required BasicLink BillActionType { get; init; }

    public required BasicLink Bill { get; init; }

    public required DateTime Date { get; init; }
}
