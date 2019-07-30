namespace BootcampTraineeDBObjects.SubDBO
{

    public class FoodItemDBO
    {
        public FoodItemDBO()
        {
            // Nothing
        }

        public FoodItemDBO(int iFoodItemIDPK, string iFoodItemName)
        {
            this.FoodItemIDPK = iFoodItemIDPK;
            this.FoodItemName = iFoodItemName;
        }
        public int FoodItemIDPK { get; set; }
        public string FoodItemName { get; set; }
        public int FoodTypeIDFK { get; set; }
        public string FoodTypeName { get; set; }
        public int IngredientTypeIDFK { get; set; }
        public string IngredientTypeName { get; set; }
        public string Description { get; set; }
        public int RestaurantIDFK { get; set; }
        public bool IsSandwichRestaurant { get; set; }
        public bool IsSelected { get; set; }
        public decimal Price { get; set; }
    }
}
