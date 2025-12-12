using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TransportStore.Domain.Models; 

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
            return View(repository.Transports
                .Select(x => x.Type)
                .Distinct()
                .OrderBy(x => x));
        }
    }
}