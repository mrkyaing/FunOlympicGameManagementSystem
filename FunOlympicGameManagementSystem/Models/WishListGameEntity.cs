using System.ComponentModel.DataAnnotations.Schema;

namespace FunOlympicGameManagementSystem.Models {
    [Table("WishListGame")]
    public class WishListGameEntity:BaseEntity {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
