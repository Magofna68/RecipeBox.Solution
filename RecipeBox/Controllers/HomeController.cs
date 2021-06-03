// using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBox.Controllers
{
  public class HomeController : Controller
  {
    private readonly RecipeBoxContext _db;
    public HomeController(RecipeBoxContext db)
    {
      _db = db;
    }
    // [HttpGet("/")]
    // public ActionResult Index()
    // {
    //   return View();
    // }

    public ActionResult Index()
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      List<Recipe> recipes = _db.Recipies.ToList();
      List<Category> categories = _db.Categories.ToList();
      model.Add("recipes", recipes);
      model.Add("categories", categories);
      return View(model);
    }
  }
}