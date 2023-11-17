using eticaret.business.Abstract.Service;
using eticaret.business.Operations.OperationEntities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Concrete.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> SendEmailAsHtml(string email, string subject, string htmlContent)
        {
            SmtpClient client = new SmtpClient(SmtpConfig.Host, SmtpConfig.Port)
            {
                Host = SmtpConfig.Host,
                Port = SmtpConfig.Port,
                EnableSsl = SmtpConfig.EnableSSL,
                Credentials = new NetworkCredential(SmtpConfig.UserName, SmtpConfig.Password),
            };

            var sender = new MailAddress("arite.com.tr@gmail.com", "Arite");
            var reciver = new MailAddress(email);
            var mailMessage = new MailMessage(sender, reciver) { IsBodyHtml = true, Sender = sender, Subject = subject, Body = htmlContent };
            await client.SendMailAsync(mailMessage);
            return true;
        }

        public async Task<bool> SendEmailAsync
            (string email, string subject, string url)
        {
            SmtpClient client = new SmtpClient(SmtpConfig.Host, SmtpConfig.Port)
            {
                Host = SmtpConfig.Host,
                Port = SmtpConfig.Port,
                EnableSsl = SmtpConfig.EnableSSL,
                Credentials = new NetworkCredential(SmtpConfig.UserName, SmtpConfig.Password),
            };

            string htmlContent = SetHtmlContent(_configuration["Domain"] + url, subject);
            await client.SendMailAsync(new MailMessage(
                                       SmtpConfig.UserName,
                                       email, subject,
                                       htmlContent)
                                       { IsBodyHtml = true });
            return true;
        }
        private Email SmtpConfig { 
            get 
            {
                Email email = new()
                {
                    EnableSSL = bool.Parse(_configuration["SmtpProfile:EnableSSL"]),
                    Host = _configuration["SmtpProfile:Host"],
                    Password = _configuration["SmtpProfile:Password"],
                    Port = int.Parse(_configuration["SmtpProfile:Port"]),
                    UserName = _configuration["SmtpProfile:UserName"]
                };
                return email;
            }
        }

        private string SetHtmlContent(string url, string subject)
        {
            return "<!DOCTYPE html><html><head> <meta charset='utf-8'> <meta http-equiv='x-ua-compatible' content='ie=edge'> <title>Email Confirmation</title> <meta name='viewport' content='width=device-width, initial-scale=1'> <style type='text/css'> /** * Google webfonts. Recommended to include the .woff version for cross-client compatibility. */ @media screen|{@font-face{font-family: 'Source Sans Pro'; font-style: normal; font-weight: 400; src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff');}@font-face{font-family: 'Source Sans Pro'; font-style: normal; font-weight: 700; src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff');}}div[style*='margin: 16px 0;']{margin: 0 !important;}body{width: 100% !important; height: 100% !important; padding: 0 !important; margin: 0 !important;}/** * Collapse table borders to avoid space between cells. */ table{border-collapse: collapse !important;}a{color: #1a82e2;}img{height: auto; line-height: 100%; text-decoration: none; border: 0; outline: none;}</style></head><body style='background-color: #e9ecef;'> <table border='0' cellpadding='0' cellspacing='0' width='100%'> <tr> <td align='center' bgcolor='#e9ecef'><!--[if (gte mso 9)|(IE)]> <table align='center' border='0' cellpadding='0' cellspacing='0' width='600'> <tr> <td align='center' valign='top' width='600'><![endif]--> <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'> <tr> <td align='center' valign='top' style='padding: 36px 24px;'> <a href='https://www.blogdesire.com' target='_blank' style='display: inline-block;'> </a> </td></tr></table><!--[if (gte mso 9)|(IE)]> </td></tr></table><![endif]--> </td></tr><tr> <td align='center' bgcolor='#e9ecef'> <img src='https://gokust.blob.core.windows.net/product-images/logo_transparent.png' alt='Logo' border='0' width='48' style='display: block; width: 125px; max-width: 125px; min-width: 48px;margin-bottom:.5rem'> </a><!--[if (gte mso 9)|(IE)]> <table align='center' border='0' cellpadding='0' cellspacing='0' width='600'> <tr> <td align='center' valign='top' width='600'><![endif]--> <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'> <tr> <td align='left' bgcolor='#ffffff' style='padding: 36px 24px 0; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; border-top: 3px solid #d4dadf;'> <h1 style='margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;'>"+ subject +"</h1> </td></tr></table><!--[if (gte mso 9)|(IE)]> </td></tr></table><![endif]--> </td></tr><tr> <td align='center' bgcolor='#e9ecef'><!--[if (gte mso 9)|(IE)]> <table align='center' border='0' cellpadding='0' cellspacing='0' width='600'> <tr> <td align='center' valign='top' width='600'><![endif]--> <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'> <tr> <td align='left' bgcolor='#ffffff' style='padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;'> <p style='margin: 0;'>Aşağıdaki butona tıklayarak hesabınızı onaylayabilirsiniz.Eğer hesabı siz oluşturmadıysanız <a href=''>buraya</a> tıklayarak hesabı silebilirsiniz</p></td></tr><tr> <td align='left' bgcolor='#ffffff'> <table border='0' cellpadding='0' cellspacing='0' width='100%'> <tr> <td align='center' bgcolor='#ffffff' style='padding: 12px;'> <table border='0' cellpadding='0' cellspacing='0'> <tr> <td align='center' bgcolor='#1a82e2' style='border-radius: 6px;'> <a href='"+ url + "' target='_blank' style='display: inline-block; padding: 16px 36px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;'>" + subject +"</a> </td></tr></table> </td></tr></table> </td></tr><tr> <td align='left' bgcolor='#ffffff' style='padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;'> <p style='margin: 0;'>Eğer buton çalışmadıysa aşağıdaki linki kopyalayıp tarayıcınızda açın</p><p style='margin: 0;'><a href='" + url + "' target='_blank'>"+url+"</a></p></td></tr><tr> <td align='left' bgcolor='#ffffff' style='padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf'> <p style='margin: 0;'>İyi Günler, Arite<br></td></tr></table><!--[if (gte mso 9)|(IE)]> </td></tr></table><![endif]--> </td></tr><tr> <td align='center' bgcolor='#e9ecef' style='padding: 24px;'><!--[if (gte mso 9)|(IE)]> <table align='center' border='0' cellpadding='0' cellspacing='0' width='600'> <tr> <td align='center' valign='top' width='600'><![endif]--> <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'> <tr> <td align='center' bgcolor='#e9ecef' style='padding: 12px 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; color: #666;'> </td></tr><tr> <td align='center' bgcolor='#e9ecef' style='padding: 12px 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; color: #666;'> <p style='margin: 0;'>Bu e-postadan rahatsız olduysasız üyeliğinizi <a href='' target='_blank'>sonlandırabilirsiniz</a></p></td></tr></table><!--[if (gte mso 9)|(IE)]> </td></tr></table><![endif]--> </td></tr></table> </body></html>";
        }


    }
}
