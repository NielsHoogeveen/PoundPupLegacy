namespace PoundPupLegacy.Model;

public record AnonymousUser : AccessRole
{
    public int Id { get; }
    public string Name { get; }
    public AnonymousUser()
    {
        Id = 0;
        Name = "anonymous";
    }
}
