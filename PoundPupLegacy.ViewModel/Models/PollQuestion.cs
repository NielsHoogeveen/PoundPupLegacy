namespace PoundPupLegacy.ViewModel.Models;

public record PollQuestionBase: SimpleTextNodeBase, PollQuestion
{
    private PollOption[] pollOptions = Array.Empty<PollOption>();
    public PollOption[] PollOptions {
        get => pollOptions;
        init {
            if (value is not null) {
                pollOptions = value;
            }

        }
    }
}

public interface PollQuestion : SimpleTextNode
{
    PollOption[] PollOptions { get; }
}
