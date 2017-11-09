
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicToBusPlanner
{
    public class LookupValue
    {
        public int iLookupValuesID { get; set; }
        public string cName { get; set; }
        public string cCode { get; set; }
        public int iDataTypesID { get; set; }
        public int iSchoolID { get; set; }
        public bool lInactive { get; set; }
        public string mComment { get; set; }
        
        public override string ToString()
        {
            return this.cName;
        }
    }
}
