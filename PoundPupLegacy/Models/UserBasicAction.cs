namespace PoundPupLegacy.Models;

public interface IUserAction
{
       string Path { get; }
}
public record UserBasicAction: IUserAction
{
    public required string Path { get; init; }
}
