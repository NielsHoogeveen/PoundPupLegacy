namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(DeportationCase))]
public partial class DeportationCaseJsonContext : JsonSerializerContext { }

public sealed record DeportationCase : CaseBase
{
    public required BasicLink? CountryTo { get; init; }
    public required BasicLink? SubdivisionFrom { get; init; }
    }
