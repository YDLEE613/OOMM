namespace BootcampTraineeDAL
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using BootcampTraineeDBObjects;

    /// <summary>
    /// Description: This class manages database connection and CRUD on database tables especially regarding board comments.
    /// </summary>
    public class BoardCommentDAL
    {
        // connection string to database
        //string lConnectionString = "Server=LAPTOP-VC1M94MC\\SQLEXPRESS01;Database=OnshoreCapstone;Trusted_Connection=True;";
        private string lConnectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;

        /// <summary>
        /// Description: This method returns a list of board comments related to the post with id
        /// </summary>
        /// <param name="id"> A unique Board ID</param>
        /// <returns> List of board comments </returns>
        public List<BoardCommentDBO> GetAllCommentsByBoardID(int id)
        {
            // List to store comments
            List<BoardCommentDBO> lBoardCommentDBOList = new List<BoardCommentDBO>();

            try
            {
                // Establish connection
                using(SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using(SqlCommand lComm = new SqlCommand("sp_GetAllCommentsByBoardID", lConn))
                    {

                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_board_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using(SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // Retrieve data about all board comments
                            while (lReader.Read())
                            {
                                // Board commented to be added into the list to be returned
                                BoardCommentDBO lBoardCommentDBO = new BoardCommentDBO();

                                // set values 
                                lBoardCommentDBO.BoardCommentIDPK = (int)lReader["comment_id"];
                                lBoardCommentDBO.BoardIDFK = (int)lReader["board_id_FK"];
                                lBoardCommentDBO.UserIDFK = (int)lReader["user_id_FK"];
                                lBoardCommentDBO.UserName = lReader["user_name"] == DBNull.Value ? "Deleted User" : (string)lReader["user_name"]; ;
                                lBoardCommentDBO.UserRoleName = lReader["role_name"] == DBNull.Value ? "" : (string)lReader["role_name"];
                                lBoardCommentDBO.Content = ((string)lReader["content"]);
                                lBoardCommentDBO.DateCreated = (DateTime)lReader["date_created"];
                                lBoardCommentDBO.DateModified = (DateTime)lReader["date_modified"];

                                lBoardCommentDBOList.Add(lBoardCommentDBO);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle Exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lBoardCommentDBOList;
        }

        /// <summary>
        /// Description: This method inserts comment data into database and return integer value
        /// </summary>
        /// <param name="iBoardCommentDBO">BoardComment object with data</param>
        /// <returns>A unique BoardComment ID on success. Otherwise, 0</returns>
        public int CreateBoardComment(BoardCommentDBO iBoardCommentDBO)
        {
            // return value
            int lResult = 0;

            try
            {
                // Establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using(SqlCommand lComm = new SqlCommand("sp_CreateBoardComment", lConn))
                    {

                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        //set parameters for stored procedure
                        lComm.Parameters.AddWithValue("@parm_board_id_FK", SqlDbType.Int).Value = iBoardCommentDBO.BoardIDFK;
                        lComm.Parameters.AddWithValue("@parm_user_id_FK", SqlDbType.Int).Value = iBoardCommentDBO.UserIDFK;
                        lComm.Parameters.AddWithValue("@parm_content", SqlDbType.VarChar).Value = iBoardCommentDBO.Content;

                        // open connection
                        lConn.Open();

                        // Get Inserted.BoardIDPK as return value
                        lResult = (int)lComm.ExecuteScalar();

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
        /// Description: This method returns list of board comments written by a user with id
        /// </summary>
        /// <param name="id"> A unique User ID</param>
        /// <returns>List of board comments written by a specific user</returns>
        public List<BoardCommentDBO> GetAllCommentsByUserID(int id)
        {
            // List to be returned
            List<BoardCommentDBO> lCommentList = new List<BoardCommentDBO>();

            try
            {
                // establish connection
                using(SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using(SqlCommand lComm = new SqlCommand("sp_GetAllCommentsByUserID", lConn))
                    {

                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_user_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // Retrieve all data bout board comments
                            while (lReader.Read())
                            {
                                BoardCommentDBO lComment = new BoardCommentDBO();

                                lComment.BoardCommentIDPK = (int)lReader["comment_id"];
                                lComment.BoardIDFK = (int)lReader["board_id_FK"];
                                lComment.UserIDFK = (int)lReader["user_id_FK"];
                                lComment.UserName = (string)lReader["user_name"];
                                lComment.UserRoleName = (string)lReader["role_name"];

                                // Replace unnecessary string
                                string lContent = (string)lReader["content"];
                                string lFilteredContent = lContent.Replace("&nbsp;", string.Empty);
                                lComment.Content = lContent;

                                lComment.DateCreated = (DateTime)lReader["date_created"];
                                lComment.DateModified = (DateTime)lReader["date_modified"];

                                lCommentList.Add(lComment);
                            }
                        }
                    }
                }
            }catch(Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }
            return lCommentList;
        }

        /// <summary>
        /// Description: This method removes board comment with id from database
        /// </summary>
        /// <param name="id">A unique board comment</param>
        /// <returns>If board comment is removed successfully, return true. Otherwise, false</returns>
        public bool RemoveBoardCommentByBoardCommentID(int id)
        {
            // return value
            bool lResult = false;

            try
            {
                // establish connection
                using(SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using(SqlCommand lComm = new SqlCommand("sp_RemoveBoardCommentByBoardCommentID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameters for stored procedure
                        lComm.Parameters.AddWithValue("@parm_comment_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        lComm.ExecuteNonQuery();

                        lResult = true;
                    }
                }
            }catch(Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }
            return lResult;

        }

        /// <summary>
        /// Description: This method returns BoardComment object with all data 
        /// </summary>
        /// <param name="id">A unique BoardComment ID</param>
        /// <returns>If there is BoardComment with the id, return BoardComment object with data. Otherwise, null</returns>
        public BoardCommentDBO FindBoardCommentByBoardCommentID(int id)
        {
            // return value
            BoardCommentDBO lBoardCommentDBO = null;

            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_FindBoardCommentByBoardCommentID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_comment_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // if there is comment with the id, retrieve data
                            while (lReader.Read())
                            {
                                lBoardCommentDBO = new BoardCommentDBO();

                                lBoardCommentDBO.BoardCommentIDPK = (int)lReader["comment_id"];
                                lBoardCommentDBO.BoardIDFK = (int)lReader["board_id_FK"];
                                lBoardCommentDBO.UserIDFK = (int)lReader["user_id_FK"];
                                lBoardCommentDBO.Content = (string)lReader["content"];
                                lBoardCommentDBO.UserRoleName = (string)lReader["role_name"];
                                lBoardCommentDBO.UserName = (string)lReader["user_name"];
                                lBoardCommentDBO.DateCreated = (DateTime)lReader["board_date_created"];
                                lBoardCommentDBO.DateModified = (DateTime)lReader["board_date_modified"];
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }
           

            return lBoardCommentDBO;
        }

        /// <summary>
        /// Description: This method checks if update on the comment was successful or not
        /// </summary>
        /// <param name="iBoardCommentDBO">BoardComment object to be updated</param>
        /// <returns>If successfully updated, return true. Otherwise, false</returns>
        public bool UpdateBoardCommentByBoardCommentID(BoardCommentDBO iBoardCommentDBO)
        {
            //return value
            bool lResult = false;

            try
            {
                // establish connection
                using(SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using(SqlCommand lComm = new SqlCommand("sp_UpdateBoardCommentByBoardCommentID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameters for stored procedure
                        lComm.Parameters.AddWithValue("@parm_comment_id", SqlDbType.Int).Value = iBoardCommentDBO.BoardCommentIDPK;
                        lComm.Parameters.AddWithValue("@parm_content", SqlDbType.VarChar).Value = iBoardCommentDBO.Content;

                        // open connection
                        lConn.Open();

                        lComm.ExecuteNonQuery();

                        lResult = true;
                    }
                }
            }catch(Exception ex)
            {
                // handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lResult;
        }
    }
}
