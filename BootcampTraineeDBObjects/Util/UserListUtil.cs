namespace BootcampTraineeDBObjects.Util
{
    using System.Collections.Generic;

    public class UserListUtil : Common
    {
        public UserListUtil()
        {
            PageSize = 15;
        }
        public List<UserDBO> UserList { get; set; }
        public int PageSize { get; set; }
    }
}
