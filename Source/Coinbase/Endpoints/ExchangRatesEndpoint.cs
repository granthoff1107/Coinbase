using System;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Models;
using Flurl.Http;

namespace Coinbase
{
    public interface IExchangeRatesEndpoint
    {
        /// <summary>
        /// Get current exchange rates. Default base currency is USD but it can be defined as any supported currency. Returned rates will define the exchange rate for one unit of the base currency.
        /// </summary>
        /// <param name="currency">Base currency (default: USD)</param>
        Task<Response<ExchangeRates>> GetExchangeRatesAsync(string currency = null, CancellationToken cancellationToken = default);

    }

    public class ExchangeRatesEndpoint : EndpointBase, IExchangeRatesEndpoint
    {
        public ExchangeRatesEndpoint(EndpointOptions options) : base(options)
        {
        }

        /// <summary>
        /// Get current exchange rates. Default base currency is USD but it can be defined as any supported currency. Returned rates will define the exchange rate for one unit of the base currency.
        /// </summary>
        /// <param name="currency">Base currency (default: USD)</param>
        public async Task<Response<ExchangeRates>> GetExchangeRatesAsync(string currency, CancellationToken cancellationToken)
        {
            var req = this._options.CreateBaseRequest();

            if (!(currency is null))
            {
                req.SetQueryParam("currency", currency);
            }

            return await req.GetJsonAsync<Response<ExchangeRates>>(cancellationToken);
        }
    }
}