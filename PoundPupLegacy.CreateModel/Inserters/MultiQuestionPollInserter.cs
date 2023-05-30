namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class MultiQuestionPollInserterFactory : SingleIdInserterFactory<MultiQuestionPoll.ToCreate>
{
    protected override string TableName => "multi_question_poll";

    protected override bool AutoGenerateIdentity => false;

}
