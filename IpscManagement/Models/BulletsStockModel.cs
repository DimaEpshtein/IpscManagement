using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IpscManagement.Models
{
    public class BulletsStockModel
    {
        public int Identity { get; set; }
        public int ShooterIdentity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Amount { get; set; }
    }
}