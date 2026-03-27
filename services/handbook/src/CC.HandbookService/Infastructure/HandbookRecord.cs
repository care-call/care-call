using CC.HandbookService.Application.Import;

namespace CC.HandbookService.Infastructure;

public class HandbookRecord
{
    public long Id { get; set; }
    public HandbookType HandbookType { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
