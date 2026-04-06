using System.Linq;
using CC.Analyzer.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CC.Analyzer;

public static class ArchitectureExtensions
{
    public static Layer? GetLayer(this ISymbol symbol, AnalyzerConfigOptions options) 
    {
        var nsName = symbol switch
        {
            INamespaceSymbol ns => ns.ToDisplayString(),
            _ => symbol.ContainingNamespace?.ToDisplayString()
        };
        return GetLayerFromConfig(nsName, options);
    }

    private static Layer? GetLayerFromConfig(string? nsName, AnalyzerConfigOptions options)
    {
        if (nsName is null or "") return null;

        if (IsMatch(nsName, "domain_ns", options)) return Layer.Domain;
        if (IsMatch(nsName, "app_ns", options)) return Layer.Application;
        if (IsMatch(nsName, "infra_ns", options)) return Layer.Infrastructure;
        if (IsMatch(nsName, "api_ns", options)) return Layer.Api;

        return null;
    }

    private static bool IsMatch(string nsName, string key, AnalyzerConfigOptions options)
    {
        if (!options.TryGetValue($"dotnet_code_quality.CCA0001.{key}", out var configuredValues)) return false;

        if (!configuredValues.Contains(',')) return nsName.Contains(configuredValues.Trim());

        var parts = configuredValues.Split(',');
        return parts.Any(p => nsName.Contains(p.Trim()));
    }
    
    public static bool IsForbiddenDependency(this Layer source, Layer target) => source switch
    {
        Layer.Domain => target != Layer.Domain,
        Layer.Application => target is Layer.Infrastructure or Layer.Api,
        Layer.Infrastructure => target == Layer.Api,
        _ => false
    };
}