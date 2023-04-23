namespace PoundPupLegacy.ViewModel.UI.Services;

public enum ChamberType
{
    House = Constants.UNITED_STATES_HOUSE_OF_REPRESENTATIVES,
    Senate = Constants.UNITED_STATES_SENATE
}

public interface ICongressionalDataService
{
    Task<CongressionalMeetingChamber?> GetCongressionalMeetingChamber(ChamberType chamberType, int number);
    Task<CongressionalMeetingChamber?> GetCongressionalMeetingChamber(string path);
    Task<UnitedStatesCongress?> GetUnitedStatesCongress();
}
