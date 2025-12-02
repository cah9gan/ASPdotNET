using Microsoft.AspNetCore.Mvc;
using TransportStore.Models;

namespace TransportStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IStoreRepository repository;

        public NavigationMenuViewComponent(IStoreRepository repo)
        {
            repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];

            var categories = repository.Transports
                .Select(x => x.Type)
                .Distinct()
                .OrderBy(x => x);

            return View(categories);
        }
    }
}