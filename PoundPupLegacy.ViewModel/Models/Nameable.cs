namespace PoundPupLegacy.ViewModel.Models;

public interface Nameable : Node
{
    string Description { get; }

    BasicLink[] SubTopics { get; }
    BasicLink[] SuperTopics { get; }

}
