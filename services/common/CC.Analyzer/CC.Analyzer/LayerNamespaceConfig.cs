using System;
using System.Collections.Generic;
using CC.Analyzer.Common;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CC.Analyzer;

internal sealed class LayerNamespaceConfig
{
    private readonly LayerMatchRule[] _rules;

    private LayerNamespaceConfig(LayerMatchRule[] rules)
    {
        _rules = rules;
    }

    public static LayerNamespaceConfig From(AnalyzerConfigOptions options)
    {
        return new LayerNamespaceConfig(
        [
            CreateRule(options, Layer.Domain, "domain_ns"),
            CreateRule(options, Layer.Application, "app_ns"),
            CreateRule(options, Layer.Infrastructure, "infra_ns"),
            CreateRule(options, Layer.Api, "api_ns")
        ]);
    }

    public Layer? GetLayer(string? namespaceName)
    {
        if (string.IsNullOrWhiteSpace(namespaceName))
        {
            return null;
        }

        var candidateNamespace = namespaceName!;
        var segments = candidateNamespace.Split('.');

        foreach (var rule in _rules)
        {
            if (rule.IsMatch(candidateNamespace, segments))
            {
                return rule.Layer;
            }
        }

        return null;
    }

    private static LayerMatchRule CreateRule(AnalyzerConfigOptions options, Layer layer, string key)
    {
        if (!options.TryGetValue($"dotnet_code_quality.CCA0001.{key}", out var raw) ||
            string.IsNullOrWhiteSpace(raw))
        {
            return new LayerMatchRule(layer, [], new HashSet<string>(StringComparer.Ordinal));
        }

        var prefixTokens = new List<string>();
        var segmentTokens = new HashSet<string>(StringComparer.Ordinal);

        var rawTokens = raw.Split([','], StringSplitOptions.RemoveEmptyEntries);
        foreach (var rawToken in rawTokens)
        {
            var token = rawToken.Trim();
            if (token.Length == 0)
            {
                continue;
            }

            if (token.IndexOf('.') >= 0)
            {
                prefixTokens.Add(token);
            }
            else
            {
                segmentTokens.Add(token);
            }
        }

        return new LayerMatchRule(layer, prefixTokens.ToArray(), segmentTokens);
    }

    private sealed class LayerMatchRule
    {
        public LayerMatchRule(Layer layer, string[] prefixTokens, HashSet<string> segmentTokens)
        {
            Layer = layer;
            PrefixTokens = prefixTokens;
            SegmentTokens = segmentTokens;
        }

        public Layer Layer { get; }

        private string[] PrefixTokens { get; }

        private HashSet<string> SegmentTokens { get; }

        public bool IsMatch(string namespaceName, string[] namespaceSegments)
        {
            foreach (var token in PrefixTokens)
            {
                if (namespaceName.Equals(token, StringComparison.Ordinal) ||
                    namespaceName.StartsWith(token + ".", StringComparison.Ordinal))
                {
                    return true;
                }
            }

            if (SegmentTokens.Count == 0)
            {
                return false;
            }

            foreach (var segment in namespaceSegments)
            {
                if (SegmentTokens.Contains(segment))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
