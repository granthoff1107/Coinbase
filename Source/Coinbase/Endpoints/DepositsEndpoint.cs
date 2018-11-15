using System;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Endpoints;
using Coinbase.Models;
using Flurl.Http;

namespace Coinbase
{

    public interface IDepositsEndpoint : IReadEndpoint<Deposit>
    {
        Task<Response<Deposit>> CreateAsync<TInput>(TInput entity, CancellationToken cancellationToken = default);
        Task<Response<Deposit>> CommitAsync(string id, CancellationToken cancellationToken = default);
    }


    public class DepositsEndpoint : ReadEndpoint<Deposit>, IDepositsEndpoint
    {
        public DepositsEndpoint(EndpointOptions options) : base(options)
        {
        }

        public async Task<Response<Deposit>> CommitAsync(string id, CancellationToken cancellationToken = default)
        {
            return await this._endpointRule.CommitAsync<Deposit>(this._options, id, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Response<Deposit>> CreateAsync<TInput>(TInput entity, CancellationToken cancellationToken = default)
        {
            return await this._endpointRule.CreateAsync<Deposit, TInput>(this._options, entity, cancellationToken).ConfigureAwait(false);
        }
    }
}