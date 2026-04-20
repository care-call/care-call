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
    

    public IEnumerable<string> GetClusterIds()
        => proxyStateLookup
            .GetClusters()
            .Select(cluster => cluster.ClusterId);
}