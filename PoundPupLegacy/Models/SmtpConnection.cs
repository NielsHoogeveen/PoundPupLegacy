namespace PoundPupLegacy.Models;

public record SmtpConnection
{
    public required int Id { get; init; }
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
}
