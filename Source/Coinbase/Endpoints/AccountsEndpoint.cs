using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Endpoints;
using Coinbase.Models;
using Flurl;
using Flurl.Http;

namespace Coinbase
{
    public interface IAccountsEndpointManger
    {
        IBuysEndpoint Buys { get; } 
        ISellsEndpoint Sells { get; }
        IAddressesEndpoint Addresses { get; }
        IDepositsEndpoint Deposits { get; }
        IWithdrawalsEndpoint Withdrawals { get; }
        ITransactionsEndpoint Transactions { get; }
    }

    public class AccountsEndpointManager: IAccountsEndpointManger
    {
        string _id;
        EndpointOptions _options;
        public AccountsEndpointManager(EndpointOptions options, string id)
        {
            this._id = id;
            this._options = options;
        }

        protected EndpointOptions CreateOptions(string subroute)
        {
            var options = this._options.GetBaseUrl().AppendPathSegments(this._id, subroute);
            return new EndpointOptions(this._options.Client, options);
        }

        public IBuysEndpoint Buys => new BuysEndpoint(this.CreateOptions("buys"));
        public ISellsEndpoint Sells => new SellsEndpoint(this.CreateOptions("sells"));
        public IAddressesEndpoint Addresses => new AddressEndpoint(this.CreateOptions("addresses"));
        public IDepositsEndpoint Deposits => new DepositsEndpoint(this.CreateOptions("deposits"));
        public IWithdrawalsEndpoint Withdrawals => new WithdrawalsEndpoint(this.CreateOptions("withdrawals"));
        public ITransactionsEndpoint Transactions => new TransactionsEndpoint(this.CreateOptions("transactions"));

    }

    public interface IAccountsEndpoint : IReadEndpoint<Account>
    {
        IAccountsEndpointManger ChildOf(string id);
        /// <summary>
        /// Promote an account as primary account.
        /// </summary>
        Task<Response<Account>> SetAccountAsPrimaryAsync(string accountId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Modifies user’s account.
        /// </summary>
        Task<Response<Account>> UpdateAsync(string accountId, UpdateAccount updateAccount, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes user’s account. In order to remove an account it can’t be:
        /// * Primary account
        /// * Account with non-zero balance
        /// * Fiat account
        /// * Vault with a pending withdrawal
        /// </summary>
        Task<HttpResponseMessage> DeleteAsync(string accountId, CancellationToken cancellationToken = default);
    }


    public partial class AccountsEndpoint : ReadEndpoint<Account>, IAccountsEndpoint
    {
        public AccountsEndpoint(EndpointOptions options) : base(options)
        {
        }

        /// <summary>
        /// Promote an account as primary account.
        /// </summary>
        public async Task<Response<Account>> SetAccountAsPrimaryAsync(string accountId, CancellationToken cancellationToken)
        {
            return await this._options.GetBaseUrl().AppendPathSegments(accountId, "primary")
                           .WithClient(_options.Client)
                           .PostJsonAsync(null, cancellationToken)
                           .ReceiveJson<Response<Account>>();
        }
        /// <summary>
        /// Modifies user’s account.
        /// </summary>
        public async Task<Response<Account>> UpdateAsync(string accountId, 
            UpdateAccount updateAccount, CancellationToken cancellationToken)
        {
            return await this._endpointRule.UpdateAsync<Account, UpdateAccount>(this._options,
                accountId, updateAccount, cancellationToken);
        }

        /// <summary>
        /// Removes user’s account. In order to remove an account it can’t be:
        /// * Primary account
        /// * Account with non-zero balance
        /// * Fiat account
        /// * Vault with a pending withdrawal
        /// </summary>
        public async Task<HttpResponseMessage> DeleteAsync(string accountId, CancellationToken cancellationToken)
        {
            return await this._endpointRule.DeleteAsync(this._options, accountId, cancellationToken);
        }

        public IAccountsEndpointManger ChildOf(string id)
        {
            return new AccountsEndpointManager(this._options, id);
        }
    }
}