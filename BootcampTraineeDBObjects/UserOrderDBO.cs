namespace BootcampTraineeDBObjects
{
    using System;
    using System.Collections.Generic;
    using BootcampTraineeDBObjects.SubDBO;

    public class UserOrderDBO
    {
        public int UserOrderIDPK { get; set; }
        public int UserIDFK { get; set; }
        public string UserName { get; set; }
        public int RestaurantIDFK { get; set; }
        public string RestaurantName { get; set; }
        public string UserNote { get; set; }
        public DateTime DateOrdered { get; set; }
        public List<FoodItemDBO> FoodItemList { get; set; }
        public double OrderRating { get; set; }
        public decimal OrderPrice { get; set; }
    }
}
