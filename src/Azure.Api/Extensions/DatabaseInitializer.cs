using Azure.Api.Resources;
using Azure.Domain;
using Azure.Persistence;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
            if(context.Coffes.Any() )
            {
                return; // Data already seeded
            }
            var rootPath = env?.ContentRootPath ??throw new Exception("El environment no se cargo");
            var fullPathCoffe = Path.Combine(rootPath, "Resources/coffe.json");
            var data = await File.ReadAllTextAsync(fullPathCoffe);
            var coffees = JsonConvert.DeserializeObject<List<CoffeJson>>(data)??Enumerable.Empty<CoffeJson>();

          var coffesEntities=  coffees.Select(json => new Coffe
            {
                Id = json.CoffeId,
                Name = json.Title!,
                Description = json.Description,
                Price = 10,
               Imagen=json.Image
            }).ToArray();

            await context.Coffes.AddRangeAsync(coffesEntities);
            await context.SaveChangesAsync();
        }
    }
}
