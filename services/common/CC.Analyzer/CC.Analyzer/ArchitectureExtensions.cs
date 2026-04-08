using CC.Analyzer.Common;
using Microsoft.CodeAnalysis;

namespace CC.Analyzer;

public static class ArchitectureExtensions
{
    internal static Layer? GetLayer(this ISymbol symbol, LayerNamespaceConfig config) 
    {
        var nsName = symbol switch
        {
            INamespaceSymbol ns => ns.ToDisplayString(),
            _ => symbol.ContainingNamespace?.ToDisplayString()
        };
        return config.GetLayer(nsName);
    }
    
    public static bool IsForbiddenDependency(this Layer source, Layer target) => source switch
    {
        Layer.Domain => target != Layer.Domain,
        Layer.Application => target is Layer.Infrastructure or Layer.Api,
        Layer.Infrastructure => target == Layer.Api,
        _ => false
    };
}
