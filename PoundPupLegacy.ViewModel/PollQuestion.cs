namespace PoundPupLegacy.ViewModel;

public interface PollQuestion : SimpleTextNode
{
    PollOption[] PollOptions { get; }
}
