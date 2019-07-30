
namespace BootcampTrainee.Models
{
    using System;

    public class UserOrder
    {
        public int UserOrderIDPK { get; set; }
        public int UserIDFK { get; set; }
        public int RestaurantIDFK { get; set; }
        public string RestaurantName { get; set; }
        public string UserNote { get; set; }
        public DateTime DateOrdered { get; set; }
        public double OrderRating { get; set; }
    }
}