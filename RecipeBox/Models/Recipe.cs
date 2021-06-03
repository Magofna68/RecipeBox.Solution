using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace RecipeBox.Models
{
  public class Recipe
  {
    public Recipe()
    {
      this.JoinEntities = new HashSet<CategoryRecipe>();
      RecipeCompleted = false;
    }
    public int RecipeId { get; set; }
    public string Name { get; set; }
    public int Rating { get; set; }
  public string Ingredients { get; set; }
  public string Instructions { get; set; }

  public bool RecipeCompleted { get; set; }
  public virtual ApplicationUser User { get; set; }
  public virtual ICollection<CategoryRecipe> JoinEntities { get; }
}
}