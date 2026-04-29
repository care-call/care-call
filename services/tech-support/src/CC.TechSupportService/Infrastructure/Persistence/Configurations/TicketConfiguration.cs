using CC.TechSupportService.Domain.Entities;
using CC.TechSupportService.Domain.ValueObjects.Ticket;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.TechSupportService.Infrastructure.Persistence.Configurations;

public sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(tc => tc.Id);
        
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
            .IsRequired(false);

        builder.Property(tc => tc.Subject)
            .HasConversion(new VogenEfCoreConverters.TicketSubjectEfCoreValueConverter())
            .HasMaxLength(TicketSubject.MaxSubjectLenght)
            .IsRequired();

        builder.Property(tc => tc.Description)
            .HasConversion(new VogenEfCoreConverters.TicketDescriptionEfCoreValueConverter())
            .HasMaxLength(TicketDescription.MaxDescriptionLenght)
            .IsRequired();

        builder.Property(tc => tc.TicketCategory)
            .HasConversion(x => TicketCategoryToInt(x),
                x => IntToTicketCategory(x))
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(tc => tc.Status)
            .HasConversion(x => TicketStatusToInt(x), x => IntToTicketStatus(x))
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
            .HasColumnType("timestamp without time zone")
            .IsRequired(false);

        builder.Property(sfr => sfr.SlaFirstResponseAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired();
        
        builder.Property(lu => lu.LastUserRespondedAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired(false);

        builder.Property(ra => ra.ResolvedAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired(false);
        
        builder.Property(ra => ra.ClosedAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired(false);
        
        builder.Property(rt => rt.CsatRating)
            .HasConversion(new VogenEfCoreConverters.CsatRatingEfCoreValueConverter())
            .IsRequired(false);
        
        builder.Property(rt => rt.CsatComment)
            .HasConversion(new VogenEfCoreConverters.CsatCommentEfCoreValueConverter())
            .IsRequired(false);

        builder.Property(cr => cr.CreatedAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        builder.Property(ut => ut.UpdatedAt)
            .HasColumnType("timestamp without time zone")
            .IsRequired();
    }

    private static int TicketCategoryToInt(TicketCategory category) => category switch
    {
        Bug => 1,
        Question => 2,
        FeatureRequest => 3,
        Complaint => 4,
        AccountIssue => 5,
        _ => throw new ArgumentException("Invalid category!")
    };

    private static TicketCategory IntToTicketCategory(int value) => value switch
    {
        1 => new Bug(),
        2 => new Question(),
        3 => new FeatureRequest(),
        4 => new Complaint(),
        5 => new AccountIssue(),
        _ => throw new ArgumentException("Invalid input!")
    };

    private static int TicketStatusToInt(TicketStatus status) => status switch
    {
        New => 1,
        Opened => 2,
        InProgress => 3,
        WaitingForUser => 4,
        Escalated => 5,
        Resolved => 6,
        Reopened => 7,
        Closed => 8,
        _ => throw new ArgumentException("Invalid status type!")
    };

    private static TicketStatus IntToTicketStatus(int value) => value switch
    {
        1 => new New(),
        2 => new Opened(),
        3 => new InProgress(),
        4 => new WaitingForUser(),
        5 => new Escalated(),
        6 => new Resolved(),
        7 => new Reopened(),
        8 => new Closed(),
        _ => throw new ArgumentException("Invalid input!")
    };
}