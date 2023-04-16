namespace PoundPupLegacy.ViewModel.Models;

public record BillAction
{
    public required Link BillActionType { get; init; }

    public required Link Bill { get; init; }

    public required DateTime Date { get; init; }
}
