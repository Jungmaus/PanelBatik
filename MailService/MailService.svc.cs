using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace MailService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class MailService : IMail
    {
        public string GetPassword()
        {
            string[] harfList = { "a", "A", "c", "X", "C", "x", "z", "Z", "u" };
            string psw = string.Empty;
            Random rndm = new Random();

            for(int i = 0; i < 8; i++)
            {
                if(i % 2 == 0)
                {
                    int sayi = rndm.Next(1, 9);
                    psw += Convert.ToInt32(sayi);
                }
                else
                {
                    psw += harfList[i];
                }
            }

            return psw;
        }

        public async void SendMail(string uri, string mail)
        {
            var body = "<label style='color:darkgreen;'>Bu mail, sizin şifre yenileme istediğiniz üzerine gönderilmiştir. Şifrenizi bu link üzerinden değiştirebilirsiniz. BATIK</label>";
            body += "<hr> <br>";
            body += "Mail Adresiniz: " + mail + "<br>";
            body += "Şifre Değiştirme Adresi: " + "<a href='" + uri + "'> Tıkla </a> </b> <br> <br>";
            body += "Bu adres ile şifrenizi değiştirebilirsiniz. Teşekkürler.";
            var message = new MailMessage();
            message.To.Add(new MailAddress(mail));
            message.From = new MailAddress("jungmausoftware@gmail.com");
            message.Subject = "Şifre Yenileme | BATIK";
            message.Body = body;
            message.IsBodyHtml = true;


            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "email@gmail.com",
                    Password = "password"
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }

        }
    }
}
