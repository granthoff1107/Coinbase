using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Endpoints;
using Coinbase.Models;
using Flurl.Http;

namespace Coinbase
{
    public interface ITransactionsEndpoint : IReadEndpoint<Transaction>
    {
        /// <summary>
        /// Send funds to a bitcoin address, bitcoin cash address, litecoin address, ethereum address, or email address. No transaction fees are required for off blockchain bitcoin transactions.
        /// It’s recommended to always supply a unique idem field for each transaction.This prevents you from sending the same transaction twice if there has been an unexpected network outage or other issue.
        /// When used with OAuth2 authentication, this endpoint requires two factor authentication unless used with wallet:transactions:send:bypass-2fa scope.
        ///If the user is able to buy bitcoin, they can send funds from their fiat account using instant exchange feature.Buy fees will be included in the created transaction and the recipient will receive the user defined amount.
        /// </summary>
        Task<Response<Transaction>> SendMoneyAsync(CreateTransaction createTransaction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Transfer bitcoin, bitcoin cash, litecoin or ethereum between two of a user’s accounts. Following transfers are allowed:
        /// * wallet to wallet
        /// * wallet to vault
        /// </summary>
        Task<Response<Transaction>> TransferMoneyAsync(CreateTransfer createTransfer, CancellationToken cancellationToken = default);

        /// <summary>
        /// Requests money from an email address.
        /// </summary>
        Task<Response<Transaction>> RequestMoneyAsync(RequestMoney requestMoney, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lets the recipient of a money request complete the request by sending money to the user who requested the money. This can only be completed by the user to whom the request was made, not the user who sent the request.
        /// </summary>
        Task<HttpResponseMessage> CompleteRequestMoneyAsync(string transactionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lets the user resend a money request. This will notify recipient with a new email.
        /// </summary>
        Task<HttpResponseMessage> ResendRequestMoneyAsync(string transactionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lets a user cancel a money request. Money requests can be canceled by the sender or the recipient.
        /// </summary>
        Task<HttpResponseMessage> CancelRequestMoneyAsync(string transactionId, CancellationToken cancellationToken = default);
    }


    public partial class TransactionsEndpoint : ReadEndpoint<Transaction>, ITransactionsEndpoint
    {
        public TransactionsEndpoint(EndpointOptions options) : base(options)
        {

        }
        /// <summary>
        /// Send funds to a bitcoin address, bitcoin cash address, litecoin address, ethereum address, or email address. No transaction fees are required for off blockchain bitcoin transactions.
        /// It’s recommended to always supply a unique idem field for each transaction.This prevents you from sending the same transaction twice if there has been an unexpected network outage or other issue.
        /// When used with OAuth2 authentication, this endpoint requires two factor authentication unless used with wallet:transactions:send:bypass-2fa scope.
        ///If the user is able to buy bitcoin, they can send funds from their fiat account using instant exchange feature.Buy fees will be included in the created transaction and the recipient will receive the user defined amount.
        /// </summary>
        public async Task<Response<Transaction>> SendMoneyAsync(CreateTransaction createTransaction, CancellationToken cancellationToken)
        {
            return await this._options.CreateBaseRequest()
               .PostJsonAsync(createTransaction, cancellationToken)
               .ReceiveJson<Response<Transaction>>()
               .ConfigureAwait(false);

        }

        /// <summary>
        /// Transfer bitcoin, bitcoin cash, litecoin or ethereum between two of a user’s accounts. Following transfers are allowed:
        /// * wallet to wallet
        /// * wallet to vault
        /// </summary>
        public async Task<Response<Transaction>> TransferMoneyAsync(CreateTransfer createTransfer, CancellationToken cancellationToken)
        {
            return await this._options.CreateBaseRequest()
               .PostJsonAsync(createTransfer, cancellationToken)
               .ReceiveJson<Response<Transaction>>()
               .ConfigureAwait(false);
        }

        /// <summary>
        /// Requests money from an email address.
        /// </summary>
        public async Task<Response<Transaction>> RequestMoneyAsync(RequestMoney requestMoney, CancellationToken cancellationToken)
        {
            return await this._options.CreateBaseRequest()
               .PostJsonAsync(requestMoney, cancellationToken)
               .ReceiveJson<Response<Transaction>>()
               .ConfigureAwait(false);
        }

        /// <summary>
        /// Lets the recipient of a money request complete the request by sending money to the user who requested the money. This can only be completed by the user to whom the request was made, not the user who sent the request.
        /// </summary>
        public async Task<HttpResponseMessage> CompleteRequestMoneyAsync(string transactionId, CancellationToken cancellationToken)
        {
            return await this._options.CreateBaseRequest()
               .AppendPathSegments(transactionId, "complete")
               .PostJsonAsync(null, cancellationToken)
               .ConfigureAwait(false);
        }


        /// <summary>
        /// Lets the user resend a money request. This will notify recipient with a new email.
        /// </summary>
        public async Task<HttpResponseMessage> ResendRequestMoneyAsync(string transactionId, CancellationToken cancellationToken)
        {
            return await this._options.CreateBaseRequest()
               .AppendPathSegments(transactionId, "resend")
               .PostJsonAsync(null, cancellationToken)
               .ConfigureAwait(false);
        }

        /// <summary>
        /// Lets a user cancel a money request. Money requests can be canceled by the sender or the recipient.
        /// </summary>
        public async Task<HttpResponseMessage> CancelRequestMoneyAsync(string transactionId, CancellationToken cancellationToken)
        {
            return await this._options.CreateBaseRequest()
            .AppendPathSegments(transactionId)
            .DeleteAsync(cancellationToken)
            .ConfigureAwait(false);
        }

    }
}