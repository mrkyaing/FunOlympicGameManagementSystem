using System.Net.Mail;
using System.Net;
using FunOlympicGameManagementSystem.Models;

namespace FunOlympicGameManagementSystem.Utility {
    public  static class OtpHelper {
        public static void SendOtpToUserEmail(string emailId, string activationCode,string subject,string body) {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("funolympicgame101@gmail.com");
            mailMessage.To.Add(emailId);
            mailMessage.Subject = $"{subject} is completed";
            mailMessage.Body = $"Your {subject.ToLower()} is completed succesfully.Please use  this OTP {activationCode} to complete {body}.";

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;

            smtpClient.Credentials = new NetworkCredential("funolympicgame101@gmail.com", "asbgmiilyrzomjph");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;

            try {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email Sent Successfully.");
            }
            catch (Exception ex) {
                Console.WriteLine("Error: " + ex.Message);
            }
           
        }
        public static OTPEntity OtpCreateWithEmail(string emailId, string otp) {
            OTPEntity oTPEntity = new OTPEntity()
            {
                Id=Guid.NewGuid().ToString(),
                EmailId = emailId,
                OTP=otp
            };
            return oTPEntity;
        }
        public  static string GetRandom6Digit() {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            return r;
        }
    }
}
