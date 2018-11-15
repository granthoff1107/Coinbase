using Coinbase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl;
using Coinbase.Rules;

namespace Coinbase.Endpoints
{
    //TODO: Consider getting rid of parent based read points and pass the parent url into the class
    public interface IReadEndpoint<T> : IEndpoint
    {
        Task<PagedResponse<T>> GetListAsync(CancellationToken cancellationToken = default);
        Task<Response<T>> GetAsync(string id, CancellationToken cancellationToken = default);
    }

    public class ReadEndpoint<T> : EndpointBase, IReadEndpoint<T>
    {
        public ReadEndpoint(EndpointOptions options): base(options)
        {
        }

        public async Task<Response<T>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            return await this._endpointRule.GetAsync<T>(this._options, id, cancellationToken);
        }

        public async Task<PagedResponse<T>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await this._endpointRule.GetListAsync<T>(this._options, cancellationToken);
        }
    }
}
