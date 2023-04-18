using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;
public interface ICongressionalDataService
{
    Task<CongressionalMeetingChamber?> GetCongressionalMeetingChamber(string path);
    Task<UnitedStatesCongress?> GetUnitedStatesCongress();
}
