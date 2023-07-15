namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class GeographicalEntityInserterFactory : SingleIdInserterFactory<GeographicalEntityToCreate>
{
    protected override string TableName => "geographical_entity";

    protected override bool AutoGenerateIdentity => false;

}
