namespace PoundPupLegacy.CreateModel;

public sealed record NewFathersRightsViolationCase : NewCaseBase, EventuallyIdentifiableFathersRightsViolationCase
{
}
public sealed record ExistingFathersRightsViolationCase : ExistingCaseBase, ImmediatelyIdentifiableFathersRightsViolationCase
{
}
public interface ImmediatelyIdentifiableFathersRightsViolationCase : FathersRightsViolationCase, ImmediatelyIdentifiableCase
{
}

public interface EventuallyIdentifiableFathersRightsViolationCase : FathersRightsViolationCase, EventuallyIdentifiableCase
{
}

public interface FathersRightsViolationCase : Case
{
}
