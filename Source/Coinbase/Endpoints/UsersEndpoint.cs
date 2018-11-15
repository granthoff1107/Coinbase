using System;
using System.Threading;
using System.Threading.Tasks;
using Coinbase.Models;
using Flurl;
using Flurl.Http;

namespace Coinbase
{
   public interface IUsersEndpoint
   {
      /// <summary>
      /// Get any user’s public information with their ID.
      /// </summary>
      Task<Response<User>> GetUserAsync(string userId, CancellationToken cancellationToken = default);

      /// <summary>
      /// Get current user’s public information. To get user’s email or private information, use permissions wallet:user:email and wallet:user:read. If current request has a wallet:transactions:send scope, then the response will contain a boolean sends_disabled field that indicates if the user’s send functionality has been disabled.
      /// </summary>
      Task<Response<User>> GetCurrentUserAsync(CancellationToken cancellationToken = default);

      /// <summary>
      /// Get current user’s authorization information including granted scopes and send limits when using OAuth2 authentication.
      /// </summary>
      Task<Response<Auth>> GetAuthInfoAsync(CancellationToken cancellationToken = default);

      /// <summary>
      /// Modify current user and their preferences.
      /// </summary>
      Task<Response<User>> UpdateAsync(UserUpdate update, CancellationToken cancellationToken = default);
   }

   public class UsersEndpoint : EndpointBase, IUsersEndpoint
   {
        public UsersEndpoint(EndpointOptions options) : base(options)
        {
        }

        /// <summary>
        /// Get any user’s public information with their ID.
        /// </summary>
        public async Task<Response<User>> GetUserAsync(string userId, CancellationToken cancellationToken)
      {
         if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException(nameof(userId));

         return await this._options.CreateBaseRequest()
            .AppendPathSegments("users",userId)
            .GetJsonAsync<Response<User>>(cancellationToken);
      }
      /// <summary>
      /// Get current user’s public information. To get user’s email or private information, use permissions wallet:user:email and wallet:user:read. If current request has a wallet:transactions:send scope, then the response will contain a boolean sends_disabled field that indicates if the user’s send functionality has been disabled.
      /// </summary>
      public async Task<Response<User>> GetCurrentUserAsync(CancellationToken cancellationToken)
      {
         return await this._options.CreateBaseRequest()
            .AppendPathSegment("user")
            .GetJsonAsync<Response<User>>(cancellationToken);
      }
      /// <summary>
      /// Get current user’s authorization information including granted scopes and send limits when using OAuth2 authentication.
      /// </summary>
      public async Task<Response<Auth>> GetAuthInfoAsync(CancellationToken cancellationToken)
      {
         return await this._options.CreateBaseRequest()
            .AppendPathSegments("user", "auth")
            .GetJsonAsync<Response<Auth>>(cancellationToken)
            .ConfigureAwait(false);
      }
      /// <summary>
      /// Modify current user and their preferences.
      /// </summary>
      public async Task<Response<User>> UpdateAsync(UserUpdate update, CancellationToken cancellationToken)
      {
         return await this._options.CreateBaseRequest()
            .AppendPathSegment("user")
            .PutJsonAsync(update, cancellationToken)
            .ReceiveJson<Response<User>>();
      }
   }
}