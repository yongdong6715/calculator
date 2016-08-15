using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data; 

namespace Calculator.Utilities
{
    public class MySQLDatabaseAccess
    {
        // Define some properties
        private MySql.Data.MySqlClient.MySqlConnection conn;
        public bool onError = false; 
        /// <summary>
        /// MySQLDatabaseAccesss default constructor
        /// 
        /// Initiates connection to mysql database 
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="database"></param>
        public MySQLDatabaseAccess(string serverAddress, String username, String password, String database)
        {
            // check the inputs before attempting to connect
            if (serverAddress != "" && username != "" && password != "" && database != "")
            {
                try
                {
                    String connString = "server=" + serverAddress + ";uid=" + username + ";pwd=" + password + ";database=" + database + ";";
                    this.conn = new MySql.Data.MySqlClient.MySqlConnection();
                    this.conn.ConnectionString = connString;
                    this.conn.Open(); 
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    // log the error
                    ErrorLog.logErrorMessage(ex.Message + "; " + ex.Source + "; " + ex.StackTrace.ToString());
                    this.onError = true; 
                }
            }
           
        }
        /// <summary>
        /// Run mysql query 
        /// </summary>
        /// <param name="query">MySql query statement</param>
        /// <param name="parameters">This is optional, use it when needed</param>
        /// <returns>List containing the data</returns>
        public List<Object>runQuery(String query, Dictionary<String, String> parameters = null)
        {
            MySql.Data.MySqlClient.MySqlDataReader rs = null; // we use data reader to get multiple-row result set instead of scalar
            List<Object>output = new List<Object>();
            if (query != "")
            {
                try
                {
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, this.conn);
                    
                    // add parameters, if supplied
                    if (parameters.Count()> 0 || parameters != null)
                    {
                        cmd.Prepare();
                        List<KeyValuePair<String, String>>elements = parameters.ToList();
                        foreach (KeyValuePair<String,String> element in elements)
                        {
                            //String parameter = parameters.ElementAt()
                            cmd.Parameters.AddWithValue("@" + element.Key.ToString(),element.Value.ToString());
                        }

                    }
                    // execute the query 
                   rs = cmd.ExecuteReader();
                   if (!rs.HasRows)
                   {

                       return null;
                   }
                   else
                   {
                       while (rs.Read())
                       {
                           Object[] values = new Object[rs.FieldCount];
                           int fieldCount = rs.GetValues(values);
                           if (fieldCount > 0)
                           {
                               output.Add(values);
                           }
                       }
                       rs.Close();
                       conn.Close();
                   }
                  

                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    // log the error
                    ErrorLog.logErrorMessage(ex.Message + "; " + ex.Source + "; " + ex.StackTrace.ToString());
                    this.onError = true; 
                }

            }
            return output;
        }
       
    }
}
