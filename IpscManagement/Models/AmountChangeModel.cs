using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IpscManagement.Models
{
    public class AmountChangeModel
    {
        public int MemberIdentity { get; set; }
        public int Amount { get; set; }
        public int AmountUpdateType { get; set; }
        public string Remarks { get; set; }
    }
}