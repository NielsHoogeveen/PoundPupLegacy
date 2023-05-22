namespace PoundPupLegacy.CreateModel;

public sealed record NewBasicPollQuestion : NewPollQuestionBase, EventuallyIdentifiablePollQuestion
{
}
public sealed record ExistingBasicPollQuestion : ExistingPollQuestionBase, ImmediatelyIdentifiablePollQuestion
{
}
