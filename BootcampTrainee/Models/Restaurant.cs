namespace BootcampTrainee.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public class Restaurant
    {
        public int RestaurantIDPK { get; set; }

        [Required(ErrorMessage = "Please Enter Restaurant Name.")]
        [Display(Name = "Restaurant Name")]
        [MaxLength(20, ErrorMessage = "Restaurant name is too long.")]
        public string RestaurantName { get; set; }

        [Required(ErrorMessage = "Please Select One.")]
        [Display(Name = "Delivery Day")]
        public string DayofWeek { get; set; }

        public List<string> DayList { get; set; }


        [Required(ErrorMessage = "Please Enter Contact Number.")]
        [Display(Name = "Phone")]
        [MaxLength(10)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Contact number must be numeric.")]
        public string Contact { get; set; }

        public string Notice { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^(([^<>()[\]\\.,;:\s@""]+(\.[^<>()[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$",
                            ErrorMessage = "Not in proper format")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Is active?")]
        public int IsActive { get; set; }

        [Required]
        [Display(Name = "Is a Sandwich Restaurant?")]
        public int IsSandwichRestaurant { get; set; }

        [Display(Name = "Price for sandwich")]
        [RegularExpression(@"\d+(\.\d{0,3})?", ErrorMessage = "Please input numbers only")]
        [Range(0.00, 100.00, ErrorMessage = "Price must be between 0.00 and 100.00")]
        public decimal SandwichPrice { get; set; }
    }
}