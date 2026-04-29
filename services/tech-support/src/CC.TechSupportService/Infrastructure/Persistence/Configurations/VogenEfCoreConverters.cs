using Vogen;

namespace CC.TechSupportService.Infrastructure.Persistence.Configurations;

[EfCoreConverter<Domain.ValueObjects.Comment.CommentBody>]
[EfCoreConverter<Domain.ValueObjects.Ticket.CsatComment>]
[EfCoreConverter<Domain.ValueObjects.Ticket.CsatRating>]
[EfCoreConverter<Domain.ValueObjects.Ticket.TicketDescription>]
[EfCoreConverter<Domain.ValueObjects.Ticket.TicketSubject>]
[EfCoreConverter<Domain.ValueObjects.FileDetails.ContentType>]
internal sealed partial  class VogenEfCoreConverters;