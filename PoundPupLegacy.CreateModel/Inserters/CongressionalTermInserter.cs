namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CongressionalTermInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableCongressionalTerm>
{
    protected override string TableName => "congressional_term";

    protected override bool AutoGenerateIdentity => false;

}
