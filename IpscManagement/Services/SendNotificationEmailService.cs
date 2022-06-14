using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using IpscManagement.Models;

namespace IpscManagement.Services
{
    public static class SendNotificationEmailService
    {
        public static void AmmountChange(BulletsAmountChangeEmailModel bulletsAmountChangeEmail)
        {
            bulletsAmountChangeEmail.Subject = "עדכון מלאי";
            bulletsAmountChangeEmail.Body = $"שלום {bulletsAmountChangeEmail.FirstName + " " + bulletsAmountChangeEmail.LastName}, יתרה קודמת {bulletsAmountChangeEmail.PreviousAmount} יתרה נוכחית {bulletsAmountChangeEmail.NewAmount} <br> ערעורים ייתקבלו עד 5 ימי עבודה מיום משלוח מייל" + "<br />";
            bulletsAmountChangeEmail.Body += (bulletsAmountChangeEmail.PreviousAmount >
                                             bulletsAmountChangeEmail.NewAmount)
                ? "משיכה " + (bulletsAmountChangeEmail.PreviousAmount - bulletsAmountChangeEmail.NewAmount)
                : "רכישה " + (bulletsAmountChangeEmail.NewAmount - bulletsAmountChangeEmail.PreviousAmount);
            bulletsAmountChangeEmail.Body += "<br />";
            bulletsAmountChangeEmail.Body += "הערות: " + bulletsAmountChangeEmail.Remarks;
            try
            {
                SendEmail(bulletsAmountChangeEmail);
            }
            catch (Exception)
            {
                
            }
            
        }

        private static void SendEmail(MailMessage emailModel)
        {
            var fromAddress = WebConfigurationManager.AppSettings["email"];
            string fromPassword = WebConfigurationManager.AppSettings["emailPassword"];

            var smtp = new SmtpClient();
            {
                smtp.UseDefaultCredentials = false;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                smtp.Timeout = 20000;
            }
            
            emailModel.From = new MailAddress(WebConfigurationManager.AppSettings["email"]);
            emailModel.IsBodyHtml = true;
            smtp.Send(emailModel);
        }
    }
}