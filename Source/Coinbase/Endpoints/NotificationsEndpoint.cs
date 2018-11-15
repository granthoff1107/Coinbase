using System;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Endpoints;
using Coinbase.Models;
using Flurl.Http;

namespace Coinbase
{

   public interface INotificationsEndpoint : IReadEndpoint<Notification>
   {
   }

   public partial class NotificationsEndpoint : ReadEndpoint<Notification>, INotificationsEndpoint
   {
        public NotificationsEndpoint(EndpointOptions options): base(options)
        {
        }
   }
}