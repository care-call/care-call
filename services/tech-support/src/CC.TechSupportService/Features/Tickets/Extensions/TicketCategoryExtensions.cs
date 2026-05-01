using CC.TechSupportService.Domain.ValueObjects.Ticket;
using CC.TechSupportService.Features.Tickets.Enums;

namespace CC.TechSupportService.Features.Tickets.Extensions;

public static class TicketCategoryExtensions
{
    extension(TicketCategoryType category)
    {
        public TicketCategory ToTicketCategory() =>
            category switch
            {
                TicketCategoryType.Bug => new Bug(),
                TicketCategoryType.Question => new Question(),
                TicketCategoryType.FeatureRequest => new FeatureRequest(),
                TicketCategoryType.Complaint => new Complaint(),
                TicketCategoryType.AccountIssue => new AccountIssue(),
                _ => throw new ArgumentOutOfRangeException(nameof(category), $@"Не найдена категория: {category}")
            };
    }
}