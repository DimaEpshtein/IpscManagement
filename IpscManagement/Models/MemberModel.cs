using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IpscManagement.Models
{
    public class MemberModel
    {
        public int Id { get; set; }
        public int Identity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public DateTime DateofBirth { get; set; }
        public string DateofBirthStr { get; set; }
        public string Gender { get; set; }
        public int? ArmyId { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public int ShooterIdentity { get; set; }
        public bool Active { get; set; }

    }
}