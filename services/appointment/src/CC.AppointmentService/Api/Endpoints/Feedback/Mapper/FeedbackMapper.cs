using CC.AppointmentService.Api.Contracts.Feedback;
using CC.AppointmentService.Application.UseCases.Feedbacks.Creation;
using Riok.Mapperly.Abstractions;

namespace CC.AppointmentService.Api.Endpoints.Feedback.Mapper;

[Mapper]
public static partial class FeedbackMapper
{
    public static partial CreateFeedback ToUseCase(CreateFeedbackRequest request);
}