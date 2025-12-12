using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TransportStore.Domain.Models;
using TransportStore.Hubs;
using TransportStore.Infrastructure;
using TransportStore.Models;
using TransportStore.Models.ViewModels;

namespace TransportStore.Controllers
{
    public class CartController : Controller
    {
        private IStoreRepository repository;
        private readonly IHubContext<TransportHub> _hubContext;

        public CartController(IStoreRepository repo, IHubContext<TransportHub> hubContext)
        {
            repository = repo;
            _hubContext = hubContext;
        }

        public IActionResult Index(string returnUrl)
        {
            return View(new CartViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl ?? "/"
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(long transportId, string returnUrl)
        {
            Transport? transport = repository.Transports
                .FirstOrDefault(p => p.Id == transportId);

            if (transport != null)
            {
                Cart cart = GetCart();
                cart.AddItem(transport, 1);
                SaveCart(cart);

                await _hubContext.Clients.All.SendAsync("ReceiveAvailabilityUpdate", transportId, false);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(long transportId, string returnUrl)
        {
            Transport? transport = repository.Transports
                .FirstOrDefault(p => p.Id == transportId);

            if (transport != null)
            {
                Cart cart = GetCart();
                cart.RemoveLine(transport);
                SaveCart(cart);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        private Cart GetCart()
        {
            return HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
        }

        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }
    }
}