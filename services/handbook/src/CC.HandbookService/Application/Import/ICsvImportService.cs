namespace CC.HandbookService.Application.Import;

public interface ICsvImportService
{
    Task<ResultImportFile> ImportAsyncFile(Stream file, HandbookType handbookType, CancellationToken token = default);
}