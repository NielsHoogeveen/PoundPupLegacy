namespace PoundPupLegacy.CreateModel;

public sealed record NewCoercedAdoptionCase : NewCaseBase, EventuallyIdentifiableCoercedAdoptionCase
{
}
public sealed record ExistingCoercedAdoptionCase : ExistingCaseBase, ImmediatelyIdentifiableCoercedAdoptionCase
{
}
public interface ImmediatelyIdentifiableCoercedAdoptionCase : CoercedAdoptionCase, ImmediatelyIdentifiableCase
{
}
public interface EventuallyIdentifiableCoercedAdoptionCase : CoercedAdoptionCase, EventuallyIdentifiableCase
{
}

public interface CoercedAdoptionCase: Case
{
}
