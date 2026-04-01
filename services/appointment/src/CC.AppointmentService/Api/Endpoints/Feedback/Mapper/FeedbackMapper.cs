using CC.AppointmentService.Api.Contracts.Feedback;
using CC.AppointmentService.Application.UseCases.Feedback.Creation;
using Riok.Mapperly.Abstractions;

namespace CC.AppointmentService.Api.Endpoints.Feedback.Mapper;
[Mapper]
public static partial class FeedbackMapper
{
    public static partial CreateReview ToUseCase(CreateFeedbackRequest request);
}