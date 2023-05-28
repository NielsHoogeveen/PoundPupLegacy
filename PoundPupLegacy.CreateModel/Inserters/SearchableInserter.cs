namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class SearchableInserterFactory : SingleIdInserterFactory<SearchableToCreate>
{
    protected override string TableName => "searchable";

    protected override bool AutoGenerateIdentity => false;

}
