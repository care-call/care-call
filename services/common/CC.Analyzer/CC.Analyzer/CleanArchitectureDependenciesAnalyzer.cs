using System.Collections.Immutable;
using System.Collections.Concurrent;
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
       "Слой '{0}' не может использовать '{1}'",
       "Чистая архитектура",
       DiagnosticSeverity.Error,
       isEnabledByDefault: true);

   
   public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule]; 
   
   public override void Initialize(AnalysisContext context)
   {
       context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
       context.EnableConcurrentExecution();

       context.RegisterCompilationStartAction(startContext =>
       {
           var configByTree = new ConcurrentDictionary<SyntaxTree, LayerNamespaceConfig>();
           startContext.RegisterSyntaxNodeAction(
               syntaxContext => AnalyzeNode(syntaxContext, configByTree),
               SyntaxKind.IdentifierName);
       });
    }

   private static void AnalyzeNode(
       SyntaxNodeAnalysisContext context,
       ConcurrentDictionary<SyntaxTree, LayerNamespaceConfig> configByTree)
   {
       var identifier = (IdentifierNameSyntax)context.Node;
       
       if (identifier.Parent is BaseNamespaceDeclarationSyntax) return;

       var config = configByTree.GetOrAdd(context.Node.SyntaxTree, syntaxTree =>
       {
           var options = context.Options.AnalyzerConfigOptionsProvider.GetOptions(syntaxTree);
           return LayerNamespaceConfig.From(options);
       });

       var enclosingSymbol = context.ContainingSymbol;

       var sourceLayer = enclosingSymbol?.GetLayer(config);
       if (!sourceLayer.HasValue) return;

       var symbol = context.SemanticModel.GetSymbolInfo(identifier, context.CancellationToken).Symbol;

       var targetLayer = symbol?.GetLayer(config);
       if (targetLayer.HasValue && sourceLayer.Value.IsForbiddenDependency(targetLayer.Value))
       {
           context.ReportDiagnostic(Diagnostic.Create(
               Rule, 
               identifier.GetLocation(), 
               sourceLayer.Value.ToString(), 
               targetLayer.Value.ToString()));
       }
   }
}
