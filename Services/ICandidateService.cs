using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crud.Models;
using FluentResults;

namespace Crud.Services
{
    public interface ICandidateService
    {
        Task<Result<List<CountryModel>>> GetCountries();
        Task<Result<int>> InsertAsync(CandidateModel candidate);
        Task<Result<int>> InsertBulkAsync(List<CandidateModel> candidates, int userId);
        Task<Result<int>> DeleteAsync(int candidateId, int updatedBy);
        Task<Result<CandidateModel>> GetAsync(int candidateId);
        Task<Result<List<CandidateModel>>> GetAllAsync();
    }
}