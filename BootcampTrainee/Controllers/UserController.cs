namespace BootcampTrainee.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using BootcampTraineeBLL;
    using BootcampTraineeDBObjects;
    using BootcampTraineeDBObjects.Util;
    using BootcampTraineeDBObjects.SubDBO;
    using BootcampTrainee.Mapper;
    using BootcampTrainee.Models;
    using BootcampTrainee.Filters;
    using System.Web.Security;
    using System.Linq;

    /// <summary>
    /// This class manages GET and POST actions regarding User
    /// </summary>
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult UserRegister()
        {
            User lUser = new User();
            return View(lUser);
        }

        [HttpPost]
        public ActionResult UserRegister(User iUser)
        {
            // variable to store return value for query execution
            int lResult = 0;

            // class to set message for users
            Common lCommon = new Common();

            // check if every input is valid
            if (ModelState.IsValid)
            {
                // Instantiate objects
                UserBLL lUserBLL = new UserBLL();
                UserMapper lUserMapper = new UserMapper();

                // Map Models.User to DBObjects.User
                UserDBO lUserDBO = lUserMapper.MapUserToUserDBO(iUser);

                // check if there are duplicates for UserLogInID
                int lIsDuplicate = lUserBLL.FindUserByUserLogInID(lUserDBO.UserLogInID);

                // no duplicate
                if (lIsDuplicate == 0)
                {
                    // insert into the database and get return value of UserIDPK
                    lResult = lUserBLL.CreateUser(lUserDBO);
                }
                else
                {
                    // there is another user using same LogIn id
                    iUser.type = -1;
                    iUser.message = "Please Use another User Name";

                    // return to the view with message
                    return View(iUser);
                }

                // if the new user is inserted into the database with no duplicates
                if (lResult > 0)
                {
                    // To pre-populate login ID and password
                    TempData["UserLogInID"] = iUser.UserLogInID;
                    TempData["UserPassword"] = iUser.UserPassword;

                    // message on success
                    TempData["msg"] = "<script>alert('Registered Successfully');</script>";

                    return RedirectToAction("UserLogIn", "User");
                }
                else
                {
                    // message on failure
                    iUser.type = -1;
                    iUser.message = "You are not registered yet. Please try later.";
                }
            }
            else
            {
                iUser.type = -1;
                iUser.message = "Please Fill all Required Information properly.";

            }

            return View(iUser);
        }

        [HttpGet]
        public ActionResult UserLogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserLogIn(User iUser)
        {

            // Instantiate Objects
            UserMapper lUserMapper = new UserMapper();
            UserBLL lUserBLL = new UserBLL();

            // Map to UserDBO object
            UserDBO lUserDBO = lUserMapper.MapUserToUserDBO(iUser);

            // Check if user is activated and if loginID and password match
            UserDBO lUser = lUserBLL.FindUserbyLogInIDAndPassword(lUserDBO);


            // store UserIDPK, LogInID and Role into session
            if (lUser != null && lUser.UserIsActive != 0)
            {
                // log in success
                Session["AUTHUserIDPK"] = lUser.UserIDPK;
                Session["AUTHUserName"] = lUser.UserLogInID;
                Session["AUTHRole"] = lUser.UserRoleName;

                // message on successful login
                TempData["msg"] = $"<script>alert('Successfully Logged In. Welcome {lUser.UserLogInID}!');</script>";

                // redirect to main page
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (lUser == null)
                {
                    // message for wrong login id or password
                    TempData["msg"] = $"<script>alert('Wrong LogIn ID or wrong password');</script>";
                }
                else
                {
                    // message for not activated users
                    TempData["msg"] = $"<script>alert('Your account is not activated.');</script>";
                }
            }

            return View(iUser);
        }

        [HttpGet]
        [MustBeLoggedIn]
        public ActionResult UserLogOut(User iUser)
        {
            // clear session
            Session.Clear();
            FormsAuthentication.SignOut();

            // redirect to user log in page
            return RedirectToAction("UserLogIn", "User");
        }

        [HttpGet]
        [MustBeLoggedIn]
        public ActionResult UserProfile(int id, Common notice)
        {
            // Instantiate objects
            UserBLL lUserBLL = new UserBLL();
            UserMapper lUserMapper = new UserMapper();
            BoardBLL lBoardBLL = new BoardBLL();
            RestaurantBLL lRestaurantBLL = new RestaurantBLL();
            BoardCommentBLL lBoardCommentBLL = new BoardCommentBLL();

            // Instantiate object to hold all info about user
            UserRestaurantBoardUtil lUserAllData = new UserRestaurantBoardUtil();

            // Get info about user with id
            lUserAllData.UserData = lUserBLL.FindUserByID(id);

            // Get info about board with user id
            lUserAllData.BoardList = lBoardBLL.GetAllBoardsByUserID(id);
            lUserAllData.BoardCommentList = lBoardCommentBLL.GetAllCommentsByUserID(id);

            // Get info about order with user id
            lUserAllData.OrderList = lRestaurantBLL.GetAllOrdersByUserID(id);

            // number of rows per page
            lUserAllData.PageSize = 10;


            return View(lUserAllData);
        }

        [HttpGet]
        [MustBeInRole(Roles = "Manager,Admin")]
        public ActionResult UserList(string searchString)
        {

            // Instantiate Objects
            UserBLL lUserBLL = new UserBLL();
            UserListUtil UserListUtil = new UserListUtil();

            // Get User list
            UserListUtil.UserList = lUserBLL.GetAllUsers(searchString);

            return View(UserListUtil);
        }


        [HttpGet]
        [MustBeLoggedIn] // same user
        public ActionResult UserUpdate(int id)
        {
            // if UserIDPK is not proper, redirect to user list
            if (id < 1)
            {
                TempData["msg"] = "<script>alert('Invalid User');</script>";
                return RedirectToAction("UserList", "User");
            }

            // Valid UserIDPK
            else
            {

                // instantiate objects
                UserBLL lUserBLL = new UserBLL();
                UserMapper lUserMapper = new UserMapper();

                // Get the user from database
                UserDBO lUserEdited = lUserBLL.FindUserByID(id);

                // Map User database object to User Model object
                User lUser = lUserMapper.MapUserDBOToUser(lUserEdited);

                // Get the list of roles 
                List<Role> lRoleList = lUserBLL.GetAllRoles();
                lUser.RoleList = lUserMapper.MapRoleDBOListToRoleModelList(lRoleList);

                return View(lUser);
            }
        }

        [HttpPost]
        [MustBeLoggedIn] 
        public ActionResult UserUpdate(User iUser)
        {
            // Instantiate objects
            UserBLL lUserBLL = new UserBLL();
            UserMapper lUserMapper = new UserMapper();

            // check if every input is valid
            if (ModelState.IsValid)
            {
                // Map Model.User to UserDBO
                UserDBO lUserDBO = lUserMapper.MapUserToUserDBO(iUser);

                // Update the user in database
                bool lResult = lUserBLL.UpdateUserByUserID(lUserDBO);

                if (lResult)
                {
                    // message on success
                    TempData["msg"] = "<script>alert('Successfully Updated!')</script>";

                    // After success, redirect to Profile page
                    return RedirectToAction("UserProfile", "User", new { id = iUser.UserIDPK });
                }
                else
                {
                    // message on failure
                    TempData["msg"] = "<script>alert('Failed to Update. Please try later')</script>";
                }
            }
            else
            {
                // message on invalid model state
                TempData["msg"] = "<script>alert('Please Fill all Required Information properly')</script>";
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }

            // Get the list of roles 
            iUser.RoleList = lUserMapper.MapRoleDBOListToRoleModelList(lUserBLL.GetAllRoles());

            return View(iUser);
        }


        // only for admin, manager
        [HttpGet]
        [MustBeInRole(Roles = "Admin,Manager")]
        public ActionResult UserStatusUpdate(int iUserID, int status)
        {
            // Instantiate objects
            UserBLL lUserBLL = new UserBLL();

            // Find the user with the id to retreive first name and last name
            UserDBO lUserUpdated = lUserBLL.FindUserByID(iUserID);

            // Update the status 
            bool lResult = lUserBLL.UpdateUserStatusByUserID(iUserID, status);

            // Success
            if (lResult)
            {
                // Different messages for Activating/Deactivating
                if (status == 1)
                {
                    TempData["msg"] = "<script>alert('Successfully Deactivated!')</script>";
                }
                else
                {
                    TempData["msg"] = "<script>alert('Successfully Activated!')</script>";
                }
            }
            // failure
            else
            {
                TempData["msg"] = "<script>alert('Error occured while processing your request. Please try later.')</script>";
            }

            
            return RedirectToAction("UserProfile", "User", new { @id = iUserID });
        }

        [HttpGet]
        [MustBeInRole(Roles = "Admin")]
        public ActionResult UserRemove(int iUserID)
        {
            // check if id is valid
            if (iUserID >= 1)
            {
                // Instantiate Objects
                UserBLL lUserBLL = new UserBLL();

                // Get Bool as return value if successfully removed
                bool lResult = lUserBLL.RemoveUserByUserID(iUserID);

                // Success
                if (lResult)
                {
                    // message on success
                    TempData["msg"] = "<script>alert('Successfully Removed!')</script>";
                }
                // Failure
                else
                {
                    // message on failure
                    TempData["msg"] = "<script>alert('Error occured while processing your request. Please try later.');</script>";
                    return RedirectToAction("UserProfile", "User", new { @id = iUserID });
                }
            }
            else
            {
                // message on invalid user
                TempData["msg"] = "<script>Invalid User</script>";
            }

            // Redirect to user list 
            return RedirectToAction("UserList", "User");
        }

        [HttpGet]
        [MustBeLoggedIn]
        public PartialViewResult UserOrderDetails(int id)
        {
            // Instantiate BLL object
            RestaurantBLL lRestaurantBLL = new RestaurantBLL();

            // get list of food items for the order
            List<FoodItemDBO> lFoodItemList = lRestaurantBLL.GetAllFoodItemsByOrderID(id);

            return PartialView("UserOrderDetails", lFoodItemList);
        }

        [HttpGet]
        [MustBeLoggedIn]
        public ActionResult UserOrderRating(int id)
        {
            // Instanitate object
            UserOrderRating lUserRating = new UserOrderRating();

            // set value for order id
            lUserRating.UserOrderIDFK = id;

            return PartialView("UserOrderRating", lUserRating);
        }

        [HttpPost]
        [MustBeLoggedIn]
        public ActionResult UserOrderRating(UserOrderRating iUserRating)
        {
            // Instantiate Objects
            UserBLL lUserBLL = new UserBLL();
            UserMapper lUserMapper = new UserMapper();

            // Mapp Model to DB object
            UserOrderRatingDBO lUserOrderRatingDBO = lUserMapper.MapOrderRatingModelToOrderRatingDBO(iUserRating);

            // Create user rating
            int lRatingIDPK = lUserBLL.CreateUserOrderRating(lUserOrderRatingDBO);

            // success
            if (lRatingIDPK > 0)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
    }
}