using Crud.Helper;
using Crud.Models;
using Crud.Repositories;
using Crud.Services;
using FluentResults;

namespace Crud.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _repository;

        public CandidateService(ICandidateRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result<List<CountryModel>>> GetCountries()
        {
            return await Task.Run(() => _repository.GetCountries(0));
        }
        public async Task<Result<List<CandidateModel>>> GetAllAsync()
        {
            return await _repository.GetCandidateAsync(0);
        }

        public async Task<Result<CandidateModel>> GetAsync(int candidateId)
        {
            var res = await _repository.GetCandidateAsync(candidateId);

            if (res.IsFailed || res.Value == null)
            {
                return Result.Fail(ErrorHelper.ConvertErrors(res.Errors));
            }

            var entity = res.Value.FirstOrDefault();

            if (entity == null)
            {
                return Result.Fail("Candidate not exist.");
            }

            return Result.Ok(entity);
        }

        public async Task<Result<int>> DeleteAsync(int candidateId, int updatedBy)
        {
            if (candidateId <= 0)
            {
                return Result.Fail("Invalid candidate Id");
            }
            if (updatedBy <= 0)
                return Result.Fail("Invalid request Id");

            var res = await _repository.DeleteAsync(candidateId, updatedBy);

            return res;
        }


        public async Task<Result<int>> InsertAsync(CandidateModel candidate)
        {
            var res = await _repository.InsertAsync(candidate);
            return res;
        }

        public async Task<Result<int>> InsertBulkAsync(List<CandidateModel> candidates)
        {
            if (candidates.Count == 1)
            {
                var entity = candidates.FirstOrDefault();
                return await _repository.InsertAsync(entity);
            }

            var res = await _repository.InsertBulkAsync(candidates);
            return res;
        }
    }
}