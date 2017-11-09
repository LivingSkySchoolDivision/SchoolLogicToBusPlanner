using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicToBusPlanner
{
    class Student
    {
        public int iStudentID { get; set; }
        public string StudentNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }      
        public string Grade { get; set; }
        public string BaseSchoolDAN { get; set; }
        public int FamilyID { get; set; }
        public Address Address { get; set; }
        public LandDescription LandDescription { get; set; }
        public ReserveAddress ReserveAddress { get; set; }

        public List<Contact> Contacts { get; set; }

        public string GradeFormatted
        {
            get
            {
                if (this.Grade.ToLower() == "0k")
                {
                    return "K";
                }
                else if (this.Grade.ToLower() == "k")
                {
                    return "K";
                }
                else if (this.Grade.ToLower() == "pk")
                {
                    return "PK";
                }

                return this.Grade;
            }
        }

        public string DisplayName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public string DisplayNameLastNameFirst
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

        public override string ToString()
        {
            return this.StudentNumber + " " + this.DisplayName;
        }

    }
}
