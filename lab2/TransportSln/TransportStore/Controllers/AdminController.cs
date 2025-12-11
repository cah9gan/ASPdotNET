using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;
using TransportStore.Domain.Models; // Updated namespace

namespace TransportStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IStoreRepository repository;

        public AdminController(IStoreRepository repo)
        {
            repository = repo;
        }

        public IActionResult Index() => View(repository.Transports);

        public IActionResult Details(long id)
        {
            var transport = repository.Transports
                .FirstOrDefault(t => t.Id == id);
                
            if (transport == null) return NotFound();
            
            return View(transport);
        }

        public IActionResult Edit(long id) 
        {
            if (id == 0)
            {
                return View(new Transport()); 
            }
            else
            {
                return View(repository.Transports.FirstOrDefault(t => t.Id == id));
            }
        }

        [HttpPost]
        public IActionResult Edit(Transport transport)
        {
            if (ModelState.IsValid)
            {
                repository.SaveTransport(transport);
                return RedirectToAction("Index");
            }
            return View(transport);
        }

        public IActionResult Delete(long id)
        {
            var transport = repository.Transports.FirstOrDefault(t => t.Id == id);
            if (transport == null) return NotFound();
            return View(transport);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(long id)
        {
            var transport = repository.Transports.FirstOrDefault(t => t.Id == id);
            if (transport != null)
            {
                repository.DeleteTransport(transport);
            }
            return RedirectToAction("Index");
        }

        
        [HttpPost]
        public IActionResult AddReview(Review review)
        {
            review.Date = DateTime.Now;
            repository.CreateReview(review);
            return RedirectToAction("Details", new { id = review.TransportId });
        }

        [HttpPost]
        public IActionResult DeleteReview(int reviewId, long transportId)
        {
            var review = repository.Reviews.FirstOrDefault(r => r.Id == reviewId);
            if (review != null)
            {
                repository.DeleteReview(review);
            }
            return RedirectToAction("Details", new { id = transportId });
        }
    }
}