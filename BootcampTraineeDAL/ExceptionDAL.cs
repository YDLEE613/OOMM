namespace BootcampTraineeDAL
{
    using Newtonsoft.Json;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;

    /// <summary>
    /// Description: This class manages database connection and CRUD on database tables especially regarding exception.
    /// </summary>
    public class ExceptionDAL
    {
        //string lConnectionString = "Server=DESKTOP-H52G7QL\\SQLEXPRESS;Database=OnshoreCapstone;Trusted_Connection=True;";
        private string lConnectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;

        /// <summary>
        /// Description: This method logs the exception occured in database.
        /// </summary>
        /// <param name="iException">Exception occured </param>
        public void CreateExceptionLog(Exception iException)
        {
            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_CreateExceptionLog", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameters for stored procedure
                        lComm.Parameters.AddWithValue("@parm_exception_msg", SqlDbType.VarChar).Value = iException.Message;
                        lComm.Parameters.AddWithValue("@parm_exception_type", SqlDbType.VarChar).Value = iException.GetType().ToString();
                        lComm.Parameters.AddWithValue("@parm_exception_source", SqlDbType.VarChar).Value = iException.Source;

                        // open connection
                        lConn.Open();
                        lComm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // log exception to a file
                this.WriteLogToFile(ex);
            }

        }

        /// <summary>
        /// Description: This method logs exception occured in a file while logging exception to database
        /// </summary>
        /// <param name="ex">Exception occured</param>
        private void WriteLogToFile(Exception iException)
        {
            // serialize exception into json with indentation
            string lJson = JsonConvert.SerializeObject(iException, Formatting.Indented);
            string path = @"C:\Users\Admin-2\source\repos\BootcampTrainee\BootcampTraineeDAL\ErrorLog\errors.txt";

            // write into file
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(lJson);

            writer.Close();
            writer.Dispose();
        }
    }
}
