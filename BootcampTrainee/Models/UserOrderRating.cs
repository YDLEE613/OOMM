namespace BootcampTrainee.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserOrderRating
    {
        public int UserOrderIDFK { get; set; }
        public string Content { get; set; }

        [Required(ErrorMessage = "Give a Score!")]
        public double Score { get; set; }
    }
}