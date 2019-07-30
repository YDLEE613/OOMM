namespace BootcampTrainee.Controllers
{
    using System.Web.Mvc;
    using BootcampTrainee.Models;
    using BootcampTrainee.Mapper;
    using BootcampTraineeDBObjects;
    using BootcampTraineeBLL;
    using BootcampTrainee.Filters;

    /// <summary>
    /// This class manages GET and POST actions regarding BoardComment
    /// </summary>
    public class BoardCommentController : Controller
    {
        [HttpPost]
        [MustBeLoggedIn]
        public ActionResult BoardCommentCreate(BoardAndCommentsViewModel iBoardComment)
        {
            // check if all inputs are valid
            if (ModelState.IsValid)
            {
                // Increate Mapper object
                BoardCommentMapper lBoardCommentMapper = new BoardCommentMapper();

                // Map Model.BoardComment to BoardCommentDBO
                BoardCommentDBO lBoardCommentDBO = lBoardCommentMapper.MapBoardCommentToBoardCommentDBO(iBoardComment);

                // Instantiate BoardCommentBLL object
                BoardCommentBLL lBoardCommentBLL = new BoardCommentBLL();

                // Insert into Database and get BoardComment ID PK as return value
                int lResult = lBoardCommentBLL.CreateBoardComment(lBoardCommentDBO);

                // If successfully inserted, redirect to the post 
                if (lResult > 0)
                {
                    // message on success
                    TempData["msg"] = "<script>alert('Successfully Written!');</script>";
                    return RedirectToAction("BoardView", "Board", new { id = lBoardCommentDBO.BoardIDFK });
                }
                else
                {
                    // error message
                    TempData["msg"] = "<script> alert('Error occured while processing your requset. Please try later.') </script>";
                }
            }
            else
            {
                // error message
                TempData["msg"] = "<script> alert('Please all Required Fields.') </script>";
            }

            // redirect to post with error message
            return RedirectToAction("BoardView", "Board", new { id = iBoardComment.BoardComment.BoardIDFK });
        }

        [HttpGet]
        [MustBeLoggedIn]

        public ActionResult BoardCommentRemove(int id, int boardID)
        {
            // Instantiate BLL object
            BoardCommentBLL lBoardCommentBLL = new BoardCommentBLL();

            // get bool as return value if comment is successfully removed
            bool lResult = lBoardCommentBLL.RemoveBoardCommentByBoardCommentID(id);

            if (lResult)
            {
                // message on success
                TempData["msg"] = "<script>alert('Successfully Removed!');</script>";
            }
            else
            {
                // message on failure
                TempData["msg"] = "<script>alert('Your request failed. Please try again');</script>";
            }

            // redirect to the board view
            return RedirectToAction("BoardView", "Board", new { id = boardID });
        }

        [HttpGet]
        [MustBeLoggedIn]
        public ActionResult BoardCommentUpdate(int iBoardCommentID)
        {
            // Instantiate objects
            BoardCommentBLL lBoardCommentBLL = new BoardCommentBLL();
            BoardCommentMapper lBoardCommentMapper = new BoardCommentMapper();

            // Find the BoardComment by boardCommentIDPK
            BoardCommentDBO lBoardCommentDBO = lBoardCommentBLL.FindBoardCommentByBoardCommentID(iBoardCommentID);

            BoardComment lBoardComment = new BoardComment();

            if (lBoardCommentDBO != null)
            {
                // Map DB object to Model and pre-populate the comment
                lBoardComment = lBoardCommentMapper.MapBoardCommentDBOToBoardComment(lBoardCommentDBO);
            }
            else
            {
                // redirect to the post with error message
                TempData["msg"] = "<script> alert('Error occured while processing your request.') </script>";
                return RedirectToAction("BoardView", "Board", new { @id = iBoardCommentID });
            }

            return PartialView("BoardCommentUpdate", lBoardComment);
        }

        [HttpPost]
        [MustBeLoggedIn]
        public ActionResult BoardCommentUpdate(BoardComment iBoardComment)
        {
            // instantiate objects
            BoardCommentBLL lBoardCommentBLL = new BoardCommentBLL();
            BoardCommentMapper lBoardCommentMapper = new BoardCommentMapper();

            // Map Model to DB objects
            BoardCommentDBO lBoardCommentDBO = lBoardCommentMapper.MapBoardCommentToBoardCommentDBO(iBoardComment);

            // Get bool result for updating comment
            bool lResult = lBoardCommentBLL.UpdateBoardCommentByBoardCommentID(lBoardCommentDBO);

            if (lResult)
            {
               TempData["msg"] = "<script> alert('Successfully Updated!') </script>";
                // redirect to the board view
                return RedirectToAction("BoardView", "Board", new { id = iBoardComment.BoardIDFK });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}