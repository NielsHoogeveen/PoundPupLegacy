namespace PoundPupLegacy.CreateModel;

public sealed record AnonymousUser : Publisher
{
    public int? Id { get; set; }
    public string Name { get; }
    public AnonymousUser()
    {
        Id = 0;
        Name = "anonymous";
    }
}
