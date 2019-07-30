namespace BootcampTrainee.Mapper
{
    using BootcampTraineeDBObjects;
    using BootcampTrainee.Models;

    /// <summary>
    /// This class manages mapping from an object or a list of objects to another object or list of objects
    /// </summary>
    public class BoardMapper
    {
        /// <summary>
        /// Description: This method maps Board database object to Board Model object
        /// </summary>
        /// <param name="iBoard">Board database object</param>
        /// <returns>Board model object</returns>
        public Board MapBoardDBOToBoard(BoardDBO iBoard)
        {
            Board oBoard = new Board();

            // set values
            oBoard.BoardIDPK = iBoard.BoardIDPK;
            oBoard.UserIDFK = iBoard.UserIDFK;
            oBoard.UserName = iBoard.UserName;
            oBoard.UserRoleName = iBoard.UserRoleName;
            oBoard.Title = iBoard.Title;
            oBoard.Content = iBoard.Content;
            oBoard.DateCreated = iBoard.DateCreated;
            oBoard.DateModified = iBoard.DateModified;
            oBoard.IsFixed = iBoard.IsFixed;
            oBoard.CategoryIDFK = iBoard.CategoryIDFK;
            oBoard.CategoryName = iBoard.CategoryName;

            return oBoard;
        }

        /// <summary>
        /// Description: This method maps Board Model object to Board database object
        /// </summary>
        /// <param name="iBoard">Board Model object</param>
        /// <returns>Board database object</returns>
        public BoardDBO MapBoardTOBoardDBO(Board iBoard)
        {
            BoardDBO oBoard = new BoardDBO();

            // set values
            oBoard.BoardIDPK = iBoard.BoardIDPK;
            oBoard.UserIDFK = iBoard.UserIDFK;
            oBoard.UserName = iBoard.UserName;
            oBoard.Title = iBoard.Title;
            oBoard.Content = iBoard.Content;
            oBoard.DateCreated = iBoard.DateCreated;
            oBoard.DateModified = iBoard.DateModified;
            oBoard.IsFixed = iBoard.IsFixed;
            oBoard.CategoryIDFK = iBoard.CategoryIDFK;
            oBoard.CategoryName = iBoard.CategoryName;

            return oBoard;
        }
    }
}