using Blazored.LocalStorage;
using BlazorMud.Contracts.Security;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace BlazorMud
{
    /// <summary>
    /// Authenticationprovider that watches the user's local storage for token changes.
    /// </summary>
    public sealed class JwtAuthStateProvider : RevalidatingServerAuthenticationStateProvider
    {
        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(1);

        private readonly ILogger<JwtAuthStateProvider> _Logger;
        private readonly ITokenManager _TokenManager;
        private readonly ILocalStorageService _LocalStorageService;

        private static AuthenticationState AnonymousState { get => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())); }

        public JwtAuthStateProvider(ILogger<JwtAuthStateProvider> logger,
            ITokenManager tokenManager,
            ILocalStorageService localStorageService,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _TokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            _LocalStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));

            _LocalStorageService.Changed += _LocalStorageService_Changed;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                if (!await _LocalStorageService.ContainKeyAsync("token"))
                    return AnonymousState;
            }
            catch (NullReferenceException)
            {
                // HACK: NullReferenceException indicates that JS interop is not available yet - find a better way to detect this!
                return AnonymousState;
            }

            try
            {
                var tokenString = await _LocalStorageService.GetItemAsync<string>("token");
                
                if (string.IsNullOrWhiteSpace(tokenString) || !tokenString.Contains("."))
                    return AnonymousState;
                
                var principal = _TokenManager.Validate(tokenString, out _);
                
                return principal is null ? AnonymousState : new AuthenticationState(principal);
            }
            catch(Exception ex)
            {
                _Logger.LogError(ex, "Error fetching and validating user token.");
                return AnonymousState;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                _LocalStorageService.Changed -= _LocalStorageService_Changed;
        }

        protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            var currentState = await GetAuthenticationStateAsync();
            var result = currentState.User.Identity.Name == authenticationState.User.Identity.Name;
            return result;
        }

        /// <summary>
        /// Reacts to changes to the user's local storage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _LocalStorageService_Changed(object sender, ChangedEventArgs e)
        {
            if (e.Key == "token")
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
