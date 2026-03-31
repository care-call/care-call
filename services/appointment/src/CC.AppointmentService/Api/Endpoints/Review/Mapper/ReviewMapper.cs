using CC.AppointmentService.Api.Contracts.Review;
using CC.AppointmentService.Application.UseCases.Reviews.Creation;
using Riok.Mapperly.Abstractions;

namespace CC.AppointmentService.Api.Endpoints.Review.Mapper;
[Mapper]
public static partial class ReviewMapper
{
    public static partial CreateReview ToUseCase(CreateReviewRequest request);
}