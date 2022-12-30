namespace PoundPupLegacy.ViewModel;

public record User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public string? AboutMe { get; set; }
    public string? AnimalWithin { get; set; }
    public string RelationToChildPlacement { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? Avatar { get; set; }
}