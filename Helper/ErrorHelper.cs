using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;

namespace Crud.Helper
{
    public static class ErrorHelper
    {
        public static string ConvertErrors(List<IError> errors)
        {
            return string.Join(",", errors.Select(e => e.Message));
        }
    }
}