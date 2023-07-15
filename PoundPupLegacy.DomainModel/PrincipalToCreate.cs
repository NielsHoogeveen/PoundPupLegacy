namespace PoundPupLegacy.DomainModel;

public interface PrincipalToCreate : Principal, PossiblyIdentifiable
{
}

public interface PrincipalToUpdate : Principal, CertainlyIdentifiable
{
}

public interface Principal
{
}

