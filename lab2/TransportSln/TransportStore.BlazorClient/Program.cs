using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TransportStore.BlazorClient;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using TransportStore.BlazorClient.Services;
using TransportStore.BlazorClient.Providers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 1. Реєструємо LocalStorage (для зберігання JWT токена)
builder.Services.AddBlazoredLocalStorage();

// 2. Додаємо підтримку авторизації в ядрі Blazor
builder.Services.AddAuthorizationCore();

// 3. Реєструємо наш кастомний провайдер стану авторизації
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// 4. Реєструємо сервіс для логіна/реєстрації
builder.Services.AddScoped<IAuthService, AuthService>();

// 5. Налаштування HttpClient з адресою вашого API
// Переконайтеся, що порт (5153) збігається з тим, який виводить WebAPI при запуску
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5153") });

await builder.Build().RunAsync();