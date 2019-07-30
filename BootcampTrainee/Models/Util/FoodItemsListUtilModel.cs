
namespace BootcampTrainee.Models.Util
{
    using BootcampTrainee.Models.SubModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public class FoodItemsListUtilModel : Common
    {
        public int RestaurantIDFK { get; set; }
        public string RestaurantName { get; set; }
        public int IsSandwichRestaurant { get; set; }
        public decimal PerSandwichPrice { get; set; }
        public string RestaurantNotice { get; set; }


        public int UserIDFK { get; set; }

        [Display(Name = "Note")]
        public string UserNote { get; set; }

        // For restaurants that do not offer sandwich
        [Display(Name = "Entree")]
        public string EntreeID { get; set; }
        public string EntreeName { get; set; }
        public List<FoodItem> EntreeList { get; set; }

        public List<FoodItem> SideList { get; set; }

        [Display(Name = "Beverage")]
        public string BeverageID { get; set; }
        public string BeverageName { get; set; }
        public List<FoodItem> BeverageList { get; set; }


        // For restaurants that offer sandwich
        [Display(Name = "Cheese")]
        public string CheeseID { get; set; }
        public string CheeseName { get; set; }
        public List<FoodItem> CheeseList { get; set; }

        [Display(Name = "Meat")]
        public string MeatID { get; set; }
        public string MeatName { get; set; }
        public List<FoodItem> MeatList { get; set; }

        [Display(Name = "Bread")]
        public string BreadID { get; set; }
        public string BreadName { get; set; }
        public List<FoodItem> BreadList { get; set; }

        public List<FoodItem> VeggieList { get; set; }
        public List<FoodItem> CondimentList { get; set; }

    }
}