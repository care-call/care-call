using CC.Shared.Domain;
using CC.TechSupportService.Domain.Entities;
using CC.TechSupportService.Domain.ValueObjects.Ticket;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.TechSupportService.Infrastructure.Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(tc => tc.Id);

        builder.Property(tc => tc.Id)
            .HasConversion(id => id.Value, value => GuidId.From(value));

        builder.HasAlternateKey(tc => tc.Number);
        
        builder.Property(tc => tc.Number)
            .IsRequired();

        builder.ComplexProperty(tc => tc.Reporter, tc =>
        {
            tc.Property(rpi => rpi.ReporterId)
                .IsRequired();

            tc.Property(rpi => rpi.ReporterType)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();
        });

        builder.Property(tc => tc.AssigneeId)
            .HasConversion(id => id!.Value, value => GuidId.From(value))
            .IsRequired(false);

        builder.Property(tc => tc.Subject)
            .HasConversion(sb => sb.Subject, value => TicketSubject.TryCreate(value).Value)
            .HasMaxLength(TicketSubject.MaxSubjectLenght)
            .IsRequired();

        builder.Property(tc => tc.Description)
            .HasConversion(sb => sb.Description, value => TicketDescription.TryCreate(value).Value)
            .HasMaxLength(TicketDescription.MaxDescriptionLenght)
            .IsRequired();

        builder.Property(tc => tc.TicketCategory)
            .HasConversion(x => TicketCategoryToString(x),
                x => StringToTicketCategory(x))
            .HasMaxLength(32)
            .IsRequired(); // maybe will work))))

        builder.Property(tc => tc.Status)
            .HasConversion(x => TicketStatusToString(x), x => StringToTicketStatus(x))
            .IsRequired();

        builder.ComplexProperty(x => x.RelatedEntity, x =>
        {
            x.Property(re => re.RelatedEntityId)
                .IsRequired();

            x.Property(rt => rt.RelatedType)
                .HasConversion<string>()
                .IsRequired();
        });

        builder.Property(fr => fr.FirstRespondedAt)
            .IsRequired(false);

        builder.Property(sfr => sfr.SlaFirstResponseAt)
            .IsRequired();
        
        builder.Property(lu => lu.LastUserRespondedAt)
            .IsRequired(false);

        builder.Property(ra => ra.ResolvedAt)
            .IsRequired(false);
        
        builder.Property(ra => ra.ClosedAt)
            .IsRequired(false);
        
        builder.Property(rt => rt.Rating)
            .HasConversion(rt => rt.CsatRating, x => Rating.TryCreate(x).Value)
            .IsRequired(false);
        
        builder.Property(rt => rt.Comment)
            .HasConversion(rt => rt.CsatComment, 
                x => Domain.ValueObjects.Ticket.TicketComment.TryCreate(x).Value)
            .IsRequired(false);

        builder.Property(cr => cr.CreatedAt)
            .IsRequired();

        builder.Property(ut => ut.UpdatedAt)
            .IsRequired();
    }

    private static string TicketCategoryToString(TicketCategory category) 
        => category.GetType().Name;

    private static TicketCategory StringToTicketCategory(string value) => value switch
    {
        nameof(Bug) => new Bug(),
        nameof(Question) => new Question(),
        nameof(FeatureRequest) => new FeatureRequest(),
        nameof(Complaint) => new Complaint(),
        nameof(AccountIssue) => new AccountIssue(),
        _ => throw new ArgumentException("Invalid input!")
    };

    private static string TicketStatusToString(TicketStatus status)
        => status.GetType().Name;

    private static TicketStatus StringToTicketStatus(string value) => value switch
    {
        nameof(New) => new New(),
        nameof(Opened) => new Opened(),
        nameof(InProgress) => new InProgress(),
        nameof(WaitingForUser) => new WaitingForUser(),
        nameof(Escalated) => new Escalated(),
        nameof(Resolved) => new Resolved(),
        nameof(Reopened) => new Reopened(),
        nameof(Closed) => new Closed(),
        _ => throw new ArgumentException("Invalid input!")
    };
}