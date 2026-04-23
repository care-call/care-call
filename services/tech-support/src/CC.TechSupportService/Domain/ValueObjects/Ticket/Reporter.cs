using CC.TechSupportService.Domain.Enums;
using FluentResults;

namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

public record Reporter
{
    private Reporter() {  }
    
    private Reporter(Guid reporterId, ReporterType reporterType)
    {
        ReporterId = reporterId;
        ReporterType = reporterType;
    }
    
    public Guid ReporterId { get; private set; }
    
    public ReporterType ReporterType { get; private set; }
    
    public static Result<Reporter> TryCreate(Guid reporterId, ReporterType reporterType)
    {
        if (reporterId == Guid.Empty)
            return Result.Fail("reporterId cannot be empty!");

        return Result.Ok(new Reporter(reporterId, reporterType));
    }
}