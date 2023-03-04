namespace PoundPupLegacy.Services;
public interface ICongressionalDataService
{

    Task<string?> GetCongressionalMeetingChamberResult();
    Task<string?> GetUnitedStatesCongress();
}
