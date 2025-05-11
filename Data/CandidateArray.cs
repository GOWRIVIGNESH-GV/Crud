using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Crud.Data
{
    [OracleCustomTypeMapping("SYS.CANDIDATE_TABLE_TYPE")]
    public class CandidateArray : IOracleCustomType, IOracleArrayTypeFactory
    {
        private CandidateUDT[] _array;
        public Array CreateArray(int numElems)
        {
            return new CandidateUDT[numElems];
        }

        public Array CreateStatusArray(int numElems)
        {
            return null;
        }

        public void FromCustomObject(OracleConnection con, object udt)
        {

        }

        public void ToCustomObject(OracleConnection con, object udt)
        {
            OracleUdt.SetValue(con, udt, 0, _array);
        }
        public CandidateUDT[] Candidates
        {
            get => _array;
            set => _array = value;
        }
    }
}