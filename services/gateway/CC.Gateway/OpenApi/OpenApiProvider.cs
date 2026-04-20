using System.Diagnostics.CodeAnalysis;
using CC.Gateway.Yarp;

namespace CC.Gateway.OpenApi;

public class OpenApiProvider(
    YarpClusterInfoProvider clusterInfoProvider,
    IHttpClientFactory httpClientFactory)
{
    private const string _defaultOpenApiRoute = "openapi/v1.json";
    
    public OpenApiDocumentInfo? GetDocument(string documentName, string version = "v1")
    {
        if (!TryGetClusterId(documentName, out string? clusterId))
            return null;
        
        return new OpenApiDocumentInfo(documentName, version, clusterId);
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

    private bool TryGetClusterId(string documentName, [NotNullWhen(true)] out string? clusterId)
    {
        clusterId = $"{documentName}-cluster";
        if (clusterInfoProvider.ClusterExist(clusterId))
            return true;

        clusterId = null;
        return false;
    }
}