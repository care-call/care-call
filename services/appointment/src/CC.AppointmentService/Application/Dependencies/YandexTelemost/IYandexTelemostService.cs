namespace CC.AppointmentService.Application.Dependencies.YandexTelemost;

public interface IYandexTelemostService
{
    public Task<CreateCallLingResponse> CreateCallLinkAsync();
}