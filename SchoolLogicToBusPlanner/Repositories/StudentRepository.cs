using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicToBusPlanner
{
    class StudentRepository
    {
        LookupValueRepository _LVRepository = new LookupValueRepository();
        ContactRepository _contactRepository = new ContactRepository();
        private readonly Dictionary<string, Student> _studentsByID = new Dictionary<string, Student>();
        private readonly List<string> _duplicates = new List<string>();

        private const string SQL = "SELECT " +
                                         "Student.iStudentID, " +
                                         "Student.cStudentNumber, " +
                                         "Student.cFirstName, " +
                                         "Student.cLastName, " +
                                         "Student.dBirthdate, " +
                                         "Grades.cName AS cGrade, " +
                                         "Student.cUserName, " +
                                         "LookupValues_1.cCode AS cGender, " +
                                         "School.cName, " +
                                         "School.cCode AS cSchoolDAN, " +
                                         "Location.iLV_RegionID, " +
                                         "Country.cName AS Country, " +
                                         "LookupValues_2.cName AS Province, " +
                                         "LookupValues.cName AS City, " +
                                         "Location.cPostalCode, " +
                                         "Location.cApartment, " +
                                         "Location.cHouseNo, " +
                                         "Location.cStreet, " +
                                         "Location.cPhone, " +
                                         "Student.mCellPhone, " +
                                         "Student.iFamilyID, " +
                                         "UserStudent.UF_1651 AS Quarter, " +
                                         "UserStudent.UF_2098 AS Section, " +
                                         "UserStudent.UF_1653_1 AS Township, " +
                                         "UserStudent.UF_1654_1 AS Range, " +
                                         "UserStudent.UF_2093 AS Meridian, " +
                                         "UserStudent.UF_2096 AS RiverLot, " +
                                         "UserStudent.cReserveName, " +
                                         "UserStudent.cReserveHouse " +
                                    "FROM " +
                                         "LookupValues AS LookupValues_1 RIGHT OUTER JOIN " +
                                         "Student LEFT OUTER JOIN " +
                                         "UserStudent ON Student.iStudentID = UserStudent.iStudentID LEFT OUTER JOIN " +
                                         "School ON Student.iSchoolID = School.iSchoolID ON LookupValues_1.iLookupValuesID = Student.iLV_GenderID LEFT OUTER JOIN " +
                                         "Country RIGHT OUTER JOIN " +
                                         "Location ON Country.iCountryID = Location.iCountryID LEFT OUTER JOIN " +
                                         "LookupValues AS LookupValues_2 ON Location.iLV_RegionID = LookupValues_2.iLookupValuesID LEFT OUTER JOIN " +
                                         "LookupValues ON Location.iLV_CityID = LookupValues.iLookupValuesID ON Student.iLocationID = Location.iLocationID LEFT OUTER JOIN " +
                                         "Grades ON Student.iGradesID = Grades.iGradesID RIGHT OUTER JOIN " +
                                         "StudentStatus ON Student.iStudentID = StudentStatus.iStudentID " +
                                    "WHERE " +
                                         "(StudentStatus.lOutsideStatus = 0) " +
                                         "AND (Student.iTrackID <> 0) " +
                                         "AND ((StudentStatus.dOutDate >= GETDATE()) " +
                                         "OR (StudentStatus.dOutDate < CONVERT(DATETIME, '1901-01-01 00:00:00', 102)))";

        private Student dataReaderToStudent(SqlDataReader dataReader)
        {
            return new Student()
            {
                iStudentID = Parsers.ParseInt(dataReader["iStudentID"].ToString().Trim()),
                StudentNumber = dataReader["cStudentNumber"].ToString().Trim(),
                FirstName = dataReader["cFirstName"].ToString().Trim(),
                LastName = dataReader["cLastName"].ToString().Trim(),
                DateOfBirth = Parsers.ParseDate(dataReader["dBirthdate"].ToString().Trim()),
                Gender = dataReader["cGender"].ToString().Trim(),
                PhoneNumber = dataReader["cPhone"].ToString().Trim(),
                BaseSchoolDAN = dataReader["cSchoolDAN"].ToString().Trim(),
                Grade = dataReader["cGrade"].ToString().Trim(),
                FamilyID = Parsers.ParseInt(dataReader["iFamilyID"].ToString().Trim()),
                Address = new Address()
                {
                    UnitNumber = dataReader["cApartment"].ToString().Trim(),
                    HouseNumber = dataReader["cHouseNo"].ToString().Trim(),
                    Street = dataReader["cStreet"].ToString().Trim(),
                    City = dataReader["City"].ToString().Trim(),
                    Province = dataReader["Province"].ToString().Trim(),
                    PostalCode = dataReader["cPostalCode"].ToString().Trim(),
                    Country = dataReader["Country"].ToString().Trim(),
                },
                LandDescription = new LandDescription()
                {
                    Quarter = _LVRepository.GetValue(dataReader["Quarter"].ToString().Trim()),
                    Section = _LVRepository.GetValue(dataReader["Section"].ToString().Trim()),
                    Township = _LVRepository.GetValue(dataReader["Township"].ToString().Trim()),
                    Range = _LVRepository.GetValue(dataReader["Range"].ToString().Trim()),
                    Meridian = _LVRepository.GetValue(dataReader["Meridian"].ToString().Trim()),
                    RiverLot = _LVRepository.GetValue(dataReader["RiverLot"].ToString().Trim()),
                },
                ReserveAddress = new ReserveAddress()
                {
                    Name = dataReader["cReserveName"].ToString().Trim(),
                    HouseNumber = dataReader["cReserveHouse"].ToString().Trim(),
                },
                Contacts = _contactRepository.GetForStudent(Parsers.ParseInt(dataReader["iStudentID"].ToString().Trim()))
            };
        }

        public StudentRepository()
        {
            _studentsByID = new Dictionary<string, Student>();
            using (SqlConnection connection = new SqlConnection(Settings.dbConnectionString_SchoolLogic))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = SQL;
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            Student s = dataReaderToStudent(dataReader);
                            if (s != null)
                            {
                                if (!_studentsByID.ContainsKey(s.StudentNumber))
                                {
                                    _studentsByID.Add(s.StudentNumber, s);
                                }
                                else
                                {
                                    if (!_duplicates.Contains(s.StudentNumber))
                                    {
                                        _duplicates.Add(s.StudentNumber);
                                    }
                                }
                            }
                        }
                    }
                    sqlCommand.Connection.Close();
                }
            }
        }

        public List<string> GetDuplicates()
        {
            return _duplicates;
        }

        public List<Student> GetAll()
        {
            return _studentsByID.Values.OrderBy(s => s.DisplayNameLastNameFirst).ToList();
        }
    }
}
