using System;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Endpoints;
using Coinbase.Models;
using Flurl.Http;

namespace Coinbase
{

   public interface IPaymentMethodsEndpoint : IReadEndpoint<PaymentMethod>
   {
   }

   public partial class PaymentMethodsEndpoint : ReadEndpoint<PaymentMethod>, IPaymentMethodsEndpoint
   {
        public PaymentMethodsEndpoint(EndpointOptions options): base(options)
        {
        }
   }
}