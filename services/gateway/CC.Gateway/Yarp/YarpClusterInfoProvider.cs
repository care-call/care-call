using Yarp.ReverseProxy;

namespace CC.Gateway.Yarp;

public class YarpClusterInfoProvider(IProxyStateLookup proxyStateLookup)
{
    public string GetAddress(string clusterId)
    {
        if (!proxyStateLookup.TryGetCluster(clusterId, out var cluster))
            throw new Exception($"Cluster '{clusterId}' not found");

        return cluster.Destinations.First().Value.Model.Config.Address;
    }

    public bool ClusterExist(string clusterId)
        => proxyStateLookup.TryGetCluster(clusterId, out _);

    public string GetOpenApiName(string clusterId)
    {
        if (!proxyStateLookup.TryGetCluster(clusterId, out var cluster))
            throw new Exception($"Cluster '{clusterId}' not found");
        
        return cluster.Model.Config.Metadata?["OpenApiName"] 
               ?? throw new Exception($"'{clusterId}' metadata not found");
    }
    
    public IEnumerable<string> GetOpenApiRoutes(string clusterId)
    {
        if (!proxyStateLookup.TryGetCluster(clusterId, out var cluster))
            throw new Exception($"Cluster '{clusterId}' not found");
        
        var routesString = cluster.Model.Config.Metadata?["OpenApiRoutes"];
        return routesString?.Split(',', StringSplitOptions.TrimEntries) 
               ?? throw new Exception($"'{clusterId}' openapi routes not found");;
    }
    
    public IEnumerable<string> GetClusterIds()
        => proxyStateLookup
            .GetClusters()
            .Select(cluster => cluster.ClusterId);
}