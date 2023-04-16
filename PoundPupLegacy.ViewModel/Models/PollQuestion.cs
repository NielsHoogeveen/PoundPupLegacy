namespace PoundPupLegacy.ViewModel.Models;

public interface PollQuestion : SimpleTextNode
{
    PollOption[] PollOptions { get; }
}
