-- Index: changed_date_time_desc

-- DROP INDEX IF EXISTS public.changed_date_time_desc;

CREATE INDEX IF NOT EXISTS changed_date_time_desc
    ON public.node USING btree
    (changed_date_time ASC NULLS LAST)
    TABLESPACE pg_default;

insert into menu_item(weight) values(0.5);

insert into action default values;

insert into action_menu_item(id, action_id, name) values(25, 394, 'Recent posts');

insert into basic_action(id, path, description) values(394, '/recent', 'Show list of recent posts');

insert into access_role_privilege(access_role_id, action_id) values(12, 394);
insert into access_role_privilege(access_role_id, action_id) values(16, 394);