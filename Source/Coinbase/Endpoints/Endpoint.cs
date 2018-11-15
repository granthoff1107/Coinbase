using Coinbase.Models;
using Coinbase.Rules;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Coinbase
{
    public class EndpointOptions
    {
        public IFlurlClient Client { get; set; }
        public Func<Url> GetBaseUrl { get; set; }

        public IFlurlRequest CreateBaseRequest() => this.GetBaseUrl().WithClient(this.Client);

        public EndpointOptions(IFlurlClient client, string baseUrl) : this(client, new Url(baseUrl))
        {
        }

        public EndpointOptions(IFlurlClient client, Url baseUrl)
        {
            this.Client = client;
            this.GetBaseUrl = () => baseUrl;
        }
    }
    public interface IEndpoint
    {
    }

    public class EndpointBase: EndpointBase<EndpointOptions>
    {
        protected EndpointRule _endpointRule;

        public EndpointBase(EndpointOptions options) : base(options)
        {
            this._endpointRule = new EndpointRule();

        }
    }

    public class EndpointBase<T> : IEndpoint
        where T: EndpointOptions
    {
        protected T _options;

        public EndpointBase(T options)
        {
            this._options = options;
        }
    }
}
