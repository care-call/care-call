using FluentResults;
using Shouldly;

namespace CC.AppointmentTests;

public static class ResultAssertions
{
    public static void ShouldHaveErrorCode(this Result result, string code)
    {
        result.Errors
            .ShouldContain(e =>
                    e.Metadata.GetValueOrDefault("ErrorCode") != null &&
                    e.Metadata.GetValueOrDefault("ErrorCode")!.ToString() == code,
                $"Expected error with code '{code}', but got: [{string.Join(", ", result.Errors.Select(e => e.Message))}]");
    }

    public static void ShouldHaveErrorCode<T>(this Result<T> result, string code)
    {
        result.Errors
            .ShouldContain(e =>
                    e.Metadata.GetValueOrDefault("ErrorCode") != null &&
                    e.Metadata.GetValueOrDefault("ErrorCode")!.ToString() == code,
                $"Expected error with code '{code}', but got: [{string.Join(", ", result.Errors.Select(e => e.Message))}]");
    }
}