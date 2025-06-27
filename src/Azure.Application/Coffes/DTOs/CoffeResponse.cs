namespace Azure.Application.Coffes.DTOs
{
    public class CoffeResponse
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; }
        public string? Imagen { get; set; }
        public ICollection<IngredientRespose> Ingredients { get; set; } = [];
    }

    public class IngredientRespose
    {
        public string Name { get; set; }

    }
}
