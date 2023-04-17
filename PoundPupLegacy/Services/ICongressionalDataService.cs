using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;
public interface ICongressionalDataService
{
    Task<string?> GetCongressionalMeetingChamberResult(HttpContext context);
    Task<string?> GetUnitedStatesCongress(HttpContext context);
    Task<CongressionalMeetingChamber?> GetCongressionalMeetingChamber(string path);
    Task<UnitedStatesCongress?> GetUnitedStatesCongress();
}
