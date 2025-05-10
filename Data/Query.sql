-- country
create table t_tb_country (
   id           number
      generated always as identity ( start with 1 increment by 1 minvalue 1 maxvalue 99999 nocache nocycle )
   primary key,
   country_name varchar(100),
   created_by   number(8),
   created_time date,
   updated_by   number(8),
   updated_time date,
   is_deleted   char default 'N'
);

insert into t_tb_country (
   country_name,
   created_by,
   created_time
) values ( 'India',
           1,
           sysdate );
insert into t_tb_country (
   country_name,
   created_by,
   created_time
) values ( 'Russia',
           1,
           sysdate );
insert into t_tb_country (
   country_name,
   created_by,
   created_time
) values ( 'United States',
           1,
           sysdate );
insert into t_tb_country (
   country_name,
   created_by,
   created_time
) values ( 'China',
           1,
           sysdate );
insert into t_tb_country (
   country_name,
   created_by,
   created_time
) values ( 'Brazil',
           1,
           sysdate );
insert into t_tb_country (
   country_name,
   created_by,
   created_time
) values ( 'Germany',
           1,
           sysdate );
insert into t_tb_country (
   country_name,
   created_by,
   created_time
) values ( 'France',
           1,
           sysdate );
insert into t_tb_country (
   country_name,
   created_by,
   created_time
) values ( 'Japan',
           1,
           sysdate );
insert into t_tb_country (
   country_name,
   created_by,
   created_time
) values ( 'South Korea',
           1,
           sysdate );
insert into t_tb_country (
   country_name,
   created_by,
   created_time
) values ( 'Australia',
           1,
           sysdate );
insert into t_tb_country (
   country_name,
   created_by,
   created_time
) values ( 'United Kingdom',
           1,
           sysdate );
insert into t_tb_country (
   country_name,
   created_by,
   created_time
) values ( 'Canada',
           1,
           sysdate );
insert into t_tb_country (
   country_name,
   created_by,
   created_time
) values ( 'Italy',
           1,
           sysdate );
insert into t_tb_country (
   country_name,
   created_by,
   created_time
) values ( 'South Africa',
           1,
           sysdate );
insert into t_tb_country (
   country_name,
   created_by,
   created_time
) values ( 'Mexico',
           1,
           sysdate );
insert into t_tb_country (
   country_name,
   created_by,
   created_time
) values ( 'Indonesia',
           1,
           sysdate );

create or replace procedure sp_tb_get_country (
   pid     in number,
   mresult out sys_refcursor
) as
begin
   open mresult for select id,
                           country_name as countryname
                                       from t_tb_country
                     where id = pid
                        or pid = 0;

exception
   when others then
      raise;
end sp_tb_get_country;


-- candidate


create table t_tb_candidate (
   id           number
      generated always as identity ( start with 1 increment by 1 minvalue 1 maxvalue 999999999 nocache nocycle )
   primary key,
   name         varchar2(50) not null,
   gender_id    number(2) not null,
   skillset     varchar2(150),
   phone        varchar2(20),
   email        varchar2(100),
   address      varchar2(100),
   country_id   number(4),
   created_by   number(8),
   created_time date,
   updated_by   number(8),
   updated_time date,
   is_deleted   char default 'N'
);



create or replace procedure sp_tb_get_candidate (
   pid     in number,
   mresult out sys_refcursor
) as
begin
   open mresult for select c.id,
                           c.name,
                           c.gender_id,
                           c.skillset,
                           c.phone,
                           c.email,
                           c.address,
                           c.country_id,
                           cn.country_name
                                       from t_tb_candidate c
                                       left join t_tb_country cn
                                     on cn.id = c.country_id
                     where c.id = pid
                        or pid = 0;

exception
   when others then
      raise;
end sp_tb_get_candidate;



-- single entity insert / update 

create or replace procedure sp_tb_insert_candidate (
   pid         in number,
   pname       in varchar,
   pgender_id  in number,
   pskillset   in varchar,
   pphone      in varchar,
   pemail      in varchar,
   paddress    in varchar,
   pcountry_id in number,
   pupdated_by in number,
   mresult     out sys_refcursor
) as
   vemail_pattern varchar2(100) := '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$';
   vphone_pattern varchar2(20) := '^[0-9]{10}$';
   vfound         number := 0;
   vai            number := 0;
begin
   if not regexp_like(
      pemail,
      vemail_pattern
   ) then
      raise_application_error(
         -20002,
         'Invalid email.'
      );
   end if;


   if not regexp_like(
      pphone,
      vphone_pattern
   ) then
      raise_application_error(
         -20002,
         'Invalid phone number.'
      );
   end if;


   select count(*)
     into vfound
     from t_tb_candidate
    where is_deleted = 'N'
      and id <> pid
      and phone = phone;

   if vfound > 0 then
    --   open mresult for select 'Phone No already registered.' as errorraise
    --                      from dual;

      raise_application_error(
         -20002,
         'Phone No already registered.'
      );
      return;
   end if;
   select count(*)
     into vfound
     from t_tb_candidate
    where is_deleted = 'N'
      and id <> pid
      and email = pemail;

   if vfound > 0 then
      raise_application_error(
         -20002,
         'Email has already registered.'
      );
      return;
   end if;
   if pid > 0 then -- UPDATE
      update t_tb_candidate
         set name = pname,
             gender_id = pgender_id,
             skillset = pskillset,
             phone = pphone,
             email = pemail,
             address = paddress,
             country_id = pcountry_id,
             updated_by = pupdated_by,
             updated_time = sysdate
       where id = pid
         and is_deleted = 'N';
      vai := pid;
   else -- INSERT

      insert into t_tb_candidate (
         name,
         gender_id,
         skillset,
         phone,
         email,
         address,
         country_id,
         created_by,
         created_time
      ) values ( pname,
                 pgender_id,
                 pskillset,
                 pphone,
                 pemail,
                 paddress,
                 pcountry_id,
                 pupdated_by,
                 sysdate ) returning id into vai;


   end if;

   open mresult for select vai as id
                     from dual;
exception
   when others then
      raise;
end sp_tb_insert_candidate;


-- candidate delete

create procedure sp_tb_del_candidate (
   pid         in number,
   pupdated_by in number,
   mresult     out sys_refcursor
) as
   vfound number := 0;
begin
   select count(*)
     into vfound
     from t_tb_candidate
    where is_deleted = 'N'
      and id = pid;

   if vfound = 0 then
      raise_application_error(
         -20002,
         'Requesting candidate data not exist.'
      );
      return;
   end if;
   update t_tb_candidate
      set is_deleted = 'Y',
          updated_by = pupdated_by,
          updated_time = sysdate
    where id = pid
      and is_deleted = 'N';

   open mresult for select pid
                     from dual;

exception
   when others then
      raise;
end sp_tb_del_candidate;