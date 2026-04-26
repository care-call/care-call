using CC.HandbookService.Application.Dependencies;
using CC.HandbookService.Domain.Handbooks;
using CC.HandbookService.Infrastructure.Services.CsvParsing;
using CC.HandbookService.Infrastructure.Services.CsvParsing.HandbookMaps;
using CsvHelper.Configuration;

namespace CC.HandbookService.Infrastructure.Services;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddServices()
        {
            return services
                .AddSingleton<IHandbookParser, HandbookParser>()
                .AddHandbookLoaders()
                .AddHandbookMaps()
                .AddHandbookQueryServices();
        }

        private IServiceCollection AddHandbookLoaders()
        {
            return services
                .AddKeyedScoped<IHandbookLoader, HandbookLoader<Language>>(HandbookType.Languages)
                .AddKeyedScoped<IHandbookLoader, HandbookLoader<AgeGroup>>(HandbookType.AgeGroups)
                .AddKeyedScoped<IHandbookLoader, HandbookLoader<ProblemArea>>(HandbookType.ProblemAreas);
        }
        
        private IServiceCollection AddHandbookMaps()
        {
            return services
                .AddKeyedTransient<ClassMap, LanguageMap>(typeof(Language))
                .AddKeyedTransient<ClassMap, AgeGroupMap>(typeof(AgeGroup))
                .AddKeyedTransient<ClassMap, ProblemAreaMap>(typeof(ProblemArea));
        }



        private IServiceCollection AddHandbookQueryServices()
        {
            return services
                .AddKeyedScoped<IHandbookQueryService, HandbookQueryService<Language>>(HandbookType.Languages)
                .AddKeyedScoped<IHandbookQueryService, HandbookQueryService<AgeGroup>>(HandbookType.AgeGroups)
                .AddKeyedScoped<IHandbookQueryService, HandbookQueryService<ProblemArea>>(HandbookType.ProblemAreas);
        }
    }
}