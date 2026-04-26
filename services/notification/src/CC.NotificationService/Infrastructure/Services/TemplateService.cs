//using CC.NotificationService.Domain;
//using CC.NotificationService.Domain.Dto;
//using CC.NotificationService.Domain.interfaces;

//namespace CC.NotificationService.Infrastructure.Services;

//public class TemplateService : ITemplateService
//{
//    private readonly ITemplateRepository _templateRepository;
//    public TemplateService(ITemplateRepository templateRepository)
//    {
//         _templateRepository = templateRepository;
//    }

//    public async Task<Template> CreateAsync(TemplateDto templateDto)
//    {
//        var template = new Template
//        {
//            Id = Guid.NewGuid(),
//            IsActive = templateDto.isActive,
//            Key = templateDto.key,
//        };
//        await _templateRepository.CreateAsync(template);
//        return template;
//    }

//    public async Task<TemplateDto> DeleteByIdAsync(Guid Id)
//    {
//        if (Id == Guid.Empty)
//            throw new ArgumentException("Id cannot be empty.", nameof(Id));
        
//        var template = await _templateRepository.GetByIdAsync(Id);

//        if (template == null)
//            return null;
        
//        var dto = new TemplateDto
//        {
//            key = template.Key,
//            isActive = template.IsActive
//        };
//        await _templateRepository.DeleteByIdAsync(Id);

//        return dto;
//    }

//    //public async Task<List<TemplateDto>> GetAllAsync()
//    //{
//    //  var template = await _templateRepository.GetAllAsync();

//    //  if (template == null)
//    //        return new List<TemplateDto>();

//    //    var templateDto = template.Select(t =>
//    //    new TemplateDto
//    //    {
//    //        key = t.Key,
//    //        isActive=t.IsActive,
//    //    }).ToList();
        
//    //    return templateDto;
//    //}

//    public Task<TemplateDto> GetByIdAsync(Guid Id)
//    {
//        throw new NotImplementedException();
//    }

//    public void UpdateAsync(TemplateDto templateDto)
//    {
//        throw new NotImplementedException();
//    }
//}