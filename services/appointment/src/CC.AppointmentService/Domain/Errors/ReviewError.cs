using FluentResults;

namespace CC.AppointmentService.Domain.Errors;

public static class ReviewError
{
    public static Error TooLate => 
        new("Вы можете оставить отзыв только в течении двух суток");
}