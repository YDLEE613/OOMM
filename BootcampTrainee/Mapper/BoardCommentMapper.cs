namespace BootcampTrainee.Mapper
{
    using System.Collections.Generic;
    using BootcampTraineeDBObjects;
    using BootcampTrainee.Models;

    /// <summary>
    /// This class manages mapping from an object or a list of objects to another object or list of objects
    /// </summary>
    public class BoardCommentMapper
    {
        /// <summary>
        /// Description: This method maps list of database objects to list of Model
        /// </summary>
        /// <param name="iBoardCommentDBOList">List of BoardComment database objects to be mapped</param>
        /// <returns>List of BoardComment Model</returns>
        public List<BoardComment> MapBoardCommentDBOToBoardComment(List<BoardCommentDBO> iBoardCommentDBOList)
        {
            // list to be returned
            List<BoardComment> lBoardCommentList = new List<BoardComment>();

            // loop through the input list and map each item to Model.BoardComment
            foreach(BoardCommentDBO lEachComment in iBoardCommentDBOList)
            {
                // Object to be added to the list
                BoardComment lBoardComment = new BoardComment();

                // set values
                lBoardComment.BoardCommentIDPK = lEachComment.BoardCommentIDPK;
                lBoardComment.UserIDFK = lEachComment.UserIDFK;
                lBoardComment.UserName = lEachComment.UserName;
                lBoardComment.UserRoleName = lEachComment.UserRoleName;
                lBoardComment.BoardIDFK = lEachComment.BoardIDFK;
                lBoardComment.Content = lEachComment.Content;
                lBoardComment.DateCreated = lEachComment.DateCreated;
                lBoardComment.DateModified = lEachComment.DateModified;

                lBoardCommentList.Add(lBoardComment);
            }

            return lBoardCommentList;
        }

        /// <summary>
        /// Description: This method maps Model object to database obect
        /// </summary>
        /// <param name="iBoardComment">Object to be mapped</param>
        /// <returns>BoardCommentDBO object</returns>
        public BoardCommentDBO MapBoardCommentToBoardCommentDBO(BoardAndCommentsViewModel iBoardComment)
        {
            // Instantiate BoardCommentDBO object
            BoardCommentDBO lBoardCommentDBO = new BoardCommentDBO();

            // set values
            lBoardCommentDBO.BoardIDFK = iBoardComment.BoardComment.BoardIDFK;
            lBoardCommentDBO.UserIDFK = iBoardComment.BoardComment.UserIDFK;
            lBoardCommentDBO.Content = iBoardComment.BoardComment.Content;

            return lBoardCommentDBO;
        }

        /// <summary>
        /// Description: This method maps database object to Model object
        /// </summary>
        /// <param name="iBoardCommentDBO">Object to be mapped</param>
        /// <returns>BoardComment object</returns>
        public BoardComment MapBoardCommentDBOToBoardComment(BoardCommentDBO iBoardCommentDBO)
        {
            // Instantiate BoardComment object
            BoardComment lBoardComment = new BoardComment();

            // set values
            lBoardComment.BoardCommentIDPK = iBoardCommentDBO.BoardCommentIDPK;
            lBoardComment.UserIDFK = iBoardCommentDBO.UserIDFK;
            lBoardComment.UserName = iBoardCommentDBO.UserName;
            lBoardComment.UserRoleName = iBoardCommentDBO.UserRoleName;
            lBoardComment.DateCreated = iBoardCommentDBO.DateCreated;
            lBoardComment.Content = iBoardCommentDBO.Content;
            lBoardComment.DateModified = iBoardCommentDBO.DateModified;
            lBoardComment.BoardIDFK = iBoardCommentDBO.BoardIDFK;

            return lBoardComment;
        }

        /// <summary>
        /// Description: This method maps Model object to database object
        /// </summary>
        /// <param name="iBoardComment">Object to be mapped</param>
        /// <returns>BoardCommentDBO object</returns>
        public BoardCommentDBO MapBoardCommentToBoardCommentDBO(BoardComment iBoardComment)
        {
            // Instantiate BoardCommentDBO object
            BoardCommentDBO lBoardCommentDBO = new BoardCommentDBO();

            // set values
            lBoardCommentDBO.BoardCommentIDPK = iBoardComment.BoardCommentIDPK;
            lBoardCommentDBO.Content = iBoardComment.Content;

            return lBoardCommentDBO;
        }
    }
}