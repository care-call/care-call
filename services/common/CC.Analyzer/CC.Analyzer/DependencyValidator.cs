using System;
using System.Linq;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CC.Analyzer;

public static class DependencyValidator
{
    private const string DiagnosticId = "CCA0001";

    public static bool Validate(
        string? sourceNamespace,
        string? targetNamespace,
        AnalyzerConfigOptions options)
    {
        if (string.IsNullOrWhiteSpace(sourceNamespace) || string.IsNullOrWhiteSpace(targetNamespace))
            return false;

        var target = targetNamespace!;
        var normalizedTarget = $".{target}.";

        return sourceNamespace!
            .Split(['.'], StringSplitOptions.RemoveEmptyEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(sourceKey =>
            {
                options.TryGetValue($"dotnet_code_quality.{DiagnosticId}.{sourceKey}", out var rawRules);
                return rawRules;
            })
            .Where(rawRules => !string.IsNullOrWhiteSpace(rawRules))
            .SelectMany(rawRules => rawRules!.Split([','], StringSplitOptions.RemoveEmptyEntries))
            .Select(rule => rule.Trim())
            .Any(rule =>
                rule.Length > 0 &&
                (rule.IndexOf('.') >= 0
                    ? target.Equals(rule, StringComparison.OrdinalIgnoreCase) ||
                      target.StartsWith($"{rule}.", StringComparison.OrdinalIgnoreCase)
                    : normalizedTarget.IndexOf($".{rule}.", StringComparison.OrdinalIgnoreCase) >= 0));
    }
}
