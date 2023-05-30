namespace PoundPupLegacy.CreateModel;

public interface UserRole : Principal
{
    string Name { get; }
    int? UserGroupId { get; }
}
