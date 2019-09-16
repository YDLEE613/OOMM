namespace BootcampTraineeDBObjects
{
    using System;

    public class RestaurantDBO
    {
        public int RestaurantIDPK { get; set; }
        public string RestaurantName { get; set; }
        public string DayofWeek { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Notice { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; } 
        public int IsActive { get; set; }
        public int IsSandwichRestaurant { get; set; }
        public decimal SandwichPrice { get; set; }
        public double AverageRating { get; set; }
    }
}
