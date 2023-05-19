namespace PoundPupLegacy.ViewModel.Models;

public abstract record PollBase: SimpleTextNodeBase, Poll
{
    public required int PollStatusId { get; init; }
    public DateTime? DateTimeClosure { get; init; }
}
public interface Poll : Node
{
    int PollStatusId { get;  }
    DateTime? DateTimeClosure { get; }

}
