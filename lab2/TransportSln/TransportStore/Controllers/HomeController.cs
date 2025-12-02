using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Index(string? category, int page = 1)
        {
            var filteredData = repository.Transports
                .Where(p => category == null || p.Type == category);

            var totalItems = filteredData.Count();

            return View(new TransportListViewModel
            {
                Transports = filteredData
                    .OrderBy(t => t.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),             

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = totalItems
                },
                CurrentCategory = category
            });
        }

        public IActionResult Details(long id)
        {
            var transport = repository.Transports
                .Include(t => t.Reviews) 
                .FirstOrDefault(t => t.Id == id);

            if (transport == null) return NotFound();

            return View(transport);
        }

        [HttpPost]
        public IActionResult AddReview(Review review)
        {
            review.Date = DateTime.Now;

            if (ModelState.IsValid)
            {
                repository.CreateReview(review);
                return RedirectToAction("Details", new { id = review.TransportId });
            }

            return RedirectToAction("Details", new { id = review.TransportId });
        }
    }
}