using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicToBusPlanner
{
    class ContactRepository
    {
        private readonly Dictionary<int, Contact> _allContacts = new Dictionary<int, Contact>();
        private readonly Dictionary<int, List<int>> _byiStudentID = new Dictionary<int, List<int>>();

        public ContactRepository()
        {
            using (SqlConnection connection = new SqlConnection(Settings.dbConnectionString_SchoolLogic))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "SELECT " +
                                                "StudentStatus.iStudentID, " +
                                                "ContactRelation.iContactID, " +
                                                "ContactRelation.lLivesWithStudent, " +
                                                "LookupValues.cName AS Relation, " +
                                                "Contact.cLastName, " +
                                                "Contact.cFirstName, " +
                                                "Contact.mEmail, " +
                                                "Contact.cBusPhone, " +
                                                "Contact.mCellphone, " +
                                                "Location.cPhone " +
                                            "FROM " +
                                                "LookupValues " +
                                                "RIGHT OUTER JOIN ContactRelation ON LookupValues.iLookupValuesID = ContactRelation.iLV_RelationID " +
                                                "LEFT OUTER JOIN Contact " +
                                                "LEFT OUTER JOIN Location ON Contact.iLocationID = Location.iLocationID ON ContactRelation.iContactID = Contact.iContactID " +
                                                "RIGHT OUTER JOIN StudentStatus ON ContactRelation.iStudentID = StudentStatus.iStudentID " +
                                            "WHERE " +
                                                "(StudentStatus.lOutsideStatus = 0) AND (StudentStatus.dInDate <= GETDATE()) AND (StudentStatus.dOutDate >= GETDATE() OR " +
                                                "StudentStatus.dOutDate < CONVERT(DATETIME, '1901-01-01 00:00:00', 102)) AND (ContactRelation.lLivesWithStudent = 1) AND ((ContactRelation.iContactPriority = 1) OR (ContactRelation.iContactPriority = 2)) " +
                                            "ORDER BY ContactRelation.iContactPriority "; 

                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            int studentID = Parsers.ParseInt(dataReader["iStudentId"].ToString().Trim());
                            int contactID = Parsers.ParseInt(dataReader["iContactID"].ToString().Trim());

                            if (!_allContacts.ContainsKey(contactID))
                            {
                                Contact c = dataReaderToContact(dataReader);
                                _allContacts.Add(contactID, c);
                            }                                                            

                            if (!_byiStudentID.ContainsKey(studentID))
                            {
                                _byiStudentID.Add(studentID, new List<int>());
                            }
                            _byiStudentID[studentID].Add(contactID);
                            
                        }
                    }
                    sqlCommand.Connection.Close();
                }
            }
        }

        private Contact dataReaderToContact(SqlDataReader dataReader)
        {
            return new Contact()
            {
                ID = Parsers.ParseInt(dataReader["iContactID"].ToString().Trim()),
                Relation = dataReader["Relation"].ToString().Trim(),
                FirstName = dataReader["cFirstName"].ToString().Trim(),
                LastName = dataReader["cLastName"].ToString().Trim(),
                Phone_Home = dataReader["cPhone"].ToString().Trim(),
                Phone_Business = dataReader["cBusPhone"].ToString().Trim(),
                Phone_Cell = dataReader["mCellPhone"].ToString().Trim(),
                Email = dataReader["mEmail"].ToString().Trim(),
            };
        }

        public Contact Get(int id)
        {
            if (_allContacts.ContainsKey(id))
            {
                return _allContacts[id];
            } else
            {
                return null;
            }
        }

        public List<Contact> GetForStudent(Student student)
        {
            return GetForStudent(student.iStudentID);
        }

        public List<Contact> GetForStudent(int iStudentID)
        {
            if (_byiStudentID.ContainsKey(iStudentID))
            {
                List<Contact> returnMe = new List<Contact>();

                foreach (int id in _byiStudentID[iStudentID])
                {
                    Contact c = Get(id);
                    if (c != null)
                    {
                        returnMe.Add(c);
                    }
                }

                return returnMe;

            } else
            {
                return new List<Contact>();
            }
        }

        private Contact NullContact()
        {
            return new Contact()
            {

            };
        }

    }
}
