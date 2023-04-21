namespace PoundPupLegacy.ViewModel.Models;

public record BillAction
{
    public required BasicLink BillActionType { get; init; }

    public required BasicLink Bill { get; init; }

    public required DateTime Date { get; init; }
}
