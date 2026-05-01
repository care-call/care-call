using CC.TechSupportService.Domain.ValueObjects.Ticket;
using CC.TechSupportService.Features.Tickets.Enums;

namespace CC.TechSupportService.Features.Tickets.Extensions;

public static class TicketCategoryExtensions
{
    extension(TicketCategoryType type)
    {
        public TicketCategory ToTicketCategory()
        {
            return type switch
            {
                TicketCategoryType.Bug => new Bug(),
                TicketCategoryType.Question => new Question(),
                TicketCategoryType.FeatureRequest => new FeatureRequest(),
                TicketCategoryType.Complaint => new Complaint(),
                TicketCategoryType.AccountIssue => new AccountIssue(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), $"Не найдена категория: {type}")
            };
        }
    }
}