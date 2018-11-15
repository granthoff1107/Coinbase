using System;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Endpoints;
using Coinbase.Models;
using Flurl.Http;

namespace Coinbase
{

    public interface ISellsEndpoint : IReadEndpoint<Sell>
    {
        Task<Response<Sell>> CreateAsync<TInput>(TInput entity, CancellationToken cancellationToken = default);
        Task<Response<Sell>> CommitAsync(string id, CancellationToken cancellationToken = default);
    }


    public class SellsEndpoint : ReadEndpoint<Sell>, ISellsEndpoint
    {
        public SellsEndpoint(EndpointOptions options) : base(options)
        {

        }

        public async Task<Response<Sell>> CommitAsync(string id,
            CancellationToken cancellationToken = default)
        {
            return await this._endpointRule.CommitAsync<Sell>(this._options, id, cancellationToken).ConfigureAwait(false);

        }

        public async Task<Response<Sell>> CreateAsync<TInput>(TInput entity, CancellationToken cancellationToken = default)
        {
            return await this._endpointRule.CreateAsync<Sell, TInput>(this._options, entity, cancellationToken).ConfigureAwait(false);
        }
    }
}