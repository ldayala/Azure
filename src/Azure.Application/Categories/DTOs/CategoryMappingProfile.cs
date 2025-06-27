
using Azure.Domain;
using Core.Mappy.Interfaces;

namespace Azure.Application.Categories.DTOs
{
    public class CategoryMappingProfile : IMappingProfile
    {
        public void Configure(IMapper mapper)
        {
            mapper.CreateMap<Category, CategoryResponse>(
                 config =>
                 {
                     config.Map(dest => dest.CategoryId, src => src.Id);
                     config.Map(dest => dest.NameTest, src => src.Name);
                 }
             );
        }
    }
}
