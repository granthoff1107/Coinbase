﻿using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Coinbase.Models;
using FluentAssertions;
using NUnit.Framework;
using static Coinbase.Tests.Examples;

namespace Coinbase.Tests.Endpoints
{
   public class TransactionTests : OAuthServerTest
   {
      [Test]
      public async Task can_list()
      {
         SetupServerPagedResponse(PaginationJson, $"{Transaction2},{Transaction3},{Transaction4},{Transaction5}");

         var r = await client.Accounts.ChildOf("fff").Transactions.GetListAsync();

         var truth = new PagedResponse<Transaction>
            {
               Pagination = PaginationModel,
               Data = new[]
                  {
                     Transaction2Model,
                     Transaction3Model,
                     Transaction4Model,
                     Transaction5Model
                  }
            };

         truth.Should().BeEquivalentTo(r);

         server.ShouldHaveExactCall("https://api.coinbase.com/v2/accounts/fff/transactions")
            .WithVerb(HttpMethod.Get);
      }

      [Test]
      public async Task can_get()
      {
         SetupServerSingleResponse(Transaction5);

         var r = await client.Accounts.ChildOf("fff").Transactions.GetAsync("uuu");

         var truth = new Response<Transaction>
         {
            Data = Transaction5Model
         };

         truth.Should().BeEquivalentTo(r);

         server.ShouldHaveExactCall($"https://api.coinbase.com/v2/accounts/fff/transactions/uuu")
            .WithVerb(HttpMethod.Get);
      }



      [Test]
      public async Task can_send()
      {
         SetupServerSingleResponse(Transaction6);

         var createTx = new CreateTransaction
            {
               Type = "send",
               To = "1AUJ8z5RuHRTqD1eikyfUUetzGmdWLGkpT",
               Amount = 0.1m,
               Currency = "BTC",
               Idem = "9316dd16-0c05"
         };
         var r = await client.Accounts.ChildOf("fff").Transactions.SendMoneyAsync(createTx );

         var truth = new Response<Transaction>
         {
            Data = Transaction6Model
         };

         truth.Should().BeEquivalentTo(r);

         server.ShouldHaveRequestBody(
            "{\"type\":\"send\",\"to\":\"1AUJ8z5RuHRTqD1eikyfUUetzGmdWLGkpT\",\"amount\":0.1,\"currency\":\"BTC\",\"skip_notifications\":false,\"idem\":\"9316dd16-0c05\",\"to_financial_institution\":false}");

         server.ShouldHaveExactCall($"https://api.coinbase.com/v2/accounts/fff/transactions")
            .WithVerb(HttpMethod.Post);
      }

      [Test]
      public async Task can_transfer()
      {
         SetupServerSingleResponse(Transaction7);

         var createTx = new CreateTransfer
            {
               Type = "send",
               To = "1AUJ8z5RuHRTqD1eikyfUUetzGmdWLGkpT",
               Amount = 0.1m,
               Currency = "BTC"
            };
         var r = await client.Accounts.ChildOf("fff").Transactions.TransferMoneyAsync(createTx);

         var truth = new Response<Transaction>
            {
               Data = Transaction7Model
            };

         truth.Should().BeEquivalentTo(r);

         server.ShouldHaveRequestBody(
            @"{""type"":""send"",""to"":""1AUJ8z5RuHRTqD1eikyfUUetzGmdWLGkpT"",""amount"":0.1,""currency"":""BTC""}");

         server.ShouldHaveExactCall($"https://api.coinbase.com/v2/accounts/fff/transactions")
            .WithVerb(HttpMethod.Post);
      }

      [Test]
      public async Task can_request()
      {
         SetupServerSingleResponse(Transaction7);

         var create = new RequestMoney
            {
               Type = "request",
               To = "email@example.com",
               Amount = 0.1m,
               Currency = "BTC"
            };
         var r = await client.Accounts.ChildOf("fff").Transactions.RequestMoneyAsync(create);

         var truth = new Response<Transaction>
            {
               Data = Transaction7Model
            };


         truth.Should().BeEquivalentTo(r);

         server.ShouldHaveExactCall("https://api.coinbase.com/v2/accounts/fff/transactions")
            .WithVerb(HttpMethod.Post);
      }


      [Test]
      public async Task can_compelte()
      {
         var r = await client.Accounts.ChildOf("fff").Transactions.CompleteRequestMoneyAsync("uuu");

         server.ShouldHaveExactCall("https://api.coinbase.com/v2/accounts/fff/transactions/uuu/complete")
            .WithVerb(HttpMethod.Post);
      }


      [Test]
      public async Task can_resend()
      {
         var r = await client.Accounts.ChildOf("fff").Transactions.ResendRequestMoneyAsync("uuu");

         server.ShouldHaveExactCall("https://api.coinbase.com/v2/accounts/fff/transactions/uuu/resend")
            .WithVerb(HttpMethod.Post);
      }


      [Test]
      public async Task can_cancel()
      {
         var r = await client.Accounts.ChildOf("fff").Transactions.CancelRequestMoneyAsync("uuu");

         server.ShouldHaveExactCall("https://api.coinbase.com/v2/accounts/fff/transactions/uuu")
            .WithVerb(HttpMethod.Delete);
      }

   }
}