namespace PoundPupLegacy.ViewModel.Models;

public interface Nameable : Node
{
    string Description { get; }

    Link[] SubTopics { get; }
    Link[] SuperTopics { get; }

}
