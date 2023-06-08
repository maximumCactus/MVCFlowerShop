using Microsoft.AspNetCore.Mvc;
using MVCFlowerShop.Models;
using MVCFlowerShop.Data;
using Microsoft.EntityFrameworkCore;
using MVC_FlowerShop.Models;

namespace MVCFlowerShop.Controllers
{
    public class FlowersController : Controller
    {
        private readonly MVCFlowerShopContext _context;
        public FlowersController(MVCFlowerShopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            List<Flower> flowerlist = await _context.Flower.ToListAsync();
            if (!string.IsNullOrEmpty(searchString)) {
                flowerlist = flowerlist.Where(s => s.FlowerName.Contains(searchString)).ToList();
            }            

            return View(flowerlist);
        }

        public IActionResult AddData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
AddData(Flower flower)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flower);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));;
            }
            return View(flower);
        }

    }
}
