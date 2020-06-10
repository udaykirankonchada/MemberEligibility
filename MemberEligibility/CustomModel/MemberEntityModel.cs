using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberEligibility.CustomModel
{
    public class MemberEntityModel
    {
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public int? TechnologyID { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Qualification { get; set; }
        public decimal YearsOfExperience { get; set; }
        public string DOB { get; set; }
        public string Technology { get; set; }
    }
}