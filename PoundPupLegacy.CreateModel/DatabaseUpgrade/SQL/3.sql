ALTER TABLE IF EXISTS public."user"
    ADD COLUMN expiry_date_time timestamp without time zone;

ALTER TABLE user_group_user_role_user 
  RENAME TO user_role_user;

create view user_group_user_role_user AS
select 
uru.user_role_id,
uru.user_id,
ur.user_group_id
from user_role_user uru
join user_role ur on ur.id = uru.user_role_id;