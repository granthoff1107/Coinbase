﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Configuration;
using NUnit.Framework;

namespace Coinbase.Tests.Integration
{
   public class DataTests 
   {
      private PublicCoinbaseClient client;

      [SetUp]
      public void BeforeEachTest()
      {
         client = new PublicCoinbaseClient();
      }

      [Test]
      public async Task can_get_currencies()
      {
         var r = await client.Currencies.GetCurrenciesAsync();
         var usd = r.Data.Where(c => c.Id == "USD").First();
         usd.Name.Should().StartWith("United States");
      }

      [Test]
      public async Task can_get_exchange_rates()
      {
         var r = await client.ExchangeRates.GetExchangeRatesAsync("ETH");
         r.Data.Currency.Should().Be("ETH");
         r.Data.Rates["USD"].Should().BeGreaterThan(5);
      }

      [Test]
      public async Task can_get_buyprice()
      {
         var r = await client.Prices.GetBuyPriceAsync("ETH-USD");
         r.Dump();
         r.Data.Amount.Should().BeGreaterThan(5);
         r.Data.Currency.Should().Be("USD");
         r.Data.Base.Should().Be("ETH");
      }
      [Test]
      public async Task can_get_sellprice()
      {
         var r = await client.Prices.GetSellPriceAsync("ETH-USD");
         r.Dump();
         r.Data.Amount.Should().BeGreaterThan(5);
         r.Data.Currency.Should().Be("USD");
         r.Data.Base.Should().Be("ETH");
      }
      [Test]
      public async Task can_get_spotprice()
      {
         var r = await client.Prices.GetSpotPriceAsync("ETH-USD");
         r.Dump();
         r.Data.Amount.Should().BeGreaterThan(5);
         r.Data.Currency.Should().Be("USD");
         r.Data.Base.Should().Be("ETH");
      }

      [Test]
      public async Task can_get_time()
      {
         var r = await client.Time.GetCurrentTimeAsync();
         r.Dump();
         r.Data.Iso.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromHours(1));
      }
   }

   public class ProxyFactory : DefaultHttpClientFactory
   {
      private readonly WebProxy proxy;

      public ProxyFactory(WebProxy proxy)
      {
         this.proxy = proxy;
      }

      public override HttpMessageHandler CreateMessageHandler()
      {
         return new HttpClientHandler
            {
               Proxy = this.proxy,
               UseProxy = true
            };
      }
   }
}