using CC.Common.Errors;
using FluentResults;

namespace CC.AppointmentService.Domain.Errors;

public static class ReviewError
{
    public static Error TooLate => 
        new Error("Вы можете оставить отзыв только в течении двух суток")
            .WithErrorCode("R101");
}