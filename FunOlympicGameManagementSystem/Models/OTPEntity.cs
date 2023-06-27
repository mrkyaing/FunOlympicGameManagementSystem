using System.ComponentModel.DataAnnotations.Schema;

namespace FunOlympicGameManagementSystem.Models {
    [Table("OTP")]
    public class OTPEntity:BaseEntity {
        public string EmailId { get; set; }
        public string OTP { get; set; }
    }
}
