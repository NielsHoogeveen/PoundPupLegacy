namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DenominationInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableDenomination>
{
    protected override string TableName => "denomination";

    protected override bool AutoGenerateIdentity => false;

}
