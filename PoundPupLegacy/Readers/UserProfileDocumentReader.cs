namespace PoundPupLegacy.Readers;

using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using Request = UserProfileDocumentReaderRequest;

public sealed record UserProfileDocumentReaderRequest : IRequest
{
    public required int UserId { get; init; }
}

internal sealed class UserProfileDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, UserProfile>
{
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };

    private static readonly FieldValueReader<UserProfile> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = $"""
        select
        	jsonb_build_object(
                'Id',
                @user_id,
                'Name',
                p.name,
                'AboutMe',
                about_me,
                'AnimalWithin',
                animal_within,
                'Avatar',
                avatar,
                'RelationToChildPlacementId',
                relation_to_child_placement_id,
                'RelationsToChildPlacement',
                (
                    select
                        jsonb_agg(
                            jsonb_build_object(
                            'Id',
                            id,
                            'Name',
                            name
                            )
                        )
                    from relation_to_child_placement
                )
        	) document
            from "user" u 
            left join publisher p on p.id = u.id
            where u.id = @user_id
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(UserIdParameter, request.UserId),
        };
    }

    protected override UserProfile Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
