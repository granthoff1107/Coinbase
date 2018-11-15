﻿using Coinbase.Models;
using Flurl.Http.Testing;
using NUnit.Framework;

namespace Coinbase.Tests
{
   public class ServerTest
   {
      protected HttpTest server;

      [SetUp]
      public void BeforeEachTest()
      {
         server = new HttpTest();
      }

      [TearDown]
      public void AfterEachTest()
      {
         this.server.Dispose();
      }

      protected void SetupServerPagedResponse(string pageJson, string dataJson)
      {
         var json = @"{
    ""pagination"": {pageJson},
    ""data"": [{dataJson}]
}
".Replace("{dataJson}", dataJson)
            .Replace("{pageJson}", pageJson);

         server.RespondWith(json);
      }

      protected void SetupServerSingleResponse(string dataJson)
      {
         var json = @"{
    ""data"": {dataJson}
}
".Replace("{dataJson}", dataJson);

         server.RespondWith(json);
      }
   }

   public class OAuthServerTest : ServerTest
   {
      protected ICoinbaseClient client;

      public string oauthKey = "369ECD3F-2D00-4D7A-ACDB-92C2DC35A878";

      [SetUp]
      public void BeforeEachTest()
      {
         client = new CoinbaseOAuthClient(new OAuthConfig{OAuthToken = oauthKey});
      }

      [TearDown]
      public void AfterEachTest()
      {
         EnsureEveryRequestHasCorrectHeaders();
      }

      private void EnsureEveryRequestHasCorrectHeaders()
      {
         server.ShouldHaveMadeACall()
            .WithHeader(HeaderNames.Version, CoinbaseConsts.ApiVersionDate)
            .WithHeader("User-Agent", CoinbaseConsts.UserAgent)
            .WithHeader("Authorization", $"Bearer {oauthKey}");
      }
   }

}