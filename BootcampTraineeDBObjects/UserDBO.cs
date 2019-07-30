namespace BootcampTraineeDBObjects
{
    using System;

    public class UserDBO
    {
        public int UserIDPK { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserLogInID { get; set; }
        public string UserPassword { get; set; }
        public DateTime UserBirth { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public int UserRoleIDFK { get; set; }
        public string UserRoleName { get; set; }
        public DateTime UserDateCreated { get; set; }
        public DateTime UserDateModified { get; set; }
        public int UserIsActive { get; set; }
    }
}
