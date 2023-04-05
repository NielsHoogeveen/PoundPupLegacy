namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CongressionalTermInserterFactory : SingleIdInserterFactory<CongressionalTerm>
{
    protected override string TableName => "congressional_term";

    protected override bool AutoGenerateIdentity => false;

}
