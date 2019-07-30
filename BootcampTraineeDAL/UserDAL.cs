namespace BootcampTraineeDAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Configuration;
    using BootcampTraineeDBObjects;
    using BootcampTraineeDBObjects.SubDBO;

    /// <summary>
    /// Description: This class manages database connection and CRUD on database tables especially regarding users
    /// </summary>
    /// 
    public class UserDAL
    {
        // connection string to database
        //string lConnectionString = "Server=DESKTOP-H52G7QL\\SQLEXPRESS;Database=OnshoreCapstone;Trusted_Connection=True;";
        private string lConnectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;

        /// <summary>
        /// Description: This method inserts user into database
        /// </summary>
        /// <param name="iNewUser">User object with data</param>
        /// <returns>If successfully created, return UserID. Otherwise, 0</returns>
        public int CreateUser(UserDBO iNewUser)
        {
            // store number of rows affected in database
            int lResult = 0;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_CreateUser", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameters for stored procedure 
                        lComm.Parameters.AddWithValue("@parm_user_fname", SqlDbType.VarChar).Value = iNewUser.UserFirstName;
                        lComm.Parameters.AddWithValue("@parm_user_lname", SqlDbType.VarChar).Value = iNewUser.UserLastName;
                        lComm.Parameters.AddWithValue("@parm_user_login_id", SqlDbType.VarChar).Value = iNewUser.UserLogInID;
                        lComm.Parameters.AddWithValue("@parm_user_password", SqlDbType.VarChar).Value = iNewUser.UserPassword;
                        lComm.Parameters.AddWithValue("@parm_user_birth", SqlDbType.VarChar).Value = iNewUser.UserBirth;


                        // check if UserEmail is either null or empty.
                        if (string.IsNullOrEmpty(iNewUser.UserEmail))
                        {
                            lComm.Parameters.AddWithValue("@parm_user_email", SqlDbType.VarChar).Value = DBNull.Value;
                        }
                        else
                        {
                            lComm.Parameters.AddWithValue("@parm_user_email", SqlDbType.VarChar).Value = iNewUser.UserEmail;
                        }

                        // check if UserPhone is either null or empty.
                        if (String.IsNullOrEmpty(iNewUser.UserPhone))
                        {
                            lComm.Parameters.AddWithValue("@parm_user_phone", SqlDbType.VarChar).Value = DBNull.Value;
                        }
                        else
                        {
                            lComm.Parameters.AddWithValue("@parm_user_phone", SqlDbType.VarChar).Value = iNewUser.UserPhone;
                        }

                        // open connection
                        lConn.Open();

                        // get Inserted.UserID
                        lResult = Convert.ToInt32(lComm.ExecuteScalar());
                    }
                }

            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lException = new ExceptionDAL();
                lException.CreateExceptionLog(ex);
            }
            return lResult;
        }


        /// <summary>
        /// Description: This method retrieves all users from database
        /// </summary>
        /// <param name="iSearchString">string to be searched in the list</param>
        /// <returns>List of Users in database</returns>
        public List<UserDBO> GetAllUsers(string iSearchString)
        {
            // list of users to be returneed
            List<UserDBO> lUserList = new List<UserDBO>();

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllUsers", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // if there is no search string, pass null
                        if (!string.IsNullOrEmpty(iSearchString))
                        {
                            lComm.Parameters.AddWithValue("@parm_search_string", SqlDbType.VarChar).Value = iSearchString;
                        }
                        else
                        {
                            lComm.Parameters.AddWithValue("@parm_search_string", SqlDbType.VarChar).Value = DBNull.Value;
                        }

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about all users, and add each user to the list
                            while (lReader.Read())
                            {
                                // Instantiate a user 
                                UserDBO lUser = new UserDBO();

                                // set values
                                lUser.UserIDPK = (int)lReader["user_id"];
                                lUser.UserFirstName = (string)lReader["user_fname"];
                                lUser.UserLastName = (string)lReader["user_lname"];
                                lUser.UserLogInID = (string)lReader["user_login_id"];
                                lUser.UserPassword = (string)lReader["user_password"];
                                lUser.UserBirth = (DateTime)lReader["user_birth"];
                                lUser.UserRoleIDFK = (int)lReader["role_id_FK"];
                                lUser.UserRoleName = (string)lReader["role_name"];
                                lUser.UserDateCreated = (DateTime)lReader["date_created"];
                                lUser.UserDateModified = (DateTime)lReader["date_modified"];
                                lUser.UserIsActive = Convert.ToInt32(lReader["is_active"]);

                                // check null DBNull values
                                lUser.UserEmail = lReader["user_email"] == DBNull.Value ? "Unknown" : (string)lReader["user_email"];
                                lUser.UserPhone = lReader["user_phone"] == DBNull.Value ? "Unknown" : (string)lReader["user_phone"];

                                lUserList.Add(lUser);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lException = new ExceptionDAL();
                lException.CreateExceptionLog(ex);
            }

            return lUserList;
        }

        /// <summary>
        /// Description: This method finds a user with specifc id
        /// </summary>
        /// <param name="iUserIDPK"></param>
        /// <returns>User with the id</returns>
        public UserDBO FindUserByID(int iUserIDPK)
        {
            // User object to be returned
            UserDBO lUser = new UserDBO();
            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_FindUserByUserID", lConn))
                    {

                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_user_id", SqlDbType.Int).Value = iUserIDPK;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // Get data about user with the id
                            while (lReader.Read())
                            {
                                // set values
                                lUser.UserIDPK = (int)lReader["user_id"];
                                lUser.UserFirstName = (string)lReader["user_fname"];
                                lUser.UserLastName = (string)lReader["user_lname"];
                                lUser.UserLogInID = (string)lReader["user_login_id"];
                                lUser.UserPassword = (string)lReader["user_password"];
                                lUser.UserBirth = (DateTime)lReader["user_birth"];
                                lUser.UserIsActive = Convert.ToInt32(lReader["is_active"]);
                                lUser.UserDateCreated = (DateTime)lReader["date_created"];
                                lUser.UserDateModified = (DateTime)lReader["date_modified"];
                                lUser.UserRoleIDFK = (int)lReader["role_id_FK"];
                                lUser.UserRoleName = (string)lReader["role_name"];

                                // check null values
                                lUser.UserEmail = lReader["user_email"] == DBNull.Value ? "Unknown" : (string)lReader["user_email"];
                                lUser.UserPhone = lReader["user_phone"] == DBNull.Value ? "Unknown" : (string)lReader["user_phone"];

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle Exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lUser;
        }

        /// <summary>
        /// Description: This method updates data for a user by User id
        /// </summary>
        /// <param name="iUser">User with the data to be updated</param>
        /// <returns>If successfully updated, return true. Otherwise, false</returns>
        public bool UpdateUserByUserID(UserDBO iUser)
        {
            // return value
            bool lResult = false;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_UpdateUserByUserID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameters for stored procedure
                        lComm.Parameters.AddWithValue("@parm_user_id", SqlDbType.Int).Value = iUser.UserIDPK;
                        lComm.Parameters.AddWithValue("@parm_role_id", SqlDbType.Int).Value = iUser.UserRoleIDFK;
                        lComm.Parameters.AddWithValue("@parm_user_fname", SqlDbType.VarChar).Value = iUser.UserFirstName;
                        lComm.Parameters.AddWithValue("@parm_user_lname", SqlDbType.VarChar).Value = iUser.UserLastName;
                        lComm.Parameters.AddWithValue("@parm_user_login_id", SqlDbType.VarChar).Value = iUser.UserLogInID;
                        lComm.Parameters.AddWithValue("@parm_user_password", SqlDbType.VarChar).Value = iUser.UserPassword;
                        lComm.Parameters.AddWithValue("@parm_user_birth", SqlDbType.DateTime).Value = iUser.UserBirth;
                        lComm.Parameters.AddWithValue("@parm_is_active", SqlDbType.Bit).Value = iUser.UserIsActive;

                        // check if null
                        if (string.IsNullOrEmpty(iUser.UserEmail))
                        {
                            lComm.Parameters.AddWithValue("@parm_user_email", SqlDbType.VarChar).Value = DBNull.Value;
                        }
                        else
                        {
                            lComm.Parameters.AddWithValue("@parm_user_email", SqlDbType.VarChar).Value = iUser.UserEmail;
                        }

                        // check if null
                        if (string.IsNullOrEmpty(iUser.UserPhone))
                        {
                            lComm.Parameters.AddWithValue("@parm_user_phone", SqlDbType.VarChar).Value = DBNull.Value;
                        }
                        else
                        {
                            lComm.Parameters.AddWithValue("@parm_user_phone", SqlDbType.VarChar).Value = iUser.UserPhone;
                        }

                        // Open connection
                        lConn.Open();

                        lComm.ExecuteNonQuery();

                        lResult = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // handle Exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lResult;

        }

        /// <summary>
        /// Description: This method updates a user's status
        /// </summary>
        /// <param name="iUserIDPK">A unique user id</param>
        /// <param name="iIsActive">user status (1: active, 0: inactive)</param>
        /// <returns>If successfully updated, return true. Otherwise, false</returns>
        public bool UpdateUserStatusByUserID(int iUserIDPK, int iIsActive)
        {
            // return false
            bool lResult = false;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_UpdateUserStatusByUserID", lConn))
                    {

                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameters for stored procedure
                        lComm.Parameters.AddWithValue("@parm_user_id", SqlDbType.Int).Value = iUserIDPK;

                        // if user is active, change status to inactive.
                        // if user is inactive, change status to active
                        lComm.Parameters.AddWithValue("@parm_user_is_active", SqlDbType.Int).Value = iIsActive == 1 ? 0 : 1;

                        // open connection
                        lConn.Open();

                        lComm.ExecuteNonQuery();

                        lResult = true;

                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);

            }

            return lResult;
        }

        /// <summary>
        /// Description: This method checks if user submited valid id and password
        /// </summary>
        /// <param name="iUser">User object with data</param>
        /// <returns>User object with data on success or null on failure</returns>
        public UserDBO FindUserByLogInIDAndPassword(UserDBO iUser)
        {
            // return value
            UserDBO lUser = null;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_FindUserByLogInIDAndPassword", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameters for stored procedure
                        lComm.Parameters.AddWithValue("@parm_user_login_id", SqlDbType.VarChar).Value = iUser.UserLogInID;
                        lComm.Parameters.AddWithValue("@parm_user_password", SqlDbType.VarChar).Value = iUser.UserPassword;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // retrieve data about the user
                            while (lReader.Read())
                            {
                                lUser = new UserDBO();

                                // set values
                                lUser.UserIDPK = (int)lReader["user_id"];
                                lUser.UserLastName = (string)lReader["user_lname"];
                                lUser.UserFirstName = (string)lReader["user_fname"];
                                lUser.UserLogInID = (string)lReader["user_login_id"];
                                lUser.UserRoleName = (string)lReader["role_name"];
                                lUser.UserIsActive = Convert.ToInt32(lReader["is_active"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }
            return lUser;

        }

        /// <summary>
        /// Description: This method returns list of possible roles
        /// </summary>
        /// <returns>List of roles</returns>
        public List<Role> GetAllRoles()
        {
            // list to be returned
            List<Role> lRoleList = new List<Role>();
            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllRoleNames", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // add each role to the list
                            while (lReader.Read())
                            {
                                Role eachRole = new Role();

                                // set values
                                eachRole.RoleIDPK = (int)lReader["role_id"];
                                eachRole.RoleName = (string)lReader["role_name"];

                                lRoleList.Add(eachRole);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lRoleList;
        }

        /// <summary>
        /// Description: This method checks if there is a user using same login ID
        /// </summary>
        /// <param name="iUserLogInID">LogIn ID to be checked</param>
        /// <returns>If there is no user with same id, returns 0. Otherwise, number of users with the same userid</returns>
        public int FindUserByUserLogInID(string iUserLogInID)
        {
            // return value
            int lResult = 0;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_FindUserByUserLogInID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("parm_user_login_id", SqlDbType.VarChar).Value = iUserLogInID;

                        // open connection
                        lConn.Open();

                        lResult = Convert.ToInt32(lComm.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lResult;
        }

        /// <summary>
        /// Description: This method inserts user's rating on food order
        /// </summary>
        /// <param name="iUserOrderRatingDBO">UserOrderRating object with data</param>
        /// <returns>If successfully inserted, returns a unique rating id. Otherwise, 0</returns>
        public int CreateUserOrderRating(UserOrderRatingDBO iUserOrderRatingDBO)
        {
            // return value
            int lResult = 0;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_CreateUserOrderRating", lConn))
                    {

                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameters for stored procedure
                        lComm.Parameters.AddWithValue("@parm_user_order_id_FK", SqlDbType.Int).Value = iUserOrderRatingDBO.UserOrderIDFK;
                        lComm.Parameters.AddWithValue("@parm_score", SqlDbType.Float).Value = iUserOrderRatingDBO.Score;

                        // check null value
                        if (string.IsNullOrEmpty(iUserOrderRatingDBO.Content))
                        {
                            lComm.Parameters.AddWithValue("@parm_content", SqlDbType.VarChar).Value = DBNull.Value;
                        }
                        else
                        {
                            lComm.Parameters.AddWithValue("@parm_content", SqlDbType.VarChar).Value = iUserOrderRatingDBO.Content;
                        }

                        // open connection
                        lConn.Open();

                        lResult = Convert.ToInt32(lComm.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }
            return lResult;
        }

        /// <summary>
        /// Description: This method removes user with the id
        /// </summary>
        /// <param name="id">A unique user id</param>
        /// <returns>If successfully removed, return true. Otherwise, false</returns>
        public bool RemoveUserByUserID(int id)
        {
            // return value
            bool lResult = false;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_RemoveUserByUserID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_user_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        lComm.ExecuteNonQuery();

                        lResult = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lResult;
        }

    }
}