namespace BootcampTraineeDBObjects.Util
{
    using System.Collections.Generic;
    using BootcampTraineeDBObjects;

    public class UserRestaurantBoardUtil
    {
        // User Data
        public UserDBO UserData { get; set; }

        // Board Data
        public List<BoardDBO> BoardList { get; set; }
        public List<BoardCommentDBO> BoardCommentList { get; set; }

        // Order Data
        public List<UserOrderDBO> OrderList { get; set; }

        public int PageSize { get; set; }
    }
}
