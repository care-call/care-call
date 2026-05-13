using CC.Common.Models;
using CC.Common.Validations;

namespace CC.Common.Pagination;

public static class PageInfoExtensions
{
    public static bool IsInvalid(
        this PageInfo pageInfo,
        out Dictionary<string, string[]> validationErrors)
    {
        validationErrors = DataAnnotationsValidation.ValidateByDataAnnotations(pageInfo);
        return validationErrors.Count > 0;
    }
}