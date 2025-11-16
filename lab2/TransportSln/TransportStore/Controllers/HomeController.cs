using Microsoft.AspNetCore.Mvc;
using TransportStore.Models; 
using TransportStore.Models.ViewModels; 

namespace TransportStore.Controllers
{
    public class HomeController : Controller
    {
        private IStoreRepository repository;
        public int PageSize = 3;
        public HomeController(IStoreRepository repo)
        {
            repository = repo;
        }


        public IActionResult Index(int page = 1)
        {
            
            return View(new TransportListViewModel
            {
                
                Transports = repository.Transports
                    .OrderBy(t => t.Id) 
                    .Skip((page - 1) * PageSize) 
                    .Take(PageSize),             

   
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Transports.Count()
                }
            });
        }
    }
}