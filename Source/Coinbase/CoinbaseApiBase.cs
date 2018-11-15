using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Endpoints;
using Coinbase.Models;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Coinbase
{
    public interface ICoinbaseClient : IPublicCoinbaseClient
    {
        IAccountsEndpoint Accounts { get; }
        INotificationsEndpoint Notifications { get; }
        IPaymentMethodsEndpoint PaymentMethods { get; }
        IUsersEndpoint Users { get; }
    }

    public class CoinbaseApiBase : PublicCoinbaseClient, ICoinbaseClient
    {
        internal CoinbaseApiBase(Config config) : base(config)
        {
        }

        protected override void InitializeEndpoints()
        {
            this.Accounts = new AccountsEndpoint(this.CreateOptions("accounts"));
            this.PaymentMethods = new PaymentMethodsEndpoint(this.CreateOptions("payment-methods"));
            this.Notifications = new NotificationsEndpoint(this.CreateOptions("notifications"));
            this.Users = new UsersEndpoint(this.CreateOptions());
            base.InitializeEndpoints();
        }

        public IAccountsEndpoint Accounts { get; internal protected set; }
        public IPaymentMethodsEndpoint PaymentMethods { get; internal protected set; }
        public INotificationsEndpoint Notifications { get; internal protected set; }
        public IUsersEndpoint Users { get; internal protected set; }
    }

    public partial class CoinbaseApiBase<TConfig> : CoinbaseApiBase
       where TConfig : Config, new()
    {
        public TConfig Config
        {
            get { return base._config as TConfig; }
            internal set
            {
                this._config = value;
                base._config = value;
            }
        }

        /// <summary>
        /// The main class for making Coinbase API calls.
        /// </summary>
        public CoinbaseApiBase(TConfig config) : base(config)
        {
            this.Config = config;
            this.Config.EnsureValid();
        }
    }
}