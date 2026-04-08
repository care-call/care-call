using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Xunit;

namespace CC.Analyzer.Tests;

public class LayerTests
{
    [Fact]
    public async Task Domain_Cannot_Depend_On_Infrastructure()
    {
        var source = @"
namespace Domain.Models
{
    using Infrastructure.Services;

    public class MyModel
    {
        private readonly SomeService _service;
    }
}

namespace Infrastructure.Services
{
    public class SomeService { }
}
";

        var test = new CSharpAnalyzerTest<CleanArchitectureDependenciesAnalyzer, DefaultVerifier>
        {
            TestCode = source
        };

        // Задаем настройки слоёв для теста
        test.TestState.AnalyzerConfigFiles.Add(("/.editorconfig", @"
root = true
[*.cs]
dotnet_code_quality.CCA0001.domain_ns = Domain
dotnet_code_quality.CCA0001.infra_ns = Infrastructure
"));

        var expectedError = new DiagnosticResult("CCA0001", DiagnosticSeverity.Error);
        
        test.ExpectedDiagnostics.Add(expectedError.WithLocation(4, 11)); // На 'Infrastructure'
        test.ExpectedDiagnostics.Add(expectedError.WithLocation(4, 26)); // На 'Services'
        test.ExpectedDiagnostics.Add(expectedError.WithLocation(8, 26));

        await test.RunAsync();
    }
    [Fact]
    public async Task Infrastructure_Does_Not_Violate_When_Using_ThirdParty_Namespace_Containing_Layer_Name_As_Substring()
    {
        var source = @"
namespace Infrastructure.OpenApi
{
    using Microsoft.OpenApi;

    public class MyConfigurator
    {
        public Info GetInfo() => new Info();
    }
}

namespace Microsoft.OpenApi
{
    public class Info { }
}
";

        var test = new CSharpAnalyzerTest<CleanArchitectureDependenciesAnalyzer, DefaultVerifier>
        {
            TestCode = source
        };

        test.TestState.AnalyzerConfigFiles.Add(("/.editorconfig", @"
root = true
[*.cs]
dotnet_code_quality.CCA0001.infra_ns = Infrastructure
dotnet_code_quality.CCA0001.api_ns = Api
"));

        // Диагностик быть не должно — Microsoft.OpenApi не является слоем Api
        await test.RunAsync();
    }
}
