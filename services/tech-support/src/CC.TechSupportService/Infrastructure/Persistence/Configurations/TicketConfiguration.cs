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
            .HasConversion(new VogenEfCoreConverters.GuidIdEfCoreValueConverter());

        builder.HasMany(x => x.Attachments)
            .WithOne()
            .HasForeignKey(x => x.TicketId)
            .IsRequired();
        
        builder.HasMany(x => x.Comments)
            .WithOne()
            .HasForeignKey(x => x.TicketId)
            .IsRequired();
        
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
            .HasConversion(new VogenEfCoreConverters.GuidIdEfCoreValueConverter())
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
            .HasConversion(x => TicketCategoryToString(x),
                x => StringToTicketCategory(x))
            .HasMaxLength(32)
            .IsRequired();

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
        
        builder.Property(rt => rt.CsatRating)
            .HasConversion(new VogenEfCoreConverters.CsatRatingEfCoreValueConverter())
            .IsRequired(false);
        
        builder.Property(rt => rt.CsatComment)
            .HasConversion(new VogenEfCoreConverters.CsatCommentEfCoreValueConverter())
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