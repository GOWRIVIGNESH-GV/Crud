using System.ComponentModel.DataAnnotations;
namespace Crud.Models
{
    public class CandidateModel
    {

        public int CandidateId { get; set; }
        public string Name { get; set; }
        public int GenderId { get; set; }
        public string Gender { get; set; }
        public string SkillSet { get; set; }
        public List<string> Skills { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}