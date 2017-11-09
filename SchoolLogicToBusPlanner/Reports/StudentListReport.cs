using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicToBusPlanner
{
    class StudentListReport
    {
        public MemoryStream Generate()
        {
            MemoryStream csvFile = new MemoryStream();
            StreamWriter writer = new StreamWriter(csvFile, Encoding.UTF8);

            //char separator = ',';
            char separator = '\t';

            int maxContactsToDisplay = 2;

            // Headings

            StringBuilder headingLine = new StringBuilder();
            headingLine.Append("\"SchoolDAN\"" + separator);
            headingLine.Append("\"StudentNumber\"" + separator);
            headingLine.Append("\"FirstName\"" + separator);
            headingLine.Append("\"LastName\"" + separator);
            headingLine.Append("\"BirthDate\"" + separator);
            headingLine.Append("\"Gender\"" + separator);
            headingLine.Append("\"Grade\"" + separator);
            headingLine.Append("\"Phone\"" + separator);
            headingLine.Append("\"House\"" + separator);
            headingLine.Append("\"Street\"" + separator);
            headingLine.Append("\"City\"" + separator);
            headingLine.Append("\"Province\"" + separator);
            headingLine.Append("\"Country\"" + separator);
            headingLine.Append("\"PostalCode\"" + separator);
            headingLine.Append("\"ReserveName\"" + separator);
            headingLine.Append("\"ReserveHouse\"" + separator);
            headingLine.Append("\"LandLocation\"" + separator);
            headingLine.Append("\"Quarter\"" + separator);
            headingLine.Append("\"Section\"" + separator);
            headingLine.Append("\"Township\"" + separator);
            headingLine.Append("\"Range\"" + separator);
            headingLine.Append("\"Meridian\"" + separator);
            headingLine.Append("\"RiverLot\"" + separator);

            for(int x = 1;x<=maxContactsToDisplay;x++)
            {
                headingLine.Append("\"Contact" + x + "ID\"" + separator);
                headingLine.Append("\"Contact" + x + "FirstName\"" + separator);
                headingLine.Append("\"Contact" + x + "LastName\"" + separator);
                headingLine.Append("\"Contact" + x + "Relation\"" + separator);
                headingLine.Append("\"Contact" + x + "HomePhone\"" + separator);
                headingLine.Append("\"Contact" + x + "WorkPhone\"" + separator);
                headingLine.Append("\"Contact" + x + "CellPhone\"" + separator);
                headingLine.Append("\"Contact" + x + "Email\"" + separator);
            }

            writer.WriteLine(headingLine.ToString());

            

            StudentRepository repo = new StudentRepository();

            foreach (Student student in repo.GetAll())
            {
                StringBuilder csvLine = new StringBuilder();
                csvLine.Append("\"" + student.BaseSchoolDAN); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.StudentNumber); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.FirstName); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.LastName); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.DateOfBirth.ToShortDateString()); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.Gender); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.GradeFormatted); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.PhoneNumber); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.Address.HouseNumber); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.Address.Street); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.Address.City); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.Address.Province); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.Address.Country); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.Address.PostalCode); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.ReserveAddress.Name); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.ReserveAddress.HouseNumber); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.LandDescription); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.LandDescription.Quarter); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.LandDescription.Section); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.LandDescription.Township); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.LandDescription.Range); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.LandDescription.Meridian); csvLine.Append("\"" + separator);
                csvLine.Append("\"" + student.LandDescription.RiverLot); csvLine.Append("\"" + separator);

                // Figure out how many contacts we should display
                int contactsToDisplay = student.Contacts.Count;
                if (contactsToDisplay > maxContactsToDisplay)
                {
                    contactsToDisplay = maxContactsToDisplay;
                }

                if (contactsToDisplay > 0)
                {
                    for(int x = 0; x < contactsToDisplay;x++)
                    {
                        Contact c = student.Contacts[x];
                        if (c != null)
                        {
                            csvLine.Append("\"" + c.ID +"\"" + separator);
                            csvLine.Append("\"" + c.FirstName +"\"" + separator);
                            csvLine.Append("\"" + c.LastName +"\"" + separator);
                            csvLine.Append("\"" + c.Relation +"\"" + separator);
                            csvLine.Append("\"" + c.Phone_Home + "\"" + separator);
                            csvLine.Append("\"" + c.Phone_Business +"\"" + separator);
                            csvLine.Append("\"" + c.Phone_Cell +"\"" + separator);
                            csvLine.Append("\"" + c.Email +"\"" + separator);
                        }
                    }
                }

                if (csvLine.Length > 0)
                {
                    writer.WriteLine(csvLine.ToString());
                }
            }

            writer.Flush();
            csvFile.Flush();
            return csvFile;
        }
    }
}
