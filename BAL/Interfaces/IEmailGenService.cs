using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public interface IEmailGenService{
    void generateEmail(HttpRequest req,string email);
    void Send(string from, string to, string subject, string html);

    void emailForForgetPass(HttpRequest req,string email,string password);
} 
public class EmailGenService : IEmailGenService{
    public void generateEmail(HttpRequest req, string email){
       
        string baseUrl = $"{req.Scheme}://{req.Host}";
        string rstlink = $"{baseUrl}/Login/ResetPass?email={email}";
        string emailBody = System.IO.File.ReadAllText("C:/Users/pct78/pizzashop_N_tier/pizzashop_n_tier/Views/Login/emailbody.cshtml");
        emailBody = emailBody.Replace("{{reset_Link}}", rstlink);
        Send("test.dotnet@etatvasoft.com",email,"sending email",emailBody);
        Console.WriteLine("sended successfully on : "+rstlink);
    }

    public void emailForForgetPass(HttpRequest req,string email,string password){
        string emailBody = System.IO.File.ReadAllText("C:/Users/pct78/pizzashop_N_tier/pizzashop_n_tier/Views/Dashboard/emailbody.cshtml");
        emailBody = emailBody.Replace("{{useremail}}",email);
        emailBody = emailBody.Replace("{{userpassword}}",password);
        Send("test.dotnet@etatvasoft.com",email,"sending email",emailBody);
    }
    public void Send(string from, string to, string subject, string html)
    {
        using (SmtpClient smtpClient = new SmtpClient("mail.etatvasoft.com", 587))
            {
                using (MailMessage message = new MailMessage())
                {
                    message.To.Add(new MailAddress(to));
                    message.From = new MailAddress(from, "dhruvil");
                    message.Subject = subject;
                    message.Body = html;
                    message.IsBodyHtml = true;
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential("test.dotnet@etatvasoft.com", "P}N^{z-]7Ilp");
                    smtpClient.Send(message); 
                }
            }

    }
}