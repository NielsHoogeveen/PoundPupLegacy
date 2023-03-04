namespace PoundPupLegacy.Model;

public interface Poll : Node
{

    int PollStatusId { get; }
    DateTime DateTimeClosure { get; }

}
