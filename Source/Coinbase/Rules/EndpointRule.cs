using Coinbase.Models;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Coinbase.Rules
{
    public class EndpointRule
    {
        public async Task<Response<T>> GetAsync<T>(EndpointOptions options, string id, CancellationToken cancellationToken = default)
        {
            return await options.GetBaseUrl()
               .AppendPathSegment(id)
               .WithClient(options.Client)
               .GetJsonAsync<Response<T>>(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task<PagedResponse<T>> GetListAsync<T>(EndpointOptions options, CancellationToken cancellationToken = default)
        {
            return await options.GetBaseUrl()
               .WithClient(options.Client)
               .GetJsonAsync<PagedResponse<T>>(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task<Response<T>> CreateAsync<T, TInput>(EndpointOptions options, TInput entity, CancellationToken cancellationToken = default)
        {
            return await options.GetBaseUrl()
               .WithClient(options.Client)
               .PostJsonAsync(entity, cancellationToken)
               .ReceiveJson<Response<T>>()
               .ConfigureAwait(false);
        }

        public async Task<Response<T>> UpdateAsync<T, TInput>(EndpointOptions options, string id, TInput entity, CancellationToken cancellationToken = default)
        {

            return await options.GetBaseUrl().AppendPathSegments(id)
              .WithClient(options.Client)
              .PutJsonAsync(entity, cancellationToken)
               .ReceiveJson<Response<T>>()
               .ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> DeleteAsync(EndpointOptions options, string id, CancellationToken cancellationToken)
        {

            return await options.GetBaseUrl().AppendPathSegments(id)
                          .WithClient(options.Client)
                          .DeleteAsync(cancellationToken)
                          .ConfigureAwait(false);
        }

        public async Task<Response<T>> CommitAsync<T>(EndpointOptions options, string id, CancellationToken cancellationToken = default)
        {
            return await options.GetBaseUrl()
                .AppendPathSegments(id, "commit")
                .WithClient(options.Client)
                .PostJsonAsync(null, cancellationToken)
                .ReceiveJson<Response<T>>()
                .ConfigureAwait(false);
        }
    }
}
