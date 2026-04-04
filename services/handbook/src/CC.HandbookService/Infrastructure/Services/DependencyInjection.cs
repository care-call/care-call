using CC.HandbookService.Application.Dependencies;
using CC.HandbookService.Domain.Handbooks;
using CC.HandbookService.Infrastructure.Services.CsvParsing;

namespace CC.HandbookService.Infrastructure.Services;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddServices()
        {
            return services
                .AddSingleton<IHandbookParser, HandbookParser>()
                .AddHandbookLoaders();
        }

        private IServiceCollection AddHandbookLoaders()
        {
            return services
                .AddKeyedScoped<IHandbookLoader, HandbookLoader<Language>>(HandbookType.Languages)
                .AddKeyedScoped<IHandbookLoader, HandbookLoader<AgeGroup>>(HandbookType.AgeGroups)
                .AddKeyedScoped<IHandbookLoader, HandbookLoader<ProblemArea>>(HandbookType.ProblemAreas);
        }
    }
}