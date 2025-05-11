using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using Oracle.ManagedDataAccess.Client;

namespace Crud.Helper
{
    public static class ErrorHelper
    {
        public static string ConvertErrors(List<IError> errors)
        {
            return string.Join(",", errors.Select(e => e.Message));
        }

        public static string GetCustomUDE(OracleException err)
        {
            var error = "";
            if (err.Number.ToString().Contains("20002"))
            {
                error = err.Message.Substring("ORA-20002:".Length).Trim();
            }
            else
                error = $"DB error : {err.Message}";
            return error;
        }
    }
}