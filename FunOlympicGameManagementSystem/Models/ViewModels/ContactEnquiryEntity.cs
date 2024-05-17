using System.ComponentModel.DataAnnotations.Schema;

namespace FunOlympicGameManagementSystem.Models.ViewModels
{
    [Table("ContactEnquiry")]
    public class ContactEnquiryEntity:BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Message { get; set; }
    }
}
