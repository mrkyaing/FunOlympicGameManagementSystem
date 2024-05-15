namespace FunOlympicGameManagementSystem.Models
{
    public class CountryEntity: BaseEntity
    {
        public string Name { get; set; }

        public static List<CountryEntity> GetAllCities()
        {
            return new List<CountryEntity> { 
             new CountryEntity() { Name="Myanmar"}, 
             new CountryEntity {Name="Thailand" }, 
             new CountryEntity { Name = "Singapore" } ,
             new CountryEntity { Name = "Cambodia" },
             new CountryEntity { Name = "Malaysia" },
             new CountryEntity { Name = "Brunei Darussalam" },
             new CountryEntity { Name = "Philippines" },
             new CountryEntity { Name = "Vietnam " }
            };
        }
    }
}
