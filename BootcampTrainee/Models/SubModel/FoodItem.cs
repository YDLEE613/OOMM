namespace BootcampTrainee.Models.SubModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class FoodItem
    {
        public int FoodItemIDPK { get; set; }

        [Display(Name = "Food Name")]
        public string FoodItemName { get; set; }
        
        [Display(Name="Food Type")]
        public int FoodTypeSelected { get; set; }

        [Display(Name="Food Ingredient Type")]
        public int IngredientTypeSelected { get; set; }

        // list of food types or ingredients for one food item
        public List<FoodType> FoodTypeList { get; set; }

        public List<IngredientType> IngredientTypeList { get; set; }
        public int RestaurantIDFK {get;set;}
        public bool IsSandwichRestaurant { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }

        [Range(0.00, 100.00, ErrorMessage ="Price must be between 0.00 and 100.00")]
        [RegularExpression(@"\d+(\.\d{0,3})?", ErrorMessage ="Please input numbers only")]
        [Display(Name="Price ($0.00 ~ $100.00)")]
        public decimal Price { get; set; }


    }
}