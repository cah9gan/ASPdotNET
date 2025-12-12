using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using TransportStore.BlazorClient.Models;
using TransportStore.BlazorClient.Providers; 

namespace TransportStore.BlazorClient.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> Register(RegisterDto registerModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/account/register", registerModel);
            return result.IsSuccessStatusCode;
        }

        public async Task<AuthResponseDto?> Login(LoginDto loginModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/account/login", loginModel);

            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadFromJsonAsync<AuthResponseDto>();
                
                // CS8604 Fix: Check if response is null before accessing properties
                if (response != null)
                {
                    await _localStorage.SetItemAsync("authToken", response.Token);
                    
                    // We can now safely pass response.Token as it's not null here
                    ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(response.Token);
                    
                    return response;
                }
            }

            return null;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
        }
    }
}