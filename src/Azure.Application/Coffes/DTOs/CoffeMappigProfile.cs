
using Azure.Domain;

using Core.Mappy.Interfaces;

namespace Azure.Application.Coffes.DTOs
{
    public class CoffeMappigProfile : IMappingProfile
    {
        public CoffeMappigProfile()
        {
        }
        ///private readonly AzureDbContext _dbContext;
        public void Configure(IMapper mapper)
        {
            mapper.CreateMap<Ingredient, IngredientRespose>(
                configure =>
                {
                    configure.Map(dest => dest.Name, src => src.Name);
                }
                );

            mapper.CreateMap<Coffe, CoffeResponse>(
                    config =>
                    {
                        config.Map(dest => dest.CategoryId, src => src.Category != null ? src.Category.Name : "");
                        config.Map(dest => dest.Ingredients, src => mapper.Map<List<IngredientRespose>>(src.Ingredients));
                    }
                );

            mapper.CreateMap<CoffeCreateRequest, Coffe>();

        }
    }
}
