using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using System.Collections.Generic;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class CheeseController : Controller
    {
        private readonly CheeseDbContext context;

        public CheeseController(CheeseDbContext dbContext)
        {
            this.context = dbContext;
        }

        // GET: /<controller>/
        [Route("/")]
        public IActionResult Index()
        {
            IList<Cheese> cheeses = context.Cheeses.Include(cheese => cheese.CheeseCategory).ToList();

            return View(cheeses);
        }

        public IActionResult Add()
        {
            AddCheeseViewModel addCheeseViewModel = new AddCheeseViewModel(context.Categories.ToList());
            return View(addCheeseViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddCheeseViewModel addCheeseViewModel)
        {
            if (ModelState.IsValid)
            {
                // Add the new cheese to my existing cheeses
                CheeseCategory newCheeseCategory = context.Categories.Single(cheese => cheese.ID == addCheeseViewModel.CheeseCategoryID);
                Cheese newCheese = new Cheese
                {
                    Name = addCheeseViewModel.Name,
                    Description = addCheeseViewModel.Description,
                    CheeseCategory = newCheeseCategory
                };

                context.Cheeses.Add(newCheese);
                context.SaveChanges();

                return Redirect("/");
            }

            return View(addCheeseViewModel);
        }

        public IActionResult Remove()
        {
            ViewBag.title = "Remove Cheeses";
            ViewBag.cheeses = context.Cheeses.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Remove(int[] cheeseIds)
        {
            foreach (int cheeseId in cheeseIds)
            {
                Cheese theCheese = context.Cheeses.Single(c => c.ID == cheeseId);
                context.Cheeses.Remove(theCheese);
            }

            context.SaveChanges();

            return Redirect("/");
        }

        public IActionResult Category(int id)
        {
            if (id == 0)
                return Redirect("/Category");

            CheeseCategory theCategory = context.Categories
                .Include(category => category.Cheeses)
                .Single(category => category.ID == id);
            /* 
             IList<Cheese> theCheeses = context.Cheese
             .Include(cheese => cheese.Category)
             .Where(cheese => cheese.CategoryID == id)
             .ToList();
             */

            return View("Index", theCategory.Cheeses);
        }
    }
}
