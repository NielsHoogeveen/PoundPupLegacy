namespace PoundPupLegacy.ViewModel;

public interface Nameable : Node
{
    string Description { get; set; }

    Link[] SubTopics { get; set; }
    Link[] SuperTopics { get; set; }

}
