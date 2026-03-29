using System.Text.RegularExpressions;
using CC.AppointmentService.Domain.Reviews.ValueObjects;

namespace CC.AppointmentService.Domain.Reviews.Rules;

public static partial class SessionReviewModerationRules
{
    private static readonly string[] ToxicKeywords =
    [
        "idiot",
        "debil",
        "suka",
        "nenavizh",
        "ubyu",
        "tvar",
        "shit",
        "fuck",
        "bitch"
    ];

    [GeneratedRegex(@"[\w\.-]+@[\w\.-]+\.\w+", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex EmailRegex();

    [GeneratedRegex(@"(?:\+?\d[\d\-\s\(\)]{9,}\d)", RegexOptions.Compiled)]
    private static partial Regex PhoneRegex();

    [GeneratedRegex(@"(?:t\.me/|telegram\.me/|@[\w_]{4,})", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex ContactHandleRegex();

    public static bool ShouldBeSentToManualReview(ReviewComment? reviewComment)
    {
        if (!reviewComment.HasValue)
        {
            return false;
        }

        var text = reviewComment.Value.Value.Trim();
        if (text.Length == 0)
        {
            return false;
        }

        if (EmailRegex().IsMatch(text) || PhoneRegex().IsMatch(text) || ContactHandleRegex().IsMatch(text))
        {
            return true;
        }

        var lowered = text.ToLowerInvariant();
        return ToxicKeywords.Any(lowered.Contains);
    }
}
