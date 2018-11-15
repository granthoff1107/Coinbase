using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Models
{
    public static class CoinbaseConsts
    {
        public const string ApiVersionDate = "2017-08-07";
        public const string Endpoint = "https://api.coinbase.com/v2/";
        public static readonly string UserAgent =
             $"{AssemblyVersionInformation.AssemblyProduct}/{AssemblyVersionInformation.AssemblyVersion} ({AssemblyVersionInformation.AssemblyTitle}; {AssemblyVersionInformation.AssemblyDescription})";

    }
}
