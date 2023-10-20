using System.ComponentModel.DataAnnotations;

namespace PoundPupLegacy.Models;

public record UserRegistrationData
{

    [Required]
    public string? UserName { get; set; }
    [Required]
    public string? RegistrationReason { get; set; }
}

public record CompletedUserRegistrationData
{
    public required string NameIdentifier { get; init; }
    public required string UserName { get; init; }
    public required string RegistrationReason { get; init; }

}
