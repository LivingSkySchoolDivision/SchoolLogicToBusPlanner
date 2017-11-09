using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicToBusPlanner
{
    class Contact
    {
        public int ID { get; set; }        
        public string Relation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string Phone_Home { get; set; }
        public string Phone_Business { get; set; }
        public string Phone_Cell { get; set; }
        public string Email { get; set; }

        public Contact()
        {
            
        }
    }
}
