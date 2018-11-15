using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Coinbase.Models;
using Flurl.Http;
using NUnit.Framework;
using Z.ExtensionMethods;

namespace Coinbase.Tests.Integration
{
   [Explicit]
   public class IntegrationTests
   {
      protected ICoinbaseClient client;

      public IntegrationTests()
      {
         Directory.SetCurrentDirectory(Path.GetDirectoryName(typeof(IntegrationTests).Assembly.Location));
         var lines = File.ReadAllLines("../../.secrets.txt");
         var apiKey = lines[0].GetAfter(":");
         var apiSecret = lines[1].GetAfter(":");

         var webProxy = new WebProxy("http://localhost.:8888", BypassOnLocal: false);

         FlurlHttp.Configure(settings =>
            {
               settings.HttpClientFactory = new ProxyFactory(webProxy);
            });

         client = new CoinbaseApiClient(new ApiKeyConfig{ ApiKey = apiKey, ApiSecret = apiSecret});
      }
   }
   [Explicit]
   public class UserTests : IntegrationTests
   {
      [Test]
      public async Task can_get_auths()
      {
         var r = await client.Users.GetAuthInfoAsync();
         r.Dump();
      }

      [Test]
      public async Task check_account_list()
      {
         var r = await client.Accounts.GetListAsync();
         r.Dump();
      }

      [Test]
      public async Task check_account_transactions()
      {
         var r = await client.Accounts.ChildOf("fff").Transactions.GetListAsync();

         r.Dump();
      }

      [Test]
      public async Task check_invalid_account()
      {
         var r = await client.Accounts.GetAsync("fff");
         r.Dump();
      }

      [Test]
      public async Task test_state()
      {
         var accounts = await client.Accounts.GetListAsync();
         var ethAccount = accounts.Data.FirstOrDefault(x => x.Name == "ETH Wallet");
         var ethAddresses = await client.Accounts.ChildOf(ethAccount.Id).Addresses.GetListAsync();
         var ethAddress = ethAddresses.Data.FirstOrDefault();
         var ethTransactions = await client.Accounts.ChildOf(ethAccount.Id).Transactions.GetListAsync();
      }
   }
}