using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IpscManagement.Models
{
    public class BulletsStockHistoryModel
    {
        public DateTime DateTime { get; set; }
        public string ActionType { get; set; }
        public int Amount { get; set; }
        public int PreviousAmmount { get; set; }
        public int NewAmmount { get; set; }
        public string Remarks { get; set; }
    }
}