using System.Data;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Crud.Models;
using FluentResults;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Crud.Repositories
{


    public class CandidateRepository : ICandidateRepository
    {

        private readonly string _connectionString;

        public CandidateRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleDb");
        }

        public async Task<Result<List<CountryModel>>> GetCountries(int id)
        {
            List<CountryModel> countries = new List<CountryModel>();

            using (var conn = new OracleConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    string query = "SP_TB_GET_COUNTRY";

                    using (var cmd = new OracleCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("pID", OracleDbType.Int32, ParameterDirection.Input).Value = id;
                        cmd.Parameters.Add("mResult", OracleDbType.RefCursor, ParameterDirection.Output);

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                var country = new CountryModel
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    CountryName = reader["countryname"].ToString()
                                };

                                countries.Add(country);
                            }
                        }
                    }

                    return Result.Ok(countries);
                }
                catch (OracleException ex)
                {
                    return Result.Fail($"Oracle error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    return Result.Fail($"Unexpected error: {ex.Message}");
                }

            }
        }

        public async Task<Result<List<CandidateModel>>> GetCandidateAsync(int candidateId)
        {
            var candidates = new List<CandidateModel>();

            try
            {
                using (var conn = new OracleConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (var cmd = new OracleCommand("SP_TB_GET_CANDIDATE", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("pID", OracleDbType.Int32).Value = candidateId;
                        cmd.Parameters.Add("mResult", OracleDbType.RefCursor, ParameterDirection.Output);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {

                                var id = reader.GetInt32(reader.GetOrdinal("ID"));
                                var name = reader.IsDBNull(reader.GetOrdinal("NAME")) ? null : reader.GetString(reader.GetOrdinal("NAME"));
                                var genderId = reader.GetInt32(reader.GetOrdinal("GENDER_ID"));
                                var skillSet = reader.IsDBNull(reader.GetOrdinal("SKILLSET")) ? null : reader.GetString(reader.GetOrdinal("SKILLSET"));
                                var phone = reader.IsDBNull(reader.GetOrdinal("PHONE")) ? null : reader.GetString(reader.GetOrdinal("PHONE"));
                                var mail = reader.IsDBNull(reader.GetOrdinal("EMAIL")) ? null : reader.GetString(reader.GetOrdinal("EMAIL"));
                                var address = reader.IsDBNull(reader.GetOrdinal("ADDRESS")) ? null : reader.GetString(reader.GetOrdinal("ADDRESS"));
                                var countryId = reader.GetInt32(reader.GetOrdinal("COUNTRY_ID"));
                                var candidate = new CandidateModel
                                {
                                    CandidateId = id,
                                    Name = name,
                                    GenderId = genderId,
                                    SkillSet = skillSet,
                                    Skills = skillSet.Split(',').ToList(),
                                    Phone = phone,
                                    Email = mail,
                                    Address = address
                                };
                                candidates.Add(candidate);
                            }
                        }
                    }

                    return Result.Ok(candidates);
                }
            }
            catch (OracleException ex)
            {
                return Result.Fail($"Oracle error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Unexpected error: {ex.Message}");
            }
        }



        public async Task<Result<int>> DeleteAsync(int candidateId, int updatedBy)
        {

            try
            {
                using var conn = new OracleConnection(_connectionString);
                await conn.OpenAsync();

                using (var cmd = new OracleCommand("SP_TB_DEL_CANDIDATE", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new OracleParameter("pID", OracleDbType.Int32) { Value = candidateId });
                    cmd.Parameters.Add(new OracleParameter("pUPDATED_BY", OracleDbType.Int32) { Value = updatedBy });


                    var outputParam = new OracleParameter("mResult", OracleDbType.RefCursor)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    await cmd.ExecuteNonQueryAsync();


                    using var reader = ((OracleRefCursor)outputParam.Value).GetDataReader();
                    if (reader.Read())
                    {
                        int deletedId = reader.GetInt32(0);
                        return Result.Ok(deletedId);
                    }

                    return Result.Fail("Failed to delete the candidate.");

                }
                ;
            }
            catch (OracleException ex) when (ex.Number == 20002)
            {
                return Result.Fail("The requested candidate does not exist or is already deleted.");
            }
            catch (OracleException ex)
            {

                return Result.Fail($"Database error: {ex.Message} ");
            }
            catch (Exception ex)
            {
                return Result.Fail("An unexpected error occurred while deleting the candidate.");
            }

        }




        public async Task<Result<int>> InsertAsync(CandidateModel candidate)
        {
            try
            {
                using (var conn = new OracleConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (var cmd = new OracleCommand("SP_TB_INSERT_CANDIDATE", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("pID", OracleDbType.Int32).Value = candidate.CandidateId;
                        cmd.Parameters.Add("pNAME", OracleDbType.Varchar2).Value = candidate.Name;
                        cmd.Parameters.Add("pGENDER_ID", OracleDbType.Int32).Value = candidate.GenderId;
                        cmd.Parameters.Add("pSKILLSET", OracleDbType.Varchar2).Value = candidate.SkillSet;
                        cmd.Parameters.Add("pPHONE", OracleDbType.Varchar2).Value = candidate.Phone;
                        cmd.Parameters.Add("pEMAIL", OracleDbType.Varchar2).Value = candidate.Email;
                        cmd.Parameters.Add("pADDRESS", OracleDbType.Varchar2).Value = candidate.Address;
                        cmd.Parameters.Add("pCOUNTRY_ID", OracleDbType.Int32).Value = candidate.CountryId;


                        var outputParam = new OracleParameter("mResult", OracleDbType.RefCursor)
                        {
                            Direction = ParameterDirection.Output
                        };

                        cmd.Parameters.Add(outputParam);

                        await cmd.ExecuteNonQueryAsync();

                        using var reader = ((OracleRefCursor)outputParam.Value).GetDataReader();
                        if (reader.Read())
                        {
                            int newId = reader.GetInt32(0);
                            return Result.Ok(newId);
                        }

                        return Result.Fail("Failed to update the candidate.");
                    }
                }
            }
            catch (OracleException ex)
            {
                return Result.Fail($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Unexpected error: {ex.Message}");
            }
        }


        public Task<Result<int>> InsertBulkAsync(List<CandidateModel> candidates)
        {
            throw new NotImplementedException();
        }
    }
}