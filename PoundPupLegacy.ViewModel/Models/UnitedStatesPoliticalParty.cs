namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(UnitedStatesPoliticalPartyAffiliation))]
public partial class UnitedStatesPoliticalPartyAffiliationJsonContext : JsonSerializerContext { }

public sealed record UnitedStatesPoliticalPartyAffiliation : NameableBase
{

}

