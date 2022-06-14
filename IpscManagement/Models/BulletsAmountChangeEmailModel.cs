using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace IpscManagement.Models
{
    public class BulletsAmountChangeEmailModel: MailMessage
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PreviousAmount { get; set; }
        public int ChangeAmount { get; set; }
        public int NewAmount { get; set; }
        public string Remarks { get; set; }

    }
}