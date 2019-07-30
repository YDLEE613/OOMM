namespace BootcampTraineeDBObjects.Util
{
    using System.Collections.Generic;
    using BootcampTraineeDBObjects;

    using BootcampTraineeDBObjects.SubDBO;
    public class RestaurantFoodsRatingUtil
    {
        public RestaurantFoodsRatingUtil()
        {
            PageSize = 15;
        }
        public RestaurantDBO Restaurant { get; set; }
        public List<FoodItemDBO> FoodItemList { get; set; }
        public List<UserOrderRatingDBO> RatingList { get; set; }
        public RestaurantRating RestaurantRating { get; set; }
        public int PageSize { get; set; }
    }
}
