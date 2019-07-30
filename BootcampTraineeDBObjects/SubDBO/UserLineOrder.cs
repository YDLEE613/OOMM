namespace BootcampTraineeDBObjects.SubDBO
{
    using System.Collections.Generic;

    public class UserLineOrder
    {
        public int UserOrderIDFK { get; set; }
        public int RestaurantIDFK { get; set; }
        public int IsSandwichRestaurant { get; set; }
        public List<FoodItemDBO> FoodItemsList { get; set; }
        public string FoodItemsIDString { get; set; }
        public string FoodItemsNameString { get; set; }
        public decimal OrderPrice { get; set; }
    }
}
