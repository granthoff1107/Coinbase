using System;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Endpoints;
using Coinbase.Models;
using Flurl.Http;

namespace Coinbase
{
   public interface IBuysEndpoint : IReadEndpoint<Buy>
   {
        Task<Response<Buy>> CreateAsync<TInput>(TInput entity, CancellationToken cancellationToken = default);
        Task<Response<Buy>> CommitAsync(string id, CancellationToken cancellationToken = default);
   }

   public class BuysEndpoint : ReadEndpoint<Buy>, IBuysEndpoint
   {
        public BuysEndpoint(EndpointOptions options):base(options)
        {

        }

        public async Task<Response<Buy>> CommitAsync(string id, CancellationToken cancellationToken = default)
        {
            return await this._endpointRule.CommitAsync<Buy>(this._options, id, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Response<Buy>> CreateAsync<TInput>(TInput entity, CancellationToken cancellationToken = default)
        {
            return await this._endpointRule.CreateAsync<Buy, TInput>(this._options, entity, cancellationToken).ConfigureAwait(false);
        }
    }
}