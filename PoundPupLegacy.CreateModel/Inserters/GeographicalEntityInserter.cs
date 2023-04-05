namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class GeographicalEntityInserterFactory : SingleIdInserterFactory<GeographicalEntity>
{
    protected override string TableName => "geographical_entity";

    protected override bool AutoGenerateIdentity => false;

}
