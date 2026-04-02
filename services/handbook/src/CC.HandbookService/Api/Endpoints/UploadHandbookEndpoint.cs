using MediatR;

namespace CC.HandbookService.Api.Endpoints;

public static class UploadCsvFileEndpoint
{
    public static async Task<IResult> Handle(string handbook, IFormFile file, IMediator mediator)
    {
        var result = await mediator.Send(new CancelAppointment()
        {
            AppointmentId =  request.AppointmentId,
            ClientId = request.ClientId,
            Reason =  request.Reason,
        });
        return Results.Ok(result);
    }
}