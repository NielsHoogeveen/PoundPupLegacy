namespace PoundPupLegacy.Model;

public interface UserRole : Principal
{
    string Name { get; }

    int? UserGroupId { get; }
}
