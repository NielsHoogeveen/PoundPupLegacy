namespace PoundPupLegacy.CreateModel;

public interface Poll : Node
{

    int PollStatusId { get; }
    DateTime DateTimeClosure { get; }

}
