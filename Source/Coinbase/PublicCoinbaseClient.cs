using Coinbase.Endpoints;
using Coinbase.Models;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase
{

    public interface IPublicCoinbaseClient
    {
        ITimeEndpoint Time { get; }
        IPricesEndpoint Prices { get; }
        IExchangeRatesEndpoint ExchangeRates { get; }
        ICurrencyEndpoint Currencies { get; }
    }

    public class PublicCoinbaseClient : IPublicCoinbaseClient
    {
        protected IFlurlClient _client;

        protected Config _config;

        public ICurrencyEndpoint Currencies { get; internal protected set; }
        public IExchangeRatesEndpoint ExchangeRates { get; internal protected set; }
        public IPricesEndpoint Prices { get; internal protected set; }
        public ITimeEndpoint Time { get; internal protected set; }


        public PublicCoinbaseClient(Config config = null)
        {
            this._config = config ?? new Config();
            this._client = this.CreateClient();
            this.InitializeEndpoints();
        }

        protected virtual void InitializeEndpoints()
        {
            this.Currencies = new CurrencyEndpoint(this.CreateOptions("currencies"));
            this.ExchangeRates = new ExchangeRatesEndpoint(this.CreateOptions("exchange-rates"));
            this.Prices = new PricesEndpoint(this.CreateOptions("prices"));
            this.Time = new TimeEndpoint(this.CreateOptions("time"));
        }

        protected EndpointOptions CreateOptions(string subRoute = null)
        {
            var url = new Url(this._config.ApiUrl);
            if (!string.IsNullOrEmpty(subRoute))
            {
                url = url.AppendPathSegment(subRoute);
            }
            return new EndpointOptions(this._client, url);
        }

        protected virtual IFlurlClient ConfigureClient(IFlurlClient client)
        {
            return client;
        }

        protected virtual IFlurlClient CreateClient()
        {
            var client = new FlurlClient();

            client.WithHeader(HeaderNames.Version, CoinbaseConsts.ApiVersionDate)
               .WithHeader("User-Agent", CoinbaseConsts.UserAgent);

            return this.ConfigureClient(client);
        }
    }
}
