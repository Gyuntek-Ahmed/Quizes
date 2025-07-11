using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Quizes.Shared;
using System.Security.Claims;

namespace Quizes.Web.Auth
{
    public class QuizAuthStateProvider : AuthenticationStateProvider
    {
        private const string AuthType = "quias-auth";
        private const string UserDataKet = "udata";

        public LoggedInUser User { get; private set; } = null!;

        public bool IsLoggedIn => User?.Id > 0;

        public bool IsInitializing { get; private set; } = true;

        public IJSRuntime _jSRuntime { get; }

        private Task<AuthenticationState> _authStateTask = null!;

        public QuizAuthStateProvider(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
            SetAuthStateTask();
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
            => _authStateTask;

        public async Task InitializeAsync()
        {
            try
            {
                var userData = await _jSRuntime.InvokeAsync<string?>("localStorage.getItem", UserDataKet);

                if (string.IsNullOrEmpty(userData))
                    return;

                var user = LoggedInUser.FromJson(userData);

                if (user == null || user.Id == 0)
                    return;

                await SetLoginAsync(user);
            }
            finally
            {
                IsInitializing = false;
            }
        }

        public async Task SetLoginAsync(LoggedInUser user)
        {
            User = user;

            SetAuthStateTask();
            NotifyAuthenticationStateChanged(_authStateTask);
            await _jSRuntime.InvokeVoidAsync("localStorage.setItem", UserDataKet, user.ToJson());
        }

        public async Task SetLogoutAsync()
        {
            User = null!;

            SetAuthStateTask();
            NotifyAuthenticationStateChanged(_authStateTask);
            await _jSRuntime.InvokeVoidAsync("localStorage.removeItem", UserDataKet);
        }

        private void SetAuthStateTask()
        {
            if (IsLoggedIn)
            {
                var identity = new ClaimsIdentity(User?.ToClaims(), AuthType);
                var principal = new ClaimsPrincipal(identity);
                var authState = new AuthenticationState(principal);

                _authStateTask = Task.FromResult(authState);
            }
            else
            {
                var identity = new ClaimsIdentity();
                var principal = new ClaimsPrincipal(identity);
                var authState = new AuthenticationState(principal);
                _authStateTask = Task.FromResult(authState);
            }
        }
    }
}
