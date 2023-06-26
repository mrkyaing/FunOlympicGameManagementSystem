using System.ComponentModel.DataAnnotations;

namespace FunOlympicGameManagementSystem.Models {
    public abstract class BaseEntity {
        [Key]
        public string Id { get; set; }
        public DateTime CreatedOn { get; set; }= DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
