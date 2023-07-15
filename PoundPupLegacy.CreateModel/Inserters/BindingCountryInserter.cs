using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class BindingCountryInserterFactory : SingleIdInserterFactory<BindingCountry.ToCreate>
{
    protected override string TableName => "binding_country";

    protected override bool AutoGenerateIdentity => false;

}
