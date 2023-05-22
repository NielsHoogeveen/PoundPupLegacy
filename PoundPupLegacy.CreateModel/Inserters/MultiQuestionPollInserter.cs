namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class MultiQuestionPollInserterFactory : SingleIdInserterFactory<NewMultiQuestionPoll>
{
    protected override string TableName => "multi_question_poll";

    protected override bool AutoGenerateIdentity => false;

}
