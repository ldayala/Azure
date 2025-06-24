namespace Azure.Api.Resources
{
    public class CoffeJson
    {
        public Guid CoffeId { get; set; } = Guid.NewGuid();
        public string? Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;
        public int Category { get; set; } 
        public string[] Ingredientes { get; set; } =[];
    }
}
