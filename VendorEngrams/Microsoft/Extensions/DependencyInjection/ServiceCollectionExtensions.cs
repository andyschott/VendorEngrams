using System;
using VendorEngrams;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        private const string DefaultBaseUrl = "https://api.vendorengrams.xyz/";

        public static void AddVendorEngramsClient(this IServiceCollection services, string source, string baseUrl = null)
        {
            if(string.IsNullOrEmpty(source))
            {
                throw new ArgumentException("source is required", nameof(source));
            }

            baseUrl = baseUrl ?? DefaultBaseUrl;
            services.Configure<VendorEngramsOptions>(options =>
            {
                options.Source = source;
            });

            services.AddHttpClient<IVendorEngramsClient, VendorEngramsClient>(client =>
            {
                client.BaseAddress = new System.Uri(baseUrl ?? DefaultBaseUrl);
            });
        }
    }
}