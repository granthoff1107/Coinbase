using System;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Endpoints;
using Coinbase.Models;
using Flurl.Http;

namespace Coinbase
{

    public interface IWithdrawalsEndpoint : IReadEndpoint<WithdrawalFunds>
    {
        Task<Response<WithdrawalFunds>> CreateAsync<TInput>(TInput entity, CancellationToken cancellationToken = default);
        Task<Response<WithdrawalFunds>> CommitAsync(
            string id, CancellationToken cancellationToken = default);
    }

    public class WithdrawalsEndpoint : ReadEndpoint<WithdrawalFunds>, IWithdrawalsEndpoint
    {
        public WithdrawalsEndpoint(EndpointOptions options) : base(options)
        {

        }

        public async Task<Response<WithdrawalFunds>> CommitAsync(string id, CancellationToken cancellationToken = default)
        {
            return await this._endpointRule.CommitAsync<WithdrawalFunds>(this._options, id, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Response<WithdrawalFunds>> CreateAsync<TInput>(TInput entity, CancellationToken cancellationToken = default)
        {
            return await this._endpointRule.CreateAsync<WithdrawalFunds, TInput>(this._options, entity, cancellationToken).ConfigureAwait(false);
        }
    }
}