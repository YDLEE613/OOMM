namespace BootcampTraineeDBObjects
{
    using System;
    using System.Collections.Generic;
    using BootcampTraineeDBObjects.SubDBO;

    public class UserOrderRatingDBO
    {
        public int UserOrderRatingIDPK { get; set; }
        public int UserOrderIDFK { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public double Score { get; set; }
        public DateTime DateCreated { get; set; }
        public List<FoodItemDBO> FoodItemList {get;set;}
    }
}
