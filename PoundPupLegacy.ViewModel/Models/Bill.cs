namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(HouseBill))]
[JsonSerializable(typeof(SenateBill))]
public partial class BillJsonContext : JsonSerializerContext { }

public abstract record Bill : NameableBase
{
    public required DateTime IntroductionDate { get; init; }

    public required BasicLink? Act { get; init; }
}

public sealed record SenateBill : Bill
{
    private List<SenateBillAction> _billActions = new();
    public required List<SenateBillAction> BillActions { 
        get => _billActions;
        init {
            if (value is not null) {
                _billActions = value;
            }
        }
    }
}
public sealed record HouseBill : Bill
{
    private List<HouseBillAction> _billActions = new();
    public required List<HouseBillAction> BillActions {
        get => _billActions;
        init {
            if (value is not null) {
                _billActions = value;
            }
        }
    }

}
public sealed record HouseBillAction
{
    public required BasicLink Representative { get; init; }

    public required BasicLink BillActionType { get; init; }

    public required DateTime Date { get; init; }

}

public sealed record SenateBillAction
{
    public required BasicLink Senator { get; init; }

    public required BasicLink BillActionType { get; init; }

    public required DateTime Date { get; init; }

}
