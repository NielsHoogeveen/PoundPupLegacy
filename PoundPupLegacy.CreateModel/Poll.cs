namespace PoundPupLegacy.CreateModel;
public interface ImmediatelyIdentifiablePoll : Poll, ImmediatelyIdentifiableNode
{
}

public interface EventuallyIdentifiablePoll : Poll, EventuallyIdentifiableNode
{
}

public interface Poll : Node
{

    int PollStatusId { get; }
    DateTime DateTimeClosure { get; }

}
