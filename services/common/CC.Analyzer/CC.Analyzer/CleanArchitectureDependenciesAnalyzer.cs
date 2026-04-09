using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CC.Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class CleanArchitectureDependenciesAnalyzer : DiagnosticAnalyzer
{
    private const string DiagnosticId = "CCA0001";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        "Запрещенная зависимость",
        "Namespace '{0}' не должен зависеть от '{1}'",
        "Чистая архитектура",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.IdentifierName);
    }

    private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not IdentifierNameSyntax identifier || identifier.Parent is BaseNamespaceDeclarationSyntax)
            return;

        var symbol = context.SemanticModel.GetSymbolInfo(identifier, context.CancellationToken).Symbol;
        if (symbol is null)
            return;
        

        var sourceNamespace = context.ContainingSymbol?.ContainingNamespace?.ToDisplayString();
        var targetNamespace = symbol is INamespaceSymbol ns ? ns.ToDisplayString() : symbol.ContainingNamespace?.ToDisplayString();
        var options = context.Options.AnalyzerConfigOptionsProvider.GetOptions(context.Node.SyntaxTree);

        if (!DependencyValidator.Validate(sourceNamespace, targetNamespace, options))
            return;

        context.ReportDiagnostic(Diagnostic.Create(
            Rule,
            identifier.GetLocation(),
            sourceNamespace,
            targetNamespace));
    }
}
