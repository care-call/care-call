using CC.Gateway.Yarp;

namespace CC.Gateway.OpenApi;

public class OpenApiProvider(
    YarpClusterInfoProvider clusterInfoProvider,
    IHttpClientFactory httpClientFactory)
{
    private const string _defaultOpenApiRoute = "openapi/v1.json";
    
    public OpenApiDocumentInfo? GetDocument(string documentName, string version = "v1")
    {
        foreach (var clusterId in clusterInfoProvider.GetClusterIds())
        {
            var currDocName = GetDocumentName(clusterId);
            
            if (currDocName.Equals(documentName, StringComparison.OrdinalIgnoreCase))
                return new OpenApiDocumentInfo(currDocName, version, clusterId);
        }
        
        return null;
    }
    
    public IEnumerable<OpenApiDocumentInfo> GetDocuments()
        => clusterInfoProvider
            .GetClusterIds()
            .Select(id => new OpenApiDocumentInfo(
                DocumentName: GetDocumentName(id),
                Version: "v1",
                ClusterId: id));
    
    public async Task<string> GetDocumentAsJsonAsync(string clusterId)
    {
        var client = httpClientFactory.CreateClient();

        var address = clusterInfoProvider.GetAddress(clusterId);
        var url = new Uri(new Uri(address, UriKind.Absolute), _defaultOpenApiRoute).ToString();
        
        return await client.GetStringAsync(url);
    }
    
    private static string GetDocumentName(string clusterId)
        => clusterId[..clusterId.LastIndexOf('-')];
}