namespace PoundPupLegacy.CreateModel;

public sealed record NewCoercedAdoptionCase : NewCaseBase, EventuallyIdentifiableCase
{
}
public sealed record ExistingCoercedAdoptionCase : ExistingCaseBase, ImmediatelyIdentifiableCase
{
}
