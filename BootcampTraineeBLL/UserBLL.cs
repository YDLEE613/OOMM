namespace BootcampTraineeBLL
{
    using System.Collections.Generic;
    using BootcampTraineeDBObjects;
    using BootcampTraineeDBObjects.SubDBO;
    using BootcampTraineeDAL;

    /// <summary>
    /// This method manages calling methods in DAL, and getting and returning values from DAL
    /// </summary>
    public class UserBLL
    {

        /// <summary>
        /// Description: This method calls a method in DAL layer to insert user into database
        /// </summary>
        /// <param name="iNewUser">User object with data</param>
        /// <returns>If successfully created, return UserID. Otherwise, 0</returns>
        public int CreateUser(UserDBO iNewUser)
        {
            // Instantiate UserDAL object
            UserDAL lUserDAL = new UserDAL();

            // Insert into Database using UserDAL
            int oResult = lUserDAL.CreateUser(iNewUser);

            return oResult;
        }
        /// <summary>
        /// Description: This method calls a method in DAL layer to retrieve all users frmo database
        /// </summary>
        /// <param name="iSearchString">string to be searched in the list</param>
        /// <returns>List of Users in database</returns>
        public List<UserDBO> GetAllUsers(string iSearchString)
        {
            // Instantiate UserDAL object
            UserDAL lUserDAL = new UserDAL();

            // get list of all users
            List<UserDBO> lUserList = lUserDAL.GetAllUsers(iSearchString);

            return lUserList;
        }
        /// <summary>
        /// Description: This method calls a method in DAL layer to find a user with specifc id
        /// </summary>
        /// <param name="iUserIDPK"></param>
        /// <returns>User with the id</returns>
        public UserDBO FindUserByID(int iUserIDPK)
        {
            // Instantiate UserDAL object
            UserDAL lUserDAL = new UserDAL();

            // get the user with specific id
            UserDBO lUser = lUserDAL.FindUserByID(iUserIDPK);

            return lUser;
        }
        /// <summary>
        /// Description: This method calls a method in DAL layer to check if there is a user using same login ID
        /// </summary>
        /// <param name="iUserLogInID">LogIn ID to be checked</param>
        /// <returns>If there is no user with same id, returns 0. Otherwise, number of users with the same userid</returns>
        public int FindUserByUserLogInID(string iUserLogInID)
        {
            // Instantiate UserDAL object
            UserDAL lUserDAL = new UserDAL();

            // get the user with specific id
            int lIsDuplicate = lUserDAL.FindUserByUserLogInID(iUserLogInID);

            return lIsDuplicate;
        }
        /// <summary>
        /// Description: This method calls a method in DAL layer to update data for a user by User id
        /// </summary>
        /// <param name="iUser">User with the data to be updated</param>
        /// <returns>If successfully updated, return true. Otherwise, false</returns>
        public bool UpdateUserByUserID(UserDBO iUser)
        {
            // Instantiate UserDAL object
            UserDAL lUserDAL = new UserDAL();

            // Update the user
            bool lResult = lUserDAL.UpdateUserByUserID(iUser);

            return lResult;
        }
        /// <summary>
        /// Description: This method calls a method in DAL layer to update a user's status
        /// </summary>
        /// <param name="iUserIDPK">A unique user id</param>
        /// <param name="iIsActive">user status (1: active, 0: inactive)</param>
        /// <returns>If successfully updated, return true. Otherwise, false</returns>
        public bool UpdateUserStatusByUserID(int iUserIDPK, int iIsActive)
        {
            // Instantiate UserDAL object
            UserDAL lUserDAL = new UserDAL();

            // Remove User
            bool lResult = lUserDAL.UpdateUserStatusByUserID(iUserIDPK, iIsActive);
                
            return lResult;
        }
        /// <summary>
        /// Description: This method calls a method in DAL layer to check if user submited valid id and password
        /// </summary>
        /// <param name="iUser">User object with data</param>
        /// <returns>User object with data on success or null on failure</returns>
        public UserDBO FindUserbyLogInIDAndPassword(UserDBO iUser)
        {
            // Instantiate UserDAL object
            UserDAL lUserDAL = new UserDAL();

            // find the user with user login id and password
            UserDBO lUser = lUserDAL.FindUserByLogInIDAndPassword(iUser);

            return lUser;
        }
        /// <summary>
        /// Description: This method calls a method in DAL layer to return list of possible roles
        /// </summary>
        /// <returns>List of roles</returns>
        public List<Role> GetAllRoles()
        {
            // Instantiate UserDAL object
            UserDAL lUserDAL = new UserDAL();

            // Get all roles
            List<Role> lRoles = lUserDAL.GetAllRoles();

            return lRoles;
        }
        /// <summary>
        /// Description: This method calls a method in DAL layer to insert user's rating on food order
        /// </summary>
        /// <param name="iUserOrderRatingDBO">UserOrderRating object with data</param>
        /// <returns>If successfully inserted, returns a unique rating id. Otherwise, 0</returns>
        public int CreateUserOrderRating(UserOrderRatingDBO iUserOrderRatingDBO)
        {
            // Instantiate UserDAL object
            UserDAL lUserDAL = new UserDAL();

            // get IDPK as return value
            int lRatingIDPK = lUserDAL.CreateUserOrderRating(iUserOrderRatingDBO);

            return lRatingIDPK;
        }
        /// <summary>
        /// Description: This method calls a method in DAL layer to remove user with the id
        /// </summary>
        /// <param name="id">A unique user id</param>
        /// <returns>If successfully removed, return true. Otherwise, false</returns>
        public bool RemoveUserByUserID(int id)
        {
            // Instantiate UserDAL object
            UserDAL lUserDAL = new UserDAL();

            // get bool value
            bool lResult = lUserDAL.RemoveUserByUserID(id);

            return lResult;
        }
    }
}
