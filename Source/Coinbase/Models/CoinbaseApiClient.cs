using Flurl.Http;
using Flurl.Http.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Models
{

    public interface ICoinbaseApiClient : ICoinbaseClient
    {
    }

    public class CoinbaseApiClient : CoinbaseApiBase<ApiKeyConfig>, ICoinbaseApiClient
    {
        public CoinbaseApiClient(ApiKeyConfig config) : base(config)
        {
        }

        protected override IFlurlClient ConfigureClient(IFlurlClient client)
        {
            return client.Configure(settings => ApiKeyAuth(settings, this.Config));
        }

        private void ApiKeyAuth(ClientFlurlHttpSettings client, ApiKeyConfig keyConfig)
        {
            async Task SetHeaders(HttpCall http)
            {
                var body = http.RequestBody;
                var method = http.Request.Method.Method.ToUpperInvariant();
                var url = http.Request.RequestUri.PathAndQuery;

                string timestamp;
                if (keyConfig.UseTimeApi)
                {
                    var timeResult = await this.Time.GetCurrentTimeAsync().ConfigureAwait(false);
                    timestamp = timeResult.Data.Epoch.ToString();
                }
                else
                {
                    timestamp = ApiKeyAuthenticator.GetCurrentUnixTimestampSeconds().ToString(CultureInfo.CurrentCulture);
                }

                var signature = ApiKeyAuthenticator.GenerateSignature(timestamp, method, url, body, keyConfig.ApiSecret).ToLower();

                http.FlurlRequest
                   .WithHeader(HeaderNames.AccessKey, keyConfig.ApiKey)
                   .WithHeader(HeaderNames.AccessSign, signature)
                   .WithHeader(HeaderNames.AccessTimestamp, timestamp);
            }

            client.BeforeCallAsync = SetHeaders;
        }
    }

}
