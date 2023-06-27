using System.Text;

namespace FunOlympicGameManagementSystem.Utility {
    public static class EncryptPassword {
        public static string TextToEncrypt(string password) {
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}
