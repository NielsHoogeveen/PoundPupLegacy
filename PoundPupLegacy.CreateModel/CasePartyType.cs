namespace PoundPupLegacy.CreateModel;
public sealed record NewCasePartyType : NewNameableBase, EventuallyIdentifiableCasePartyType
{
}
public sealed record ExistingCasePartyType : ExistingNameableBase, ImmediatelyIdentifiableCasePartyType
{
}
public interface ImmediatelyIdentifiableCasePartyType : CasePartyType, ImmediatelyIdentifiableNameable
{
}
public interface EventuallyIdentifiableCasePartyType : CasePartyType, EventuallyIdentifiableNameable
{
}
public interface CasePartyType: Nameable
{
}
