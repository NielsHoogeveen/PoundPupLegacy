namespace PoundPupLegacy.Services;
public interface ICongressionalDataService
{

    Task<string?> GetCongressionalMeetingChamberResult(HttpContext context);
    Task<string?> GetUnitedStatesCongress(HttpContext context);
}
