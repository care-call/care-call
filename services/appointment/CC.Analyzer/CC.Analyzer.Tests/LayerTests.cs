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
}
