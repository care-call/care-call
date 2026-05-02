using System.Diagnostics.CodeAnalysis;
using CC.Gateway.Yarp;

namespace CC.Gateway.OpenApi;

public class OpenApiProvider
{
    private readonly YarpClusterInfoProvider _clusterInfoProvider;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Dictionary<string, OpenApiDocumentInfo> _documents;

    public OpenApiProvider(
        YarpClusterInfoProvider clusterInfoProvider,
        IHttpClientFactory httpClientFactory)
    {
        _clusterInfoProvider = clusterInfoProvider;
        _httpClientFactory = httpClientFactory;

        _documents = CreateDocuments().ToDictionary(x => x.DocumentName);
    }
    
    public OpenApiDocumentInfo? GetDocument(string documentName)
        => _documents.GetValueOrDefault(documentName);

    public IEnumerable<OpenApiDocumentInfo>? GetDocuments()
        => _documents.Values;
    
    public Task<string> GetDocumentAsJsonAsync(string clusterId, string route)
    {
        var client = _httpClientFactory.CreateClient();
        
        var address = _clusterInfoProvider.GetAddress(clusterId);
        var url = new Uri(new Uri(address, UriKind.Absolute), route).ToString();
        
        return client.GetStringAsync(url);
    }
    
    private IEnumerable<OpenApiDocumentInfo> CreateDocuments()
        => _clusterInfoProvider
            .GetClusterIds()
            .SelectMany(id => _clusterInfoProvider.GetOpenApiRoutes(id)
                .Select(route => new OpenApiDocumentInfo(
                    DocumentName: _clusterInfoProvider.GetOpenApiName(id),
                    ClusterId: id,
                    Route: route)));
}