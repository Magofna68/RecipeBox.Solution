using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RecipeBox.Models
{
  public class RecipeBoxContext : IdentityDbContext<ApplicationUser>
  {
    public virtual DbSet<Category> Categories { get; set; }
    public DbSet<Recipe> Recipies { get; set; }
    public DbSet<CategoryRecipe> CategoryRecipe { get; set; }
    //Each DbSet we've included will become a table in our database. We've previously declared both Categories and Items DbSets, but now include a CategoryItem DbSet as well that represents our join table.

    public ToDoListContext(DbContextOptions options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { //Notice that we add the OnConfiguring method to enable lazy-loading.
      optionsBuilder.UseLazyLoadingProxies();
    }
  }
}