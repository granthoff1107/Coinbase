using Coinbase.Models;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Coinbase.Endpoints
{
    public interface IPricesEndpoint
    {
        /// <summary>
        /// Get the total price to buy one bitcoin or ether.
        /// Note that exchange rates fluctuates so the price is only correct for seconds at the time.This buy price includes standard Coinbase fee (1%) but excludes any other fees including bank fees.
        /// </summary>
        /// <param name="currencyPair">Currency pair such as BTC-USD, ETH-USD, etc.</param>
        Task<Response<Money>> GetBuyPriceAsync(string currencyPair, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the total price to sell one bitcoin or ether.
        /// Note that exchange rates fluctuates so the price is only correct for seconds at the time.This sell price includes standard Coinbase fee (1%) but excludes any other fees including bank fees.
        /// </summary>
        /// <param name="currencyPair">Currency pair such as BTC-USD, ETH-USD, etc.</param>
        Task<Response<Money>> GetSellPriceAsync(string currencyPair, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the current market price for bitcoin. This is usually somewhere in between the buy and sell price.
        ///Note that exchange rates fluctuates so the price is only correct for seconds at the time.
        /// </summary>
        /// <param name="currencyPair"></param>
        /// <param name="cancellationToken"></param>
        Task<Response<Money>> GetSpotPriceAsync(string currencyPair, DateTime? date = null, CancellationToken cancellationToken = default);


    }

    public class PricesEndpoint : EndpointBase, IPricesEndpoint
    {
        public PricesEndpoint(EndpointOptions options) : base(options)
        {
        }

        /// <summary>
        /// Get the total price to buy one bitcoin or ether.
        /// Note that exchange rates fluctuates so the price is only correct for seconds at the time.This buy price includes standard Coinbase fee (1%) but excludes any other fees including bank fees.
        /// </summary>
        /// <param name="currencyPair">Currency pair such as BTC-USD, ETH-USD, etc.</param>
        public async Task<Response<Money>> GetBuyPriceAsync(string currencyPair, CancellationToken cancellationToken)
        {
            return await this._options.CreateBaseRequest()
               .AppendPathSegments(currencyPair, "buy")
               .GetJsonAsync<Response<Money>>(cancellationToken)
               .ConfigureAwait(false);
        }

        /// <summary>
        /// Get the total price to sell one bitcoin or ether.
        /// Note that exchange rates fluctuates so the price is only correct for seconds at the time.This sell price includes standard Coinbase fee (1%) but excludes any other fees including bank fees.
        /// </summary>
        /// <param name="currencyPair">Currency pair such as BTC-USD, ETH-USD, etc.</param>
        public async Task<Response<Money>> GetSellPriceAsync(string currencyPair, CancellationToken cancellationToken)
        {
            return await this._options.CreateBaseRequest()
               .AppendPathSegments(currencyPair, "sell")
               .GetJsonAsync<Response<Money>>(cancellationToken)
               .ConfigureAwait(false);
        }

        /// <summary>
        /// Get the current market price for bitcoin. This is usually somewhere in between the buy and sell price.
        ///Note that exchange rates fluctuates so the price is only correct for seconds at the time.
        /// </summary>
        /// <param name="currencyPair"></param>
        /// <param name="cancellationToken"></param>
        public async Task<Response<Money>> GetSpotPriceAsync(string currencyPair, DateTime? date, CancellationToken cancellationToken)
        {
            var req = this._options.CreateBaseRequest()
               .AppendPathSegments(currencyPair, "spot");

            if (!(date is null))
            {
                req = req.SetQueryParam("date", date.Value.ToString("yyyy-MM-dd"));
            }

            return await req.GetJsonAsync<Response<Money>>(cancellationToken).ConfigureAwait(false);
        }

    }
}
