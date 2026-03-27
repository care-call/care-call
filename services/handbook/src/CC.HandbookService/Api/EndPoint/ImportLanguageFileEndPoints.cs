using CC.HandbookService.Application.Import;

namespace CC.HandbookService.Api.EndPoint;

public static class ImportLanguageFileEndPoints
{
	public static async Task<IResult> Import(IFormFile file, ICsvImportService csvImportService, HandbookType handbookType, CancellationToken token)
	{
		if (file.Length == 0)
		{
			return Results.BadRequest(new { Message = "File is empty" });
		}

		await using var stream = file.OpenReadStream();
		var result = await csvImportService.ImportAsyncFile(stream, handbookType, token);
		return Results.Ok(result);
	}
}