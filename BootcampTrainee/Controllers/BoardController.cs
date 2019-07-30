namespace BootcampTrainee.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using BootcampTrainee.Filters;
    using BootcampTrainee.Mapper;
    using BootcampTrainee.Models;
    using BootcampTraineeBLL;
    using BootcampTraineeDBObjects;
    using BootcampTraineeDBObjects.Util;

    /// <summary>
    /// This class manages GET and POST actions regarding Board
    /// </summary>
    public class BoardController : Controller
    {
        [HttpGet]
        [MustBeLoggedIn]
        public ActionResult BoardCreate()
        {
            Board lBoard = new Board();

            // message for user
            lBoard.type = 0;
            lBoard.message = "Please share ideas or codes!";

            return View(lBoard);
        }

        [HttpPost]
        [MustBeLoggedIn]
        public ActionResult BoardCreate(Board iBoard)
        {
            // check if every input is valid
            if (ModelState.IsValid)
            {
                //get value from executing query
                int lResult = 0;

                // Instantiate Mapper object
                BoardMapper lBoardMapper = new BoardMapper();

                // Map Model.Board to BoardDBO
                BoardDBO lBoardDBO = lBoardMapper.MapBoardTOBoardDBO(iBoard);

                // Instantiate DAL object
                BoardBLL lBoardBLL = new BoardBLL();

                // Change the UserIDFK to Session["AUTHUserIDPK"]
                lBoardDBO.UserIDFK = Convert.ToInt32(Session["AUTHUserIDPK"]);

                // insert into the database
                lResult = lBoardBLL.CreateBoard(lBoardDBO);

                // Success
                if (lResult > 0)
                {
                    // message on success
                    TempData["msg"] = "<script>alert('Successfully Written!');</script>";
                    return RedirectToAction("BoardList", "Board");
                }

                // Failed to insert
                else
                {
                    // message on failure
                    iBoard.type = -1;
                    iBoard.message = "Failed. Please try again.";
                }
            }
            // ModelState is not valid
            else
            {
                iBoard.type = -1;
                iBoard.message = "Please Fill all Required Information.";
            }

            return View(iBoard);
        }

        [HttpGet]
        [MustBeLoggedIn]
        public ActionResult BoardList(string category, string searchString)
        {
            // Instantiate Objects
            BoardBLL lBoardBLL = new BoardBLL();
            BoardListUtil BoardListUtil = new BoardListUtil();

            // Retreive list of posts that are in the same category and contain search string from database 
            BoardListUtil.BoardList = lBoardBLL.GetAllBoards(category, searchString);

            // highlighting search string
            ViewBag.searchString = searchString;

            // categories for search bar
            BoardListUtil.CategoryList = new SelectList(lBoardBLL.GetAllBoardCategories(), "CategoryID", "CategoryName");

            return View(BoardListUtil);
        }


        [HttpGet]
        [MustBeLoggedIn]
        public ActionResult BoardView(int id)
        {
            // Instantiate BCViewModel to pass three models
            BoardAndCommentsViewModel lBCViewModel = new BoardAndCommentsViewModel();

            // Instantiate Mapper Objects
            BoardCommentMapper lBCMapper = new BoardCommentMapper();
            BoardMapper lBoardMapper = new BoardMapper();

            // Instantiate BLL Objects
            BoardBLL lBoardBLL = new BoardBLL();
            BoardCommentBLL lBoardCommentBLL = new BoardCommentBLL();


            // Retreive data for post from database based on BoardIDPK
            BoardDBO lBoardDBO = lBoardBLL.FindBoardByBoardID(id);

            // if there is no post with the id, redirect to board list with error message
            if (lBoardDBO == null)
            {
                TempData["msg"] = "<script>alert('Error occured while processing your request.')</script>";
                return RedirectToAction("BoardList", "Board");
            }

            // Retreive data for comments from database based on BoardIDPK
            List<BoardCommentDBO> lBoardCommentDBOList = lBoardCommentBLL.GetAllCommentsByBoardID(id);

            // Map DB objects to Model.BoardComment
            lBCViewModel.BoardCommentList = lBCMapper.MapBoardCommentDBOToBoardComment(lBoardCommentDBOList);
            lBCViewModel.Board = lBoardMapper.MapBoardDBOToBoard(lBoardDBO);

            // set values for board comment
            lBCViewModel.BoardComment = new BoardComment();
            lBCViewModel.BoardComment.BoardIDFK = lBCViewModel.Board.BoardIDPK;
            lBCViewModel.BoardComment.UserIDFK = Convert.ToInt32(Session["AUTHUserIDPK"]);

            return View(lBCViewModel);
        }

        [HttpGet]
        [MustBeLoggedIn]
        public ActionResult BoardRemove(int id)
        {
            // Instantiate BoardBLL object
            BoardBLL lBoardBLL = new BoardBLL();

            // Remove data from database and get bool as return value
            bool lResult = lBoardBLL.RemoveBoardByBoardID(id);

            if (lResult)
            {
                // success
                TempData["msg"] = "<script>alert('Successfully Removed!');</script>";
            }
            else
            {
                // fail
                TempData["msg"] = "<script>alert('Your request failed. Please try later.');</script>";
            }

            return RedirectToAction("BoardList", "Board");
        }

        [HttpGet]
        [MustBeLoggedIn]
        public ActionResult BoardUpdate(int id)
        {
            // Instantiate BoardBLL object
            BoardBLL lBoardBLL = new BoardBLL();

            // check if the user requesting to edit the view is same user by boardID and UserID
            bool lIsSameUser = lBoardBLL.FindBoolSameUserByUserIDAndBoardID(id, Convert.ToInt32(Session["AUTHUserIDPK"]));

            // if it is different user who is requesting to edit the post, redirect to the view with error message
            if (!lIsSameUser)
            {
                int lBoardID = id;
                TempData["msg"] = "<script>alert('You are not authorized to edit.');</script>";
                return RedirectToAction("BoardView", "Board", new { id = lBoardID });
            }

            // Find the board with id as primary key
            BoardDBO lBoardDBO = lBoardBLL.FindBoardByBoardID(id);

            // Instantiate Model.Board object
            BoardMapper lBoardMapper = new BoardMapper();

            // Map to Model.Board
            Board lBoard = lBoardMapper.MapBoardDBOToBoard(lBoardDBO);

            return View(lBoard);
        }

        [HttpPost]
        [MustBeLoggedIn]
        public ActionResult BoardUpdate(Board iBoard)
        {

            if (ModelState.IsValid)
            {
                // Instantiate BoardMapper object
                BoardMapper lBoardMapper = new BoardMapper();
                BoardDBO lBoardDBO = lBoardMapper.MapBoardTOBoardDBO(iBoard);

                // Instantiate BoardBLL object
                BoardBLL lBoardBLL = new BoardBLL();

                // Update the board in database
                bool lResult = lBoardBLL.UpdateBoardByBoardID(lBoardDBO);

                if (lResult)
                {
                    // success
                    iBoard.type = 1;
                    iBoard.message = "Successfully Updated!";
                }
                else
                {
                    // fail
                    iBoard.type = -1;
                    iBoard.message = "Failed to Update!";
                }
            }
            else
            {
                iBoard.type = -1;
                iBoard.message = "Please Fill all required fields.";
            }

            return RedirectToAction("BoardView", "Board", new { @id = iBoard.BoardIDPK });
        }

    }
}