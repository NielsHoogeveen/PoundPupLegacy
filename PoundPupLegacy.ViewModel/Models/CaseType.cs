using PoundPupLegacy.Common;

namespace PoundPupLegacy.ViewModel.Models;
public enum CaseType
{
    Any = 0,
    Abuse = Constants.ABUSE_CASE,
    ChildTrafficking = Constants.CHILD_TRAFFICKING_CASE,
    CoercedAdoption = Constants.COERCED_ADOPTION_CASE,
    Deportation = Constants.DEPORTATION_CASE,
    FathersRightsViolation = Constants.FATHERS_RIGHTS_VIOLATION_CASE,
    WrongfulMedication = Constants.WRONGFUL_MEDICATION_CASE,
    WrongfulRemoval = Constants.WRONGFUL_REMOVAL_CASE,
    DisruptedPlacement = Constants.DISRUPTED_PLACEMENT_CASE,

}
