namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(ErrorViewModel))]
public partial class ErrorViewModelJsonContext : JsonSerializerContext { }

public record ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}