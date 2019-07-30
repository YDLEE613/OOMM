namespace BootcampTraineeBLL
{
    using System.Collections.Generic;
    using BootcampTraineeDAL;
    using BootcampTraineeDBObjects;

    /// <summary>
    /// This class manages calling methods in DAL, and getting and returning values from DAL
    /// </summary>
    public class BoardCommentBLL
    {
        /// <summary>
        /// Description: This method calls a method in DAL layer to get a list of board comments related to the post with id
        /// </summary>
        /// <param name="id"> A unique Board ID</param>
        /// <returns> List of board comments </returns>
        public List<BoardCommentDBO> GetAllCommentsByBoardID(int id)
        {
            // Instantitate BoardCommentDAL object
            BoardCommentDAL lBoardCommentDAL = new BoardCommentDAL();

            // get list of comments
            List<BoardCommentDBO> lBoardCommentDBOList = lBoardCommentDAL.GetAllCommentsByBoardID(id);

            return lBoardCommentDBOList;
        }
        /// <summary>
        /// Description: This method calls a method in DAL layer to insert comment data into database and return integer value
        /// </summary>
        /// <param name="iBoardCommentDBO">BoardComment object with data</param>
        /// <returns>A unique BoardComment ID on success. Otherwise, 0</returns>
        public int CreateBoardComment(BoardCommentDBO iBoardCommentDBO)
        {
            // Instantiate BoardCommentDAL object
            BoardCommentDAL lBoardCommentDAL = new BoardCommentDAL();

            // get comment id on success. 0 in fail
            int lResult = lBoardCommentDAL.CreateBoardComment(iBoardCommentDBO);

            return lResult;
        }
        /// <summary>
        /// Description: This method calls a method in DAL layer to get list of board comments written by a user with id
        /// </summary>
        /// <param name="id"> A unique User ID</param>
        /// <returns>List of board comments written by a specific user</returns>
        public List<BoardCommentDBO> GetAllCommentsByUserID(int id)
        {
            // Instantiate DAL object
            BoardCommentDAL lBoardCommentDAL = new BoardCommentDAL();

            // get list of comments
            List<BoardCommentDBO> lCommentList = lBoardCommentDAL.GetAllCommentsByUserID(id);

            return lCommentList;
        }
        /// <summary>
        /// Description: This method calls a method in DAL layer to remove board comment with id from database
        /// </summary>
        /// <param name="id">A unique board comment</param>
        /// <returns>If board comment is removed successfully, return true. Otherwise, false</returns>
        public bool RemoveBoardCommentByBoardCommentID(int id)
        {
            // Instantiate DAL object
            BoardCommentDAL lBoardCommentDAL = new BoardCommentDAL();

            // get true on success or false on failure
            bool lResult = lBoardCommentDAL.RemoveBoardCommentByBoardCommentID(id);

            return lResult;
        }
        /// <summary>
        /// Description: This method calls a method in DAL layer to get BoardComment object with all data 
        /// </summary>
        /// <param name="id">A unique BoardComment ID</param>
        /// <returns>If there is BoardComment with the id, return BoardComment object with data. Otherwise, null</returns>
        public BoardCommentDBO FindBoardCommentByBoardCommentID(int id)
        {
            // Instantiate DAL object
            BoardCommentDAL lBoardCommentDAL = new BoardCommentDAL();

            // get data for comment
            BoardCommentDBO lBoardCommentDBO = lBoardCommentDAL.FindBoardCommentByBoardCommentID(id);

            return lBoardCommentDBO;
        }

        /// <summary>
        /// Description: This method calls a method in DAL layer to check if update on the comment was successful or not
        /// </summary>
        /// <param name="iBoardCommentDBO">BoardComment object to be updated</param>
        /// <returns>If successfully updated, return true. Otherwise, false</returns>
        public bool UpdateBoardCommentByBoardCommentID(BoardCommentDBO iBoardCommentDBO)
        {
            // Instantiate DAL object
            BoardCommentDAL lBoardCommentDAL = new BoardCommentDAL();

            // get true for success or false for failure
            bool lResult = lBoardCommentDAL.UpdateBoardCommentByBoardCommentID(iBoardCommentDBO);

            return lResult;
        }
    }
}
