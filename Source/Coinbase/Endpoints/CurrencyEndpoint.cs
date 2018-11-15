using Coinbase.Models;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Endpoints
{
    public interface ICurrencyEndpoint
    {
        /// <summary>
        /// List known currencies. Currency codes will conform to the ISO 4217 standard where possible. Currencies which have or had no representation in ISO 4217 may use a custom code (e.g. BTC).
        /// </summary>
        Task<PagedResponse<Currency>> GetCurrenciesAsync();
    }


    public class CurrencyEndpoint : EndpointBase, ICurrencyEndpoint
    {
        public CurrencyEndpoint(EndpointOptions options) : base(options)
        {
        }

        /// <summary>
        /// List known currencies. Currency codes will conform to the ISO 4217 standard where possible. Currencies which have or had no representation in ISO 4217 may use a custom code (e.g. BTC).
        /// </summary>
        public Task<PagedResponse<Currency>> GetCurrenciesAsync()
        {
            return this._options.CreateBaseRequest().GetJsonAsync<PagedResponse<Currency>>();
        }
    }
}
