using Azure.Api.Resources;
using Azure.Domain;
using Azure.Persistence;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Azure.Api.Extensions
{
    /*la clase la hacemos estatico porque es una extension, una vez creada la extension la llamamos en el programcs*/
    public static class DatabaseInitializer
    {
        public static async Task ApplyMigration(
            this IApplicationBuilder builder,
            IWebHostEnvironment? env
            )
        {

            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var service = scope.ServiceProvider;
                var loggerFactory = service.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = service.GetRequiredService<AzureDbContext>();
                    await context.Database.MigrateAsync();
                    await SeedData(context, env);

                }
                catch (Exception e)
                {
                    var logger = loggerFactory.CreateLogger("Program");
                    logger.LogError(e, "An error occurred while applying migrations.");
                }
            }
        }

        private static async Task SeedData(AzureDbContext context, IWebHostEnvironment? env)
        {
            if (context.Coffes.Any())
            {
                return; // Data already seeded
            }
            var rootPath = env?.ContentRootPath ?? throw new Exception("El environment no se cargo");
            var fullPathCoffe = Path.Combine(rootPath, "Resources/coffe.json");
            var data = await File.ReadAllTextAsync(fullPathCoffe);
            var coffes = JsonConvert.DeserializeObject<List<CoffeJson>>(data) ?? Enumerable.Empty<CoffeJson>();

            var ingredientesMaster = new List<Ingredient>();
            var coffeMaster = new List<Coffe>();
            var random = new Random();

            foreach (var coffeJson in coffes)
            {
                var ingredientesLocal = new List<Ingredient>();

                foreach (var ingredientName in coffeJson.Ingredientes)
                {
                    var ingredient = ingredientesMaster.Where(s => s.Name.Equals(ingredientName)).FirstOrDefault();

                    if (ingredient is null)
                    {
                        ingredient = new Ingredient
                        {
                            Id = Guid.NewGuid(),
                            Name = ingredientName
                        };
                        ingredientesMaster.Add(ingredient);
                    }
                    ingredientesLocal.Add(ingredient);
                }
                var coffe = new Coffe
                {
                    Id = coffeJson.CoffeId,
                    Name = coffeJson.Title!,
                    Description = coffeJson.Description,
                    CategoryId = coffeJson.Category,
                    Imagen = coffeJson.Image,
                    Price = RandomPrice(random, 2, 15),
                    Ingredients = ingredientesLocal
                };    
                
                coffeMaster.Add(coffe);
            }



            await context.Coffes.AddRangeAsync(coffeMaster);
            await context.SaveChangesAsync();
        }

        private static decimal RandomPrice(Random random, double min, double max)
        {
            return (decimal)Math.Round(random.NextDouble() * Math.Abs(max - min) + min, 2);
        }
    }
}
