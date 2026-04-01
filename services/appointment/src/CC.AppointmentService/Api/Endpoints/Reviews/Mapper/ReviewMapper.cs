using CC.AppointmentService.Api.Contracts.Reviews;
using CC.AppointmentService.Application.UseCases.Reviews.Creation;
using Riok.Mapperly.Abstractions;

namespace CC.AppointmentService.Api.Endpoints.Reviews.Mapper;
[Mapper]
public static partial class ReviewMapper
{
    public static partial CreateReview ToUseCase(CreateReviewRequest request);
}