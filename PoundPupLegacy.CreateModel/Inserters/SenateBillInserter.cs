namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SenateBillInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableSenateBill>
{
    protected override string TableName => "senate_bill";

    protected override bool AutoGenerateIdentity => false;

}
