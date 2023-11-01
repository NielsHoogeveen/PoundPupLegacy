namespace PoundPupLegacy.Common;

public interface ITenant
{
    int Id { get; }
    string Name { get; }
    string Title { get; }
    string Description { get; }
    string DomainName { get; }
    int CountryIdDefault { get; }
    string CountryNameDefault { get; }
    string? FrontPageText { get; }
    string? Logo { get; }
    string? Subtitle { get; }
    string? FooterText { get; }
    string? CssFile { get; }
    int FrontPageId { get; }
    string? GoogleAnalyticsMeasurementId { get; }
    string? RegistrationText { get; }
    bool TrackActiveUsers { get; }
}
