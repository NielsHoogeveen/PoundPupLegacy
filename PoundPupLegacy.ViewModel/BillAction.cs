
namespace PoundPupLegacy.ViewModel;

public record BillAction
{
    public required Link BillActionType { get; init; }

    public required Link Bill { get; init; }

    public required DateTime Date { get; init; }
}
