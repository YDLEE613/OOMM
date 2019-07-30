namespace BootcampTraineeDAL
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Data;
    using BootcampTraineeDBObjects;
    using BootcampTraineeDBObjects.SubDBO;
    using System.Configuration;

    /// <summary>
    /// Description: This class manages database connection and CRUD on database tables especially regarding board.
    /// </summary>
    public class BoardDAL
    {
        // connection string to database
        // string lConnectionString = "Server=DESKTOP-H52G7QL\\SQLEXPRESS;Database=OnshoreCapstone;Trusted_Connection=True;";
        private string lConnectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;

        /// <summary>
        /// Description: This method inserts Board data into database and return integer value.
        /// </summary>
        /// <param name="iBoard"> A BoardDBO object with all the information to be inserted into database </param>
        /// <returns>If the parameter iBoard is successfully inserted into database, return Inserted.IDPK. Otherwise, 0</returns>
        public int CreateBoard(BoardDBO iBoard)
        {
            // variable to be returned
            int lResult = 0;

            try
            {
                // Establish conneciton
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {

                    // Use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_CreateBoard", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set values to be inserted as parameters for stored procedure
                        lComm.Parameters.AddWithValue("@parm_user_id_FK", SqlDbType.Int).Value = iBoard.UserIDFK;
                        lComm.Parameters.AddWithValue("@parm_title", SqlDbType.VarChar).Value = iBoard.Title;
                        lComm.Parameters.AddWithValue("@parm_content", SqlDbType.Text).Value = iBoard.Content;
                        lComm.Parameters.AddWithValue("@parm_is_fixed", SqlDbType.Bit).Value = iBoard.IsFixed;
                        lComm.Parameters.AddWithValue("@parm_category_id_FK", SqlDbType.Int).Value = iBoard.CategoryIDFK;

                        // Open Connection
                        lConn.Open();

                        // Execute Query and get Inserted.IDPK as return value
                        lResult = Convert.ToInt32(lComm.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lResult;
        }

        /// <summary>
        /// Description: This method removes the board in database by boardID and return bool value.
        /// </summary>
        /// <param name="iBoardID">A unique BoardIDPK to find the post to be removed </param>
        /// <returns>If the post is successfully removed, return True. Otherwise, false.</returns>
        public bool RemoveBoardByBoardID(int iBoardID)
        {
            // variable to be returend
            bool iResult = false;

            try
            {
                // Establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {

                    // Use stored procudure
                    using (SqlCommand lComm = new SqlCommand("sp_RemoveBoardByBoardID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // Set value
                        lComm.Parameters.AddWithValue("@parm_board_id", SqlDbType.Int).Value = iBoardID;

                        // Open Conenction
                        lConn.Open();

                        // Execute Query
                        lComm.ExecuteNonQuery();

                        // Update return value
                        iResult = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle Exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return iResult;
        }

        /// <summary>
        /// Description: This method updates a post in the database by BoardIDPK and returns bool value.
        /// </summary>
        /// <param name="iBoardDBO">A post to be updated.</param>
        /// <returns>If the post is successfully updated, return True. Otherwise, false</returns>
        public bool UpdateBoardByBoardID(BoardDBO iBoardDBO)
        {
            // variable to be returned
            bool lResult = false;

            try
            {
                // Establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {

                    // Use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_UpdateBoardByBoardID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set values for parameters needed for stored procedure
                        lComm.Parameters.AddWithValue("@parm_board_id", SqlDbType.Int).Value = iBoardDBO.BoardIDPK;
                        lComm.Parameters.AddWithValue("@parm_title", SqlDbType.VarChar).Value = iBoardDBO.Title;
                        lComm.Parameters.AddWithValue("@parm_content", SqlDbType.VarChar).Value = iBoardDBO.Content;
                        lComm.Parameters.AddWithValue("@parm_is_fixed", SqlDbType.Bit).Value = iBoardDBO.IsFixed;
                        lComm.Parameters.AddWithValue("@parm_category_id_FK", SqlDbType.Int).Value = iBoardDBO.CategoryIDFK;

                        // Open connection
                        lConn.Open();

                        // Execute query
                        lComm.ExecuteNonQuery();

                        // change return value
                        lResult = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lResult;
        }

        /// <summary>
        /// Description: This method retrieves all the list of categories for board from database
        /// </summary>
        /// <returns>List of Categories for board</returns>
        public List<CategoryDBO> GetAllBoardCategories()
        {
            // List to store all the categories
            List<CategoryDBO> lCategoryList = new List<CategoryDBO>();
            try
            {
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllBoardCategories", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        lConn.Open();

                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            // Add all (all the categories)
                            lCategoryList.Add(new CategoryDBO(0, "All"));

                            // Retrieve all categories and add to the list
                            while (lReader.Read())
                            {
                                int lCategoryID = (int)lReader["category_id"];
                                string lCategoryName = (string)lReader["category_name"];

                                lCategoryList.Add(new CategoryDBO(lCategoryID, lCategoryName));

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

            return lCategoryList;
        }

        /// <summary>
        /// Description: This method finds and returns the list of specific posts required by user 
        /// </summary>
        /// <param name="category">category name that a user specified </param>
        /// <param name="searchString">search string that a user specified </param>
        /// <returns>List of posts that are in the same category and contains searchString that user specified.</returns>
        public List<BoardDBO> GetAllBoards(string category, string searchString)
        {
            // List to be returned
            List<BoardDBO> lBoardList = new List<BoardDBO>();
            try
            {
                // establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllBoards", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameters for stroed procedure  
                        lComm.Parameters.AddWithValue("@parm_category_id", SqlDbType.Int).Value = Convert.ToInt32(category);

                        // if searchString is not null, pass the variable for parameter. 
                        // If it is, pass DB null value.
                        if (!string.IsNullOrEmpty(searchString))
                        {
                            lComm.Parameters.AddWithValue("@parm_search_string", SqlDbType.VarChar).Value = searchString;
                        }
                        else
                        {
                            lComm.Parameters.AddWithValue("@parm_search_string", SqlDbType.VarChar).Value = DBNull.Value;
                        }

                        // Open connection
                        lConn.Open();

                        // Retrieve all the data of posts that meet user's requirements and add to list
                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            while (lReader.Read())
                            {
                                BoardDBO lBoard = new BoardDBO();

                                // set values
                                lBoard.BoardIDPK = (int)lReader["board_id"];
                                lBoard.UserIDFK = (int)lReader["user_id_FK"];
                                lBoard.UserName = (string)lReader["user_fname"] + " " + (string)lReader["user_lname"];
                                lBoard.Title = (string)lReader["title"];
                                lBoard.Content = (string)lReader["content"];
                                lBoard.DateCreated = (DateTime)lReader["date_created"];
                                lBoard.DateModified = (DateTime)lReader["date_modified"];
                                lBoard.IsFixed = Convert.ToInt32(lReader["is_fixed"]);
                                lBoard.CategoryIDFK = (int)lReader["category_id_FK"];
                                lBoard.CategoryName = (string)lReader["category_name"];

                                lBoardList.Add(lBoard);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                ExceptionDAL lExceptionDAL = new ExceptionDAL();
                lExceptionDAL.CreateExceptionLog(ex);
            }

            return lBoardList;

        }

        /// <summary>
        /// Description: This method retrieves all the posts written by specific user by a unique UserID
        /// </summary>
        /// <param name="id">A unique ID for user</param>
        /// <returns>List of posts that the user has written.</returns>
        public List<BoardDBO> GetAllBoardsByUserID(int id)
        {
            // List to be returned
            List<BoardDBO> lBoardList = new List<BoardDBO>();

            try
            {
                // Establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // Use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_GetAllBoardsByUserID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_user_id", SqlDbType.Int).Value = id;

                        // Open Connection
                        lConn.Open();

                        // Retrieves all the data about posts written by user with the id(parameter)
                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {
                            while (lReader.Read())
                            {
                                BoardDBO lBoard = new BoardDBO();

                                // set values
                                lBoard.BoardIDPK = (int)lReader["board_id"];
                                lBoard.UserIDFK = (int)lReader["user_id_FK"];
                                lBoard.UserName = (string)lReader["user_name"];
                                lBoard.Title = (string)lReader["title"];
                                lBoard.Content = (string)lReader["content"];
                                lBoard.DateCreated = (DateTime)lReader["date_created"];
                                lBoard.DateModified = (DateTime)lReader["date_modified"];
                                lBoard.IsFixed = Convert.ToInt32(lReader["is_fixed"]);
                                lBoard.CategoryIDFK = (int)lReader["category_id"];
                                lBoard.CategoryName = (string)lReader["category_name"];

                                // add to the list
                                lBoardList.Add(lBoard);
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

            return lBoardList;
        }

        /// <summary>
        /// Description: This method finds and returns the post with specific id
        /// </summary>
        /// <param name="id">A unique Board Id</param>
        /// <returns>BoardDBO object with the specific id</returns>
        public BoardDBO FindBoardByBoardID(int id)
        {
            // object to be returned
            BoardDBO iBoardDBO = new BoardDBO();

            try
            {
                // Establish connection
                using (SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // Use stored procedure
                    using (SqlCommand lComm = new SqlCommand("sp_FindBoardByBoardID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // set parameter for stored procedure
                        lComm.Parameters.AddWithValue("@parm_board_id", SqlDbType.Int).Value = id;

                        // open connection
                        lConn.Open();

                        // Retrieves all that about the post with the id
                        using (SqlDataReader lReader = lComm.ExecuteReader())
                        {

                            while (lReader.Read())
                            {

                                // Assign values from database to an object of BoardDBO
                                iBoardDBO.BoardIDPK = (int)lReader["board_id"];
                                iBoardDBO.UserIDFK = (int)lReader["user_id_FK"];
                                iBoardDBO.UserName = (string)lReader["user_fname"] + " " + (string)lReader["user_lname"];
                                iBoardDBO.UserRoleName = (string)lReader["role_name"];
                                iBoardDBO.Title = (string)lReader["title"];
                                iBoardDBO.Content = (string)lReader["content"];
                                iBoardDBO.DateCreated = (DateTime)lReader["date_created"];
                                iBoardDBO.DateModified = (DateTime)lReader["date_modified"];
                                iBoardDBO.IsFixed = Convert.ToInt32(lReader["is_fixed"]);
                                iBoardDBO.CategoryIDFK = (int)lReader["category_id_FK"];
                                iBoardDBO.CategoryName = (string)lReader["category_name"];

                            }

                            return iBoardDBO;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle Exception
                ExceptionDAL lExeceptionDAL = new ExceptionDAL();
                lExeceptionDAL.CreateExceptionLog(ex);
            }


            return null;
        }

        /// <summary>
        /// Description: This methods checks if the user who resquested to edit a post has the same user id.
        /// </summary>
        /// <param name="iBoardIDPK"> A unique Board ID </param>
        /// <param name="iUserIDPK"> A unique User ID</param>
        /// <returns>If it is the same user, return true. Otherwise, false </returns>
        public bool FindBoolSameUserByUserIDAndBoardID(int iBoardIDPK, int iUserIDPK)
        {
            // bool value to be returned
            bool lResult = false;

            try{
                // Establish connecction
                using(SqlConnection lConn = new SqlConnection(lConnectionString))
                {
                    // User stored procedure
                    using(SqlCommand lComm = new SqlCommand("sp_FindBoolSameUserByUserIDAndBoardID", lConn))
                    {
                        lComm.CommandType = CommandType.StoredProcedure;
                        lComm.CommandTimeout = 10;

                        // Set paratemers for stored procedure
                        lComm.Parameters.AddWithValue("@parm_user_id", SqlDbType.Int).Value = iUserIDPK;
                        lComm.Parameters.AddWithValue("@parm_board_id", SqlDbType.Int).Value = iBoardIDPK;

                        // Open connection
                        lConn.Open();

                        // if there is a board with same UserID and BoardID, stored procedure returns 1. Otherwise, 0.
                        // if returned value is 1, the post is written by the user who requested to edit.
                        lResult = Convert.ToInt32(lComm.ExecuteScalar()) == 0? false : true;
                    }
                }
            }catch(Exception ex)
            {
                // Handle Exception
                ExceptionDAL lExeceptionDAL = new ExceptionDAL();
                lExeceptionDAL.CreateExceptionLog(ex);
            }

            return lResult;
        }
    }
}
