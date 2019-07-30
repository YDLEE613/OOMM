namespace BootcampTrainee.Mapper
{
    using System.Collections.Generic;
    using BootcampTraineeDBObjects;
    using BootcampTraineeDBObjects.SubDBO;
    using BootcampTrainee.Models;

    /// <summary>
    /// This class manages mapping from one object or list to another object or list
    /// </summary>
    public class UserMapper
    {
        /// <summary>
        /// Description: This method maps User Model object to User database object
        /// </summary>
        /// <param name="iNewUser">User object to be mapped</param>
        /// <returns>User database object</returns>
        public UserDBO MapUserToUserDBO(User iNewUser)
        {
            UserDBO oNewUser = new UserDBO();

            // set values
            oNewUser.UserIDPK = iNewUser.UserIDPK;
            oNewUser.UserLogInID = iNewUser.UserLogInID;
            oNewUser.UserPassword = iNewUser.UserPassword;
            oNewUser.UserFirstName = iNewUser.UserFirstName;
            oNewUser.UserLastName = iNewUser.UserLastName;
            oNewUser.UserBirth = iNewUser.UserBirth;
            oNewUser.UserEmail = iNewUser.UserEmail;
            oNewUser.UserPhone = iNewUser.UserPhone;
            oNewUser.UserIsActive = iNewUser.UserIsActive;
            oNewUser.UserRoleIDFK = iNewUser.UserRoleIDFK;
            oNewUser.UserRoleName = iNewUser.UserRoleName;
            oNewUser.UserDateCreated = iNewUser.UserDateCreated;
            oNewUser.UserDateModified = iNewUser.UserDateModified;

            return oNewUser;
        }

        /// <summary>
        /// Description: This method maps User database object to User Model object
        /// </summary>
        /// <param name="iNewUser">User database object to be mapped</param>
        /// <returns>User Model object</returns>
        public User MapUserDBOToUser(UserDBO iNewUser)
        {
            User oUser = new User();

            // set values
            oUser.UserIDPK = iNewUser.UserIDPK;
            oUser.UserLogInID = iNewUser.UserLogInID;
            oUser.UserPassword = iNewUser.UserPassword;
            oUser.UserFirstName = iNewUser.UserFirstName;
            oUser.UserLastName = iNewUser.UserLastName;
            oUser.UserBirth = iNewUser.UserBirth;
            oUser.UserDateCreated = iNewUser.UserDateCreated;
            oUser.UserDateModified = iNewUser.UserDateModified;
            oUser.UserRoleIDFK = iNewUser.UserRoleIDFK;
            oUser.UserRoleName = iNewUser.UserRoleName;
            oUser.UserIsActive = iNewUser.UserIsActive;

            // If info is not provided, set value to empty string
            oUser.UserEmail = iNewUser.UserEmail == "Unknown" ? "" : iNewUser.UserEmail;
            oUser.UserPhone = iNewUser.UserPhone == "Unknown" ? "" : iNewUser.UserPhone;

            return oUser;
        }

        /// <summary>
        /// Description: This method maps list of Role database objects to list of Role Model objects
        /// </summary>
        /// <param name="RoleDBOList">List of Role database objects to be mapped</param>
        /// <returns>List of Role Model object</returns>
        public List<Models.SubModel.Role> MapRoleDBOListToRoleModelList(List<Role> RoleDBOList)
        {
            // list to be returned
            List<Models.SubModel.Role> lRoleList = new List<Models.SubModel.Role>();

            // map each Role database object to Role Model object and add to the list
            foreach (Role each in RoleDBOList)
            {
                Models.SubModel.Role role = new Models.SubModel.Role();

                role.RoleIDPK = each.RoleIDPK;
                role.RoleName = each.RoleName;

                lRoleList.Add(role);
            }

            return lRoleList;
        }

        /// <summary>
        /// Description: This method maps UserOrderRating Model object to UserOrderRating database object
        /// </summary>
        /// <param name="iUserOrderRating">UserOrderRating Model to be mapped</param>
        /// <returns>UserOrderRating database object</returns>
        public UserOrderRatingDBO MapOrderRatingModelToOrderRatingDBO(UserOrderRating iUserOrderRating)
        {
            UserOrderRatingDBO lOrderRatingDBO = new UserOrderRatingDBO();

            // set values
            lOrderRatingDBO.UserOrderIDFK = iUserOrderRating.UserOrderIDFK;
            lOrderRatingDBO.Score = iUserOrderRating.Score;
            lOrderRatingDBO.Content = iUserOrderRating.Content;

            return lOrderRatingDBO;
        }
    }
}