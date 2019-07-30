namespace BootcampTrainee.Models.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using BootcampTrainee.Models;
    using BootcampTraineeDBObjects.Util;


    public class UserRestaurantBoardUtil
    {
        // User Data
        public User UserData { get; set; }

        // Board Data
        public List<Board> BoardList { get; set; }
        public List<BoardComment> BoardCommentList { get; set; }

        // Order Data
        public List<UserOrder> OrderList { get; set; } // each order
        //public List<FoodItems> LineOrderList { get; set; } // items in each order
    }
}