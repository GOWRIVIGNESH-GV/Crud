using Crud.Services;


public static class ServiceRegistration
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICandidateService, CandidateService>();
    }
}


