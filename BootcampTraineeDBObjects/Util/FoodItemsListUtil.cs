namespace BootcampTraineeDBObjects.Util
{
    using System.Collections.Generic;
    using BootcampTraineeDBObjects.SubDBO;

    public class FoodItemsListUtil : Common

    {
        public int RestaurantIDFK { get; set; }
        public string RestaurantName { get; set; }
        public int IsSandwichRestaurant { get; set; }
        public decimal SandwichPrice { get; set; }
        public RestaurantDBO Restaurant { get; set; }
        public int UserIDFK { get; set; }
        public string UserNote { get; set; }
        
        // For restaurants that do not offer sandwich
        public List<FoodItemDBO> EntreeList { get; set; }
        public string EntreeID { get; set; }
        public string EntreeName { get; set; }
        public List<FoodItemDBO> SideList { get; set; }
        public string BeverageID { get; set; }
        public string BeverageName { get; set; }
        public List<FoodItemDBO> BeverageList { get; set; }


        // For restaurants that offer sandwich
        public string CheeseID { get; set; }
        public string CheeseName { get; set; }
        public List<FoodItemDBO> CheeseList { get; set; }
        public string MeatID { get; set; }
        public string MeatName { get; set; }
        public List<FoodItemDBO> MeatList { get; set; }

        public string BreadID { get; set; }
        public string BreadName { get; set; }
        public List<FoodItemDBO> BreadList { get; set; }
        public List<FoodItemDBO> VeggieList { get; set; }
        public List<FoodItemDBO>  CondimentList { get; set; }
    }
}
