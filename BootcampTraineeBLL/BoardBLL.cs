namespace BootcampTraineeBLL
{
    using System.Collections.Generic;
    using BootcampTraineeDBObjects;
    using BootcampTraineeDBObjects.SubDBO;
    using BootcampTraineeDAL;

    /// <summary>
    /// This class manages calling methods in DAL, and getting and returning values from DAL
    /// </summary>
    public class BoardBLL
    {
        /// <summary>
        /// Description: This method calls method in DAL layer to insert data into database and return integer value.
        /// </summary>
        /// <param name="iBoard"> A BoardDBO object with all the information to be inserted into database </param>
        /// <returns>If the parameter iBoard is successfully inserted into database, return Inserted.IDPK. Otherwise, 0</returns>
        public int CreateBoard(BoardDBO lBoardDBO)
        {
            // Instantiate BoardDAL object
            BoardDAL lBoardDAL = new BoardDAL();

            int lResult = lBoardDAL.CreateBoard(lBoardDBO);

            return lResult;
        }
        /// <summary>
        /// Description: This method calls method in DAL layer to retrieve all the list of categories for board from database
        /// </summary>
        /// <returns>List of Categories for board</returns>
        public List<CategoryDBO> GetAllBoardCategories()
        {
            // Instantiate BoardDAL object
            BoardDAL lBoardDAL = new BoardDAL();

            List<CategoryDBO> lCategoryList = lBoardDAL.GetAllBoardCategories();

            return lCategoryList;
        }
        /// <summary>
        /// Description: This method calls method in DAL layer to find and return the list of specific posts required by user 
        /// </summary>
        /// <param name="category">category name that a user specified </param>
        /// <param name="searchString">search string that a user specified </param>
        /// <returns>List of posts that are in the same category and contains searchString that user specified.</returns>
        public List<BoardDBO> GetAllBoards(string category, string searchString)
        {
            // Instantiate BoardDAL object
            BoardDAL lBoardDAL = new BoardDAL();

            List<BoardDBO> oBoardList = lBoardDAL.GetAllBoards(category, searchString);

            return oBoardList;
        }
        /// <summary>
        /// Description: This method calls method in DAL layer to retrieve all the posts written by specific user by a unique UserID
        /// </summary>
        /// <param name="id">A unique ID for user</param>
        /// <returns>List of posts that the user has written.</returns>
        public List<BoardDBO> GetAllBoardsByUserID(int id)
        {
            // Instantiate DAL object
            BoardDAL lBoardDAL = new BoardDAL();

            List<BoardDBO> lBoardList = lBoardDAL.GetAllBoardsByUserID(id);

            return lBoardList;
        }
        /// <summary>
        /// Description: This method calls method in DAL layer to find and returns the post with specific id
        /// </summary>
        /// <param name="id">A unique Board Id</param>
        /// <returns>BoardDBO object with the specific id</returns>
        public BoardDBO FindBoardByBoardID(int id)
        {
            // Instantiate BoardDAL object
            BoardDAL lBoardDAL = new BoardDAL();

            BoardDBO oBoard = lBoardDAL.FindBoardByBoardID(id);

            return oBoard;
        }
        /// <summary>
        /// Description: This method calls method in DAL layer to remove the board in database by boardID and return bool value.
        /// </summary>
        /// <param name="iBoardID">A unique BoardIDPK to find the post to be removed </param>
        /// <returns>If the post is successfully removed, return True. Otherwise, false.</returns>
        public bool RemoveBoardByBoardID(int iBoardID)
        {
            // Instantiate BoardDAL object
            BoardDAL lBoardDAL = new BoardDAL();

            bool lResult = lBoardDAL.RemoveBoardByBoardID(iBoardID);

            return lResult;
        }
        /// <summary>
        /// Description: This method calls method in DAL layer to update a post in the database by BoardIDPK and returns bool value.
        /// </summary>
        /// <param name="iBoardDBO">A post to be updated.</param>
        /// <returns>If the post is successfully updated, return True. Otherwise, false</returns>
        public bool UpdateBoardByBoardID(BoardDBO iBoardDBO)
        {
            // Instantiate BoardDAL object
            BoardDAL lBoardDAL = new BoardDAL();

            bool lResult = lBoardDAL.UpdateBoardByBoardID(iBoardDBO);

            return lResult;
        }
        /// <summary>
        /// Description: This methods calls method in DAL layer to check if the user who resquested to edit a post has the same user id.
        /// </summary>
        /// <param name="iBoardIDPK"> A unique Board ID </param>
        /// <param name="iUserIDPK"> A unique User ID</param>
        /// <returns>If it is the same user, return true. Otherwise, false </returns>
        public bool FindBoolSameUserByUserIDAndBoardID(int iBoardIDPK, int iUserIDPK)
        {
            // Instantiate BoardDAL object
            BoardDAL lBoardDAL = new BoardDAL();

            bool lResult = lBoardDAL.FindBoolSameUserByUserIDAndBoardID(iBoardIDPK, iUserIDPK);

            return lResult;
        }

    }
}
