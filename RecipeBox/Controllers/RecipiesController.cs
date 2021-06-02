using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace RecipeBox.Controllers
{
  public class RecipeController : Controller
  {
    private readonly RecipeBoxContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public RecipeController(UserManager<ApplicationUser> userManager, RecipeBoxContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public ActionResult Index()
    {
      List<Recipe> userRecipe = _db.Recipe.ToList();
      return View(userRecipe);
    }

    [Authorize]
    public ActionResult Create()
    {
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Tag", "Description"); //dont think we need ', "Description"'
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Recipe recipe, int CategoryId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      recipe.User = currentUser;
      _db.Recipe.Add(recipe);
      _db.SaveChanges();
      if (CategoryId != 0)
      {
        _db.CategoryRecipe.Add(new CategoryRecipe() { CategoryId = CategoryId, RecipeId = recipe.RecipeId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Recipe thisRecipe = _db.Recipe
        .Include(Recipe => recipe.JoinEntities)
        .ThenInclude(join => join.Category)
        .Include(recipe => recipe.User)
        .FirstOrDefault(recipe => recipe.RecipeId == id);
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ViewBag.IsCurrentUser = userId != null ? userId == thisRecipe.User.Id : false;
      return View(thisRecipe);
    }

    public ActionResult Edit(int id)
    {
      var thisRecipe = _db.Recipes.FirstOrDefault(Recipe => Recipe.RecipeId == id);
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View(thisRecipe);
    }

    [HttpPost]
    public ActionResult Edit(Recipe Recipe, int CategoryId)
    {
      if (CategoryId != 0)
      //Notice that we again use a conditional in the case that no Categories yet exist or are being used.
      {
        _db.CategoryRecipe.Add(new CategoryRecipe()
        {
          CategoryId = categoryId,
          RecipeId = Recipe.RecipeId
        });
      }
      _db.Entry(Recipe).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddCategory(int id)
    {
      var thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View(thisRecipe);
    }

    [HttpPost]
    public ActionResult AddCategory(Recipe Recipe, int CategoryId)
    {
      if (CategoryId != 0)
      {
        _db.CategoryRecipe.Add(new CategoryRecipe() { CategoryId = CategoryId, RecipeId = recipe.RecipeId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
      return View(thisRecipe);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
      _db.Recipes.Remove(thisRecipe);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteCategory(int joinId)
    {
      var joinEntry = _db.CategoryRecipe.FirstOrDefault(entry => entry.CategoryRecipeId == joinId);
      _db.CategoryRecipe.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}