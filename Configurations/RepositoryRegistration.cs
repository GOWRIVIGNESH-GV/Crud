using Crud.Repositories;


public static class RepositoryRegistration
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICandidateRepository, CandidateRepository>();
    }
}
