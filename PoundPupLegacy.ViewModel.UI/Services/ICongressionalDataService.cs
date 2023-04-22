namespace PoundPupLegacy.ViewModel.UI.Services;
public interface ICongressionalDataService
{
    Task<CongressionalMeetingChamber?> GetCongressionalMeetingChamber(string path);
    Task<UnitedStatesCongress?> GetUnitedStatesCongress();
}
