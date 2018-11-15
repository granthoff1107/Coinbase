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
    public interface ITimeEndpoint
    {
        /// <summary>
        /// Get the API server time.
        /// </summary>
        Task<Response<Time>> GetCurrentTimeAsync(CancellationToken cancellationToken = default);
    }

    public class TimeEndpoint : EndpointBase, ITimeEndpoint
    {
        public TimeEndpoint(EndpointOptions options) : base(options)
        {
        }

        /// <summary>
        /// Get the API server time.
        /// </summary>
        public async Task<Response<Time>> GetCurrentTimeAsync(CancellationToken cancellationToken)
        {
            return await this._options.CreateBaseRequest().GetJsonAsync<Response<Time>>(cancellationToken);
        }
    }
}
