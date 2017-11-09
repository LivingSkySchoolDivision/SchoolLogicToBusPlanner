using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLogicToBusPlanner
{
    public class LookupValueRepository
    {
        private Dictionary<int, LookupValue> _cachedLookupValues;

        public LookupValueRepository()
        {
            _cachedLookupValues = new Dictionary<int, LookupValue>();

            using (SqlConnection connection = new SqlConnection(Settings.dbConnectionString_SchoolLogic))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "SELECT * FROM LookupValues";
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            _cachedLookupValues.Add(Parsers.ParseInt(dataReader["iLookupValuesID"].ToString().Trim()), dataReaderToLookupValue(dataReader));
                        }
                    }
                    sqlCommand.Connection.Close();
                }
            }
        }

        private LookupValue CreateDummyLookupValue()
        {
            return new LookupValue()
            {
                cCode = string.Empty,
                cName = string.Empty,
                iDataTypesID = 0,
                iLookupValuesID = 0,
                iSchoolID = 0,
                lInactive = true,
                mComment = string.Empty
            };
        }

        public LookupValue Get(int iLookupValueID)
        {
            return _cachedLookupValues.ContainsKey(iLookupValueID) ? _cachedLookupValues[iLookupValueID] : CreateDummyLookupValue();
        }

        public List<LookupValue> GetAll()
        {
            return _cachedLookupValues.Values.ToList<LookupValue>();
        }

        public string GetValue(string iLookupValueID)
        {
            return this.GetValue(Parsers.ParseInt(iLookupValueID));
        }

        public string GetValue(int iLookupValueID)
        {
            return _cachedLookupValues.ContainsKey(iLookupValueID) ? _cachedLookupValues[iLookupValueID].cName : string.Empty;
        }

        public string GetCode(string iLookupValueID)
        {
            return this.GetCode(Parsers.ParseInt(iLookupValueID));
        }

        public string GetCode(int iLookupValueID)
        {
            return _cachedLookupValues.ContainsKey(iLookupValueID) ? _cachedLookupValues[iLookupValueID].cCode : string.Empty;
        }

        public List<LookupValue> GetAllForDataType(int dataTypeID)
        {
            return _cachedLookupValues.Values.Where(l => l.iDataTypesID == dataTypeID).ToList();
        }

        private LookupValue dataReaderToLookupValue(SqlDataReader dataReader)
        {
            return new LookupValue()
            {
                iLookupValuesID = Parsers.ParseInt(dataReader["iLookupValuesID"].ToString().Trim()),
                cName = dataReader["cName"].ToString().Trim(),
                cCode = dataReader["cCode"].ToString().Trim(),
                mComment = dataReader["mComment"].ToString().Trim(),
                iDataTypesID = Parsers.ParseInt(dataReader["iDataTypesID"].ToString().Trim()),
                iSchoolID = Parsers.ParseInt(dataReader["iSchoolID"].ToString().Trim()),
                lInactive = Parsers.ParseBool(dataReader["lInactive"].ToString().Trim())
            };
        }

    }
}
