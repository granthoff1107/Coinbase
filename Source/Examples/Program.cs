using System;
using System.Threading.Tasks;
using Coinbase;
using Coinbase.Models;
using Flurl.Http;
using static Coinbase.HeaderNames;

namespace Examples
{
   class Program
   {
      static async Task Main(string[] args)
      {
         Console.WriteLine("Hello World!");
         var client = new PublicCoinbaseClient();

         var response = await client.ExchangeRates.GetExchangeRatesAsync();

         if( response.HasError() )
         {
            // transaction is okay!
         }
      }
   }
}
