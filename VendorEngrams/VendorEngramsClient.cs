using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace VendorEngrams
{
    public class VendorEngramsClient : IVendorEngramsClient
    {
        private readonly HttpClient _client;
        private readonly VendorEngramsOptions _options;
        private readonly ILogger _logger;

        public VendorEngramsClient(HttpClient client, IOptions<VendorEngramsOptions> options, ILogger<VendorEngramsClient> logger)
        {
            _client = client;
            _logger = logger;
            _options = options.Value;
        }
        
        public async Task<IEnumerable<Vendor>> GetVendorDrops(bool visibleOnly = true)
        {
            var vendors = await Get<IEnumerable<Vendor>>($"getVendorDrops?source={_options.Source}");

            if(visibleOnly)
            {
                vendors = vendors.Where(vendor => vendor.Display);
            }

            return vendors;
        }

        private async Task<T> Get<T>(string uri)
        {
            _logger.LogInformation($"GET {_client.BaseAddress}{uri}");
            try
            {
                var response = await _client.GetAsync(uri);
                var json  = await response.Content.ReadAsStringAsync();

                if(!response.IsSuccessStatusCode)
                {
                    var msg = $"GET {_client.BaseAddress}{uri} returned {(int)response.StatusCode} - {response.StatusCode}. Content = '{json}'";
                    _logger.LogError(msg);
                    throw new Exception(msg);
                }

                return JsonSerializer.Deserialize<T>(json);
            }
            catch(HttpRequestException ex)
            {
                var msg = $"GET {_client.BaseAddress}{uri} threw HttpRequestException: {ex.Message}";
                _logger.LogError(msg);
                throw;
            }
            catch(JsonException ex)
            {
                var msg = $"Error deserializing result of {_client.BaseAddress}{uri}: {ex.Message}";
                _logger.LogError(msg);
                throw;
            }
        }
    }
}
