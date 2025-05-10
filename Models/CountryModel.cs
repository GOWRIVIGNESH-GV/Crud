using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.Models
{
    public class CountryModel
    {
        public int Id { get; set; }
        public string? CountryName { get; set; }
        public int CreatedBy { get; set; }

    }
}