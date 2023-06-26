using System.ComponentModel.DataAnnotations.Schema;

namespace FunOlympicGameManagementSystem.Models {
    [Table("OTP")]
    public class OTPEntity:BaseEntity {
        public string UserId { get; set; }
        public string OTP { get; set; }
    }
}
