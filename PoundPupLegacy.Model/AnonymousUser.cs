namespace PoundPupLegacy.Model;

public sealed record AnonymousUser : AccessRole, Publisher
{
    public int? Id { get; set; }
    public string Name { get; }
    public AnonymousUser()
    {
        Id = 0;
        Name = "anonymous";
    }
}
