namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(ExecutiveCompensation))]
public partial class ExecutiveCompensationJsonContext : JsonSerializerContext { }

public record ExecutiveCompensation
{
}
