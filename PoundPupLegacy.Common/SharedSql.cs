namespace PoundPupLegacy.Common;

public class SharedSql
{

    public const string ACCESSIBLE_PUBLICATIONS_STATUS = """
        accessible_publication_status as(
            select 
        	    publication_status_id id,
        	    tenant_id,
        	    subgroup_id
            from user_publication_status
            where user_id = @user_id
        )
        """;

    public const string ACCESSIBLE_PUBLICATIONS_STATUS_OLD = """
        accessible_publication_status as(
            select 
        	    distinct
        	    id,
        	    tenant_id,
        	    subgroup_id
            from(
        	    select
        	    urps.publication_status_id id,
        	    t.id tenant_id,
        	    sg.id subgroup_id,
        	    uru.user_id
        	    from publishing_user_group pug 
        	    left join subgroup sg on sg.id = pug.id
        	    join tenant t on t.id = case 
        		    when sg.id is not null then sg.tenant_id
        		    else pug.id
        	    end
        	    join user_role ur on ur.user_group_id = pug.id
        	    join user_role_user uru on uru.user_role_id = ur.id
        	    join user_role_publication_status urps on urps.user_role_id = ur.id
        	    union
        	    select
        	    distinct
        	    ps.id,
        	    t.id tenant_id,
        	    sg.id subgroup_id,
        	    uru.user_id
        	    from publishing_user_group pug 
        	    left join subgroup sg on sg.id = pug.id
        	    join tenant t on t.id = case 
        		    when sg.id is not null then sg.tenant_id
        		    else pug.id
        	    end
        	    join user_role_user uru on uru.user_role_id = t.administrator_role_id
        	    join publication_status ps on 1=1
        	    union
        	    select
        	    urps.publication_status_id id,
        	    t.id tenant_id,
        	    sg.id subgroup_id,
        	    0 user_id
        	    from publishing_user_group pug 
        	    left join subgroup sg on sg.id = pug.id
        	    join tenant t on t.id = case 
        		    when sg.id is not null then sg.tenant_id
        		    else pug.id
        	    end
        	    join user_role_publication_status urps on urps.user_role_id = pug.access_role_id_not_logged_in

            ) x where user_id = @user_id
        )
        """;

}
