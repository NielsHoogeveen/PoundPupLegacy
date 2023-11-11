namespace PoundPupLegacy.ViewModel.UI.Services;

public enum ChamberType
{
    House = Constants.UNITED_STATES_HOUSE_OF_REPRESENTATIVES,
    Senate = Constants.UNITED_STATES_SENATE
}

public interface ICongressionalDataService
{
    Task<CongressionalMeetingChamber?> GetSenateMeeting(int number, int tenantId);

    Task<CongressionalMeetingChamber?> GetHouseMeeting(int number, int tenantId);
    Task<CongressionalMeetingChamber?> GetCongressionalMeetingChamber(ChamberType chamberType, int number, int tenantId);
    Task<CongressionalMeetingChamber?> GetCongressionalMeetingChamber(string path, int tenantId);
    Task<UnitedStatesCongress?> GetUnitedStatesCongress(int tenantId, int userId);
}
