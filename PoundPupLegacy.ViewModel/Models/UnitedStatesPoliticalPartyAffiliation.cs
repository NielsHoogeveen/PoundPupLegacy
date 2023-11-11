namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(UnitedStatesPoliticalParty))]
public partial class UnitedStatesPoliticalPartyJsonContext : JsonSerializerContext { }

public sealed record UnitedStatesPoliticalParty : Organization
{

}

