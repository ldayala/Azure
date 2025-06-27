
using Azure.Domain;

namespace Azure.Application.Coffes.DTOs
{
   public  class CoffeCreateRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public required string Imagen { get; set; }


        
    }
}
