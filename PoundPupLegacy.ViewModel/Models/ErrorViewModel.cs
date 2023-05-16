namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(ErrorViewModel))]
public partial class ErrorViewModelJsonContext : JsonSerializerContext { }

public sealed record ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}