using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
// ReSharper disable All
#pragma warning disable 649

namespace ReverseProxy.LoadBalancing
{
    public class LoadBalancer
    {
        private readonly Dictionary<string, int> _hostsRequests;
        private readonly ApiUrls _apiUrls;

        public LoadBalancer(IOptions<ApiUrls> apiUrls)
        {
            _hostsRequests = new Dictionary<string, int>();
            _apiUrls = apiUrls.Value;
            if (_apiUrls.Hosts.Contains(','))
            {
                foreach (var host in _apiUrls.Hosts.Split(','))
                {
                    _hostsRequests.Add(host, 0);
                }
            }
            else
            {
                _hostsRequests.Add(_apiUrls.Hosts, 0);
            }
        }

        public string GetLastInvoked()
        {
            if (_apiUrls.IsRandomlyForwarded)
            {
                return _hostsRequests.ToArray()[new Random().Next(0, _hostsRequests.Count)].Key;
            }

            var (key, _) = _hostsRequests
                .FirstOrDefault(r => r.Value == _hostsRequests
                    .Min(x => x.Value));

            _hostsRequests[key]++;

            return key;
        }
    }
}