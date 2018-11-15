using System;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Endpoints;
using Coinbase.Models;
using Flurl.Http;

namespace Coinbase
{
    public interface IAddressesEndpoint : IReadEndpoint<AddressEntity>
    {
        /// <summary>
        /// List transactions that have been sent to a specific address. A regular bitcoin, bitcoin cash, litecoin or ethereum address can be used in place of address_id but the address has to be associated to the correct account.
        /// </summary>
        Task<PagedResponse<Transaction>> ListAddressTransactionsAsync(string addressId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new address for an account. As all the arguments are optinal, it’s possible just to do a empty POST which will create a new address. This is handy if you need to create new receive addresses for an account on-demand.
        ///Addresses can be created for all account types.With fiat accounts, funds will be received with Instant Exchange.
        /// </summary>
        Task<Response<AddressEntity>> CreateAsync(CreateAddress createAddress, CancellationToken cancellationToken = default);
    }


    public class AddressEndpoint : ReadEndpoint<AddressEntity>, IAddressesEndpoint
    {
        public AddressEndpoint(EndpointOptions options) : base(options)
        {
        }

        //TODO: Move to Transactions endpoint
        /// <inheritdoc />
        public async Task<PagedResponse<Transaction>> ListAddressTransactionsAsync(string addressId, CancellationToken cancellationToken)
        {
            return await this._options.CreateBaseRequest()
               .AppendPathSegments(addressId, "transactions")
               .GetJsonAsync<PagedResponse<Transaction>>(cancellationToken)
               .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<Response<AddressEntity>> CreateAsync(CreateAddress createAddress, CancellationToken cancellationToken)
        {
            return await this._endpointRule.CreateAsync<AddressEntity, CreateAddress>(
                                this._options, createAddress, cancellationToken)
                            .ConfigureAwait(false);
        }
    }
}