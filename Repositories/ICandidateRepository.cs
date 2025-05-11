using Crud.Models;
using FluentResults;

namespace Crud.Repositories
{

    public interface ICandidateRepository
    {
        Task<Result<int>> InsertAsync(CandidateModel candidate);
        Task<Result<int>> InsertBulkAsync(List<CandidateModel> candidates, int userId);
        Task<Result<int>> DeleteAsync(int candidateId, int updatedBy);
        Task<Result<List<CandidateModel>>> GetCandidateAsync(int candidateId);
        Task<Result<List<CountryModel>>> GetCountries(int id);

    }
}