namespace PoundPupLegacy.ViewModel;

public interface Nameable : Node
{
    string Description { get; }

    Link[] SubTopics { get; }
    Link[] SuperTopics { get; }

}
