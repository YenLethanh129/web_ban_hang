using Dashboard.Winform.ViewModels;
using System;
using System.Collections.Generic;

namespace Dashboard.Winform.Events
{
    public class RecipesLoadedEventArgs : EventArgs
    {
        public List<RecipeViewModel> Recipes { get; set; } = new();
        public int TotalCount { get; set; }
    }

    public class RecipeSavedEventArgs : EventArgs
    {
        public RecipeDetailViewModel? Recipe { get; set; }
        public bool IsNewRecipe { get; set; }
    }

    public class RecipeDeletedEventArgs : EventArgs
    {
        public long RecipeId { get; set; }
        public string RecipeName { get; set; } = string.Empty;
    }

    public class RecipeIngredientChangedEventArgs : EventArgs
    {
        public long RecipeId { get; set; }
        public RecipeIngredientViewModel? Ingredient { get; set; }
        public string Action { get; set; } = string.Empty; // "Added", "Updated", "Deleted"
    }
}