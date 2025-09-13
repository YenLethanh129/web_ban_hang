using Dashboard.Winform.ViewModels;

namespace Dashboard.Winform.Events
{
    public class RecipesLoadedEventArgs : EventArgs
    {
        public List<RecipeViewModel> Recipes { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
