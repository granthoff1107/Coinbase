using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase
{
    public interface ICoinbaseOAuthClient : ICoinbaseClient
    {
    }

    public partial class CoinbaseOAuthClient : CoinbaseApiBase<OAuthConfig>, ICoinbaseOAuthClient
    {
        public CoinbaseOAuthClient(OAuthConfig config) : base(config)
        {
        }

        protected override IFlurlClient ConfigureClient(IFlurlClient client)
        {
            return client.WithOAuthBearerToken(this.Config.OAuthToken);
        }
    }
}
