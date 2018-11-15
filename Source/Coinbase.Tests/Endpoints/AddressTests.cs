﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Coinbase.Models;
using FluentAssertions;
using NUnit.Framework;
using static Coinbase.Tests.Examples;

namespace Coinbase.Tests.Endpoints
{
    public class AddressTests : OAuthServerTest
    {

        [Test]
        public async Task can_list_addresses()
        {
            SetupServerPagedResponse(PaginationJson, $"{Address1},{Address2}");

            var accounts = await client.Accounts.ChildOf("ffff").Addresses.GetListAsync();

            var truth = new PagedResponse<AddressEntity>
            {
                Pagination = PaginationModel,
                Data = new[] { Address1Model, Address2Model }
            };

            truth.Should().BeEquivalentTo(accounts);

            server.ShouldHaveExactCall("https://api.coinbase.com/v2/accounts/ffff/addresses")
               .WithVerb(HttpMethod.Get);
        }

        [Test]
        public async Task get_an_address()
        {
            SetupServerSingleResponse(Address1);

            var account = await client.Accounts.ChildOf("ffff").Addresses.GetAsync(Address1Model.Id);

            var truth = new Response<AddressEntity>
            {
                Data = Address1Model
            };

            truth.Should().BeEquivalentTo(account);

            server.ShouldHaveExactCall($"https://api.coinbase.com/v2/accounts/ffff/addresses/{Address1Model.Id}")
               .WithVerb(HttpMethod.Get);
        }



        [Test]
        public async Task can_list_address_transactions()
        {
            SetupServerPagedResponse(PaginationJson, $"{Transaction1}");

            var txs = await client.Accounts.ChildOf("fff").Addresses.ListAddressTransactionsAsync("uuu");

            var truth = new PagedResponse<Transaction>
            {
                Pagination = PaginationModel,
                Data = new[] { Transaction1Model }
            };

            truth.Should().BeEquivalentTo(txs);

            server.ShouldHaveExactCall($"https://api.coinbase.com/v2/accounts/fff/addresses/uuu/transactions")
               .WithVerb(HttpMethod.Get);
        }

        [Test]
        public async Task can_create_address()
        {
            SetupServerSingleResponse(Address1WithName("ddd"));

            var create = new CreateAddress { Name = "ddd" };
            var add = await client.Accounts.ChildOf("fff").Addresses.CreateAsync(create);

            var truth = new Response<AddressEntity>
            {
                Data = Address1Model
            };

            truth.Data.Name = "ddd";

            truth.Should().BeEquivalentTo(add);

            server.ShouldHaveExactCall("https://api.coinbase.com/v2/accounts/fff/addresses")
               .WithVerb(HttpMethod.Post);
        }

    }
}