namespace PoundPupLegacy.ViewModel.Readers;

using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using Request = UsersRolesToAsignReaderRequest;

public sealed record UsersRolesToAsignReaderRequest : IRequest
{
    public required int UserId { get; init; }
}
internal sealed class UsersRolesToAsignReaderFactory : SingleItemDatabaseReaderFactory<Request, List<UserRolesToAssign>>
{
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };

    private static readonly FieldValueReader<List<UserRolesToAssign>> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = $"""
        select
        json_agg(
        	json_build_object(
        		'UserId',
        		user_id,
        		'UserName',
        		user_name,
        		'RegistrationReason',
        		registration_reason,
        		'Tenants',
        		tenants
        	)
        ) document
        from(
        	select
        	user_id,
        	user_name,
        	registration_reason,
        	jsonb_agg(
        		jsonb_build_object(
        			'Id',
        			tenant_id,
        			'DomainName',
        			domain_name,
        			'UserRoles',
        			user_roles
        		)
        	) tenants
        	from(
        		select 
        		u.id user_id,
        		p.name user_name,
        		u.registration_reason,
        		t.id tenant_id,
        		t.domain_name,
        		jsonb_agg(
        			jsonb_build_object(
        				'Id',
        				ur.id,
        				'Name',
        				ur.name
        			)
        		) user_roles
        		from "user" u
        		join publisher p on p.id = u.id
        		join user_role_user uru on uru.user_id = @user_id
        		join tenant t on t.administrator_role_id = uru.user_role_id
                join publishing_user_group pug on pug.id = t.id
        		join user_role ur on ur.user_group_id = t.id 
                    and ur.id <> pug.access_role_id_not_logged_in
                    and ur.id <> t.administrator_role_id
        		where u.user_status_id = 2
        		group by u.id, p.name, registration_reason, tenant_id, domain_name
        	) x
        group by user_id, user_name, registration_reason
        ) x
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(UserIdParameter, request.UserId),
        };
    }

    protected override List<UserRolesToAssign> Read(NpgsqlDataReader reader)
    {
        if (reader.IsDBNull(0)) {
            return new();
        }
        return DocumentReader.GetValue(reader);
    }
}
