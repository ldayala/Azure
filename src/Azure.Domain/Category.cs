

using System.Diagnostics.CodeAnalysis;

namespace Azure.Domain
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Coffe> Coffes { get; set; } = [];
        //en el momento de pasarle esos parametros los va a setear a las properties de la case
        [SetsRequiredMembers] //para que esl constructor acepte los required members
        private Category(int id,string name) =>(Id,Name)= (id,name);

        public static Category Create(int id)
        {
          var categoryName=   (CategoryEnum)id;
          string categoryNameString = categoryName.ToString();

         return  new Category(id, categoryNameString);
        }

    }
}
