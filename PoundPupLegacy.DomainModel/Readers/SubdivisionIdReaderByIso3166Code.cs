﻿namespace PoundPupLegacy.DomainModel.Readers;

using Request = SubdivisionIdReaderByIso3166CodeRequest;


public sealed record SubdivisionIdReaderByIso3166CodeRequest : IRequest
{
    public required string Iso3166Code { get; init; }
}


internal sealed class SubdivisionIdReaderByIso3166CodeFactory : IntDatabaseReaderFactory<Request>
{
    private static readonly NonNullableStringDatabaseParameter Iso3166Code = new() { Name = "iso_3166_2_code" };

    private static readonly IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT id
        FROM public.iso_coded_subdivision 
        WHERE iso_3166_2_code = @iso_3166_2_code 
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Iso3166Code, request.Iso3166Code)
        };
    }

    protected override IntValueReader IntValueReader => IdReader;

    protected override string GetErrorMessage(Request request)
    {
        return $"subdivision with code {request} cannot be found";
    }

}