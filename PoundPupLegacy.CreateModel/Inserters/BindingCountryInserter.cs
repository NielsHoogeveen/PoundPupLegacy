namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BindingCountryInserterFactory : SingleIdInserterFactory<NewBindingCountry>
{
    protected override string TableName => "binding_country";

    protected override bool AutoGenerateIdentity => false;

}
