using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Crud.Data
{
    [OracleCustomTypeMapping("SYS.CANDIDATE_OBJ")]
    public class CandidateUDT : IOracleCustomType, IOracleCustomTypeFactory
    {

        [OracleObjectMapping("NAME")]
        public string? Name { get; set; }
        [OracleObjectMapping("GENDER_ID")]
        public int GenderId { get; set; }
        [OracleObjectMapping("SKILLSET")]
        public string? SkillSet { get; set; }
        [OracleObjectMapping("PHONE")]
        public string? Phone { get; set; }
        [OracleObjectMapping("EMAIL")]
        public string? Email { get; set; }
        [OracleObjectMapping("ADDRESS")]
        public string? Address { get; set; }
        [OracleObjectMapping("COUNTRY_ID")]
        public int CountryId { get; set; }



        public void FromCustomObject(OracleConnection con, object udt)
        {

        }

        public void ToCustomObject(OracleConnection con, object udt)
        {

            OracleUdt.SetValue(con, udt, "NAME", Name);
            OracleUdt.SetValue(con, udt, "GENDER_ID", GenderId);
            OracleUdt.SetValue(con, udt, "SKILLSET", SkillSet);
            OracleUdt.SetValue(con, udt, "PHONE", Phone);
            OracleUdt.SetValue(con, udt, "EMAIL", Email);
            OracleUdt.SetValue(con, udt, "ADDRESS", Address);
            OracleUdt.SetValue(con, udt, "COUNTRY_ID", CountryId);
        }

        public IOracleCustomType CreateObject()
        {
            return new CandidateUDT();
        }


    }
}