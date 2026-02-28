using FluentResults;

namespace CC.Common.Errors;

public static class ErrorExtensions
{
    extension(Error error)
    {
        public Error WithErrorCode(string errorCode) => error.WithMetadata("ErrorCode", errorCode);
    }
}