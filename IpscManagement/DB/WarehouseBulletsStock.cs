//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IpscManagement.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class WarehouseBulletsStock
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public int Amount { get; set; }
        public int AmmoType { get; set; }
        public System.DateTime DateTime { get; set; }
    
        public virtual Warehouse Warehouse { get; set; }
        public virtual AmmoType AmmoType1 { get; set; }
    }
}