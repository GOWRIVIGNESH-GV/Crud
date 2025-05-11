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
                     where ( id = pid
                        or pid = 0 )
                       and is_deleted = 'N';

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
                           g.gender,
                           c.skillset,
                           c.phone,
                           c.email,
                           c.address,
                           c.country_id,
                           cn.country_name
                                       from t_tb_candidate c
                                       left join t_tb_country cn
                                     on cn.id = c.country_id
                                       left join t_tb_gender g
                                     on g.id = c.gender_id
                     where c.is_deleted = 'N'
                       and ( c.id = pid
                        or pid = 0 );

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




create or replace type candidate_obj as object (
      id         number,
      name       varchar2(50),
      gender_id  number,
      skillset   varchar2(150),
      phone      varchar2(20),
      email      varchar2(100),
      address    varchar2(100),
      country_id number
);

create or replace type candidate_table_type as
   table of candidate_obj;

create or replace procedure sp_tb_insert_blk_candidate (
   pcandidates in candidate_table_type,
   pcreated_by in number,
   mresult     out sys_refcursor
) as
   vemail_pattern varchar2(100) := '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$';
   vphone_pattern varchar2(20) := '^[0-9]{10}$';
   type emaillist is
      table of varchar2(100) index by pls_integer;
   type phonelist is
      table of varchar2(20) index by pls_integer;
   vemails        emaillist;
   vphones        phonelist;
   vdbemails      sys.odcivarchar2list;
   vdbphones      sys.odcivarchar2list;
begin
   for i in 1..pcandidates.count loop
      if not regexp_like(
         pcandidates(i).email,
         vemail_pattern
      ) then
         raise_application_error(
            -20002,
            'Invalid email format: ' || pcandidates(i).email
         );
      end if;


      if not regexp_like(
         pcandidates(i).phone,
         vphone_pattern
      ) then
         raise_application_error(
            -20002,
            'Invalid phone format: ' || pcandidates(i).phone
         );
      end if;

      vemails(i) := pcandidates(i).email;
      vphones(i) := pcandidates(i).phone;
   end loop;


   select email
   bulk collect
     into vdbemails
     from t_tb_candidate
    where is_deleted = 'N'
      and email in (
      select column_value
        from table ( cast(vemails as sys.odcivarchar2list) )
   );

   if vdbemails.count > 0 then
      raise_application_error(
         -20002,
         'Emails already registered: ' || vdbemails(1)
      );
   end if;


   select phone
   bulk collect
     into vdbphones
     from t_tb_candidate
    where is_deleted = 'N'
      and phone in (
      select column_value
        from table ( cast(vphones as sys.odcivarchar2list) )
   );

   if vdbphones.count > 0 then
      raise_application_error(
         -20002,
         'Phone numbers already registered: ' || vdbphones(1)
      );
   end if;


   forall i in 1..pcandidates.count
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
      ) values ( pcandidates(i).name,
                 pcandidates(i).gender_id,
                 pcandidates(i).skillset,
                 pcandidates(i).phone,
                 pcandidates(i).email,
                 pcandidates(i).address,
                 pcandidates(i).country_id,
                 pcreated_by,
                 sysdate );


   open mresult for select pcreated_by as user_id
                     from dual;

exception
   when others then
      raise;
end;




create or replace procedure sp_tb_insert_blk_candidate (
   pnames      in sys.odcivarchar2list,
   pgenders    in sys.odcinumberlist,
   pskillsets  in sys.odcivarchar2list,
   pphones     in sys.odcivarchar2list,
   pemails     in sys.odcivarchar2list,
   paddresses  in sys.odcivarchar2list,
   pcountries  in sys.odcinumberlist,
   pcreated_by in number,
   mresult     out sys_refcursor
) as
   vemail_pattern varchar2(100) := '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$';
   vphone_pattern varchar2(20) := '^[0-9]{10}$';
   vdbemails      sys.odcivarchar2list;
   vdbphones      sys.odcivarchar2list;
begin
   for i in 1..pnames.count loop
      if not regexp_like(
         pemails(i),
         vemail_pattern
      ) then
         raise_application_error(
            -20002,
            'Invalid email format: ' || pemails(i)
         );
      end if;

      if not regexp_like(
         pphones(i),
         vphone_pattern
      ) then
         raise_application_error(
            -20002,
            'Invalid phone format: ' || pphones(i)
         );
      end if;
   end loop;


   select email
   bulk collect
     into vdbemails
     from t_tb_candidate
    where is_deleted = 'N'
      and email in (
      select column_value
        from table ( cast(pemails as sys.odcivarchar2list) )
   );

   if vdbemails.count > 0 then
      raise_application_error(
         -20002,
         'Emails already registered: ' || vdbemails(1)
      );
   end if;


   select phone
   bulk collect
     into vdbphones
     from t_tb_candidate
    where is_deleted = 'N'
      and phone in (
      select column_value
        from table ( cast(pphones as sys.odcivarchar2list) )
   );

   if vdbphones.count > 0 then
      raise_application_error(
         -20002,
         'Phone numbers already registered: ' || vdbphones(1)
      );
   end if;


   forall i in 1..pnames.count
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
      ) values ( pnames(i),
                 pgenders(i),
                 pskillsets(i),
                 pphones(i),
                 pemails(i),
                 paddresses(i),
                 pcountries(i),
                 pcreated_by,
                 sysdate );


   open mresult for select pcreated_by as user_id
                     from dual;
exception
   when others then
      raise;
end;


-- json 

create or replace procedure sp_tb_insert_blk_candidate (
   pcandidate  in clob,
   pcreated_by in number,
   mresult     out sys_refcursor
) as
   l_json_data  clob := pcandidate;
   l_pos        pls_integer := 1;
   l_name       varchar2(100);
   l_gender_id  number;
   l_skillset   varchar2(100);
   l_phone      varchar2(50);
   l_email      varchar2(100);
   l_address    varchar2(200);
   l_country_id number;
begin
    -- Loop through each JSON object manually using JSON_TABLE
   for rec in (
      select *
        from
         json_table ( l_json_data,'$[*]'
            columns (
               name varchar2 ( 100 ) path '$.Name',
               gender_id number path '$.GenderId',
               skillset varchar2 ( 100 ) path '$.SkillSet',
               phone varchar2 ( 50 ) path '$.Phone',
               email varchar2 ( 100 ) path '$.Email',
               address varchar2 ( 200 ) path '$.Address',
               country_id number path '$.CountryId'
            )
         )
   ) loop
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
      ) values ( rec.name,
                 rec.gender_id,
                 rec.skillset,
                 rec.phone,
                 rec.email,
                 rec.address,
                 rec.country_id,
                 pcreated_by,
                 sysdate );
   end loop;

   open mresult for select count(*) as inserted_count
                                     from t_tb_candidate
                    where created_by = pcreated_by;

exception
   when others then
      raise;
end;